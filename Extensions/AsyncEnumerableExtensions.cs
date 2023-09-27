using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    public static class AsyncEnumerableExtensions
    {
        public static async IAsyncEnumerable<TResult> SelectParallelAsync<T, TResult>(
            this IEnumerable<T> items,
            Func<T, Task<TResult>> asyncAction)
        {
            var tasks = new List<Task<TResult>>();
            foreach (var item in items)
            {
                var task = asyncAction(item);
                tasks.Add(task);
            }

            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);
                yield return completedTask.Result;
                tasks.Remove(completedTask);
            }
        }

        public static async IAsyncEnumerable<TResult> SelectParallelAsync<T, TResult>(
            this IAsyncEnumerable<T> items,
            Func<T, CancellationToken, Task<TResult>> asyncAction)
        {
            var tasks = new List<Task<TResult>>();
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            await foreach (var item in items.WithCancellation(cancellationToken))
            {
                var task = asyncAction(item, cancellationToken);
                tasks.Add(task);
            }

            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);
                if (completedTask.IsFaulted)
                {
                    cancellationTokenSource.Cancel();
                    throw completedTask.Exception.InnerException;
                }
                yield return completedTask.Result;
                tasks.Remove(completedTask);
            }
        }

        public static async IAsyncEnumerable<TResult> SelectAsync<T, TResult>(
            this IEnumerable<T> items,
            Func<T, Task<TResult>> asyncAction)
        {
            foreach (var item in items)
                yield return await asyncAction(item);
        }
        public static async IAsyncEnumerable<TResult> SelectAsync<T, TResult>(
            this IAsyncEnumerable<T> items,
            Func<T, Task<TResult>> asyncAction)
        {
            await foreach (var item in items)
                yield return await asyncAction(item);
        }
        public static async IAsyncEnumerable<TResult> Select<T, TResult>(
            this IAsyncEnumerable<T> items,
            Func<T, TResult> action)
        {
            await foreach (var item in items)
                yield return action(item);
        }

        public static async IAsyncEnumerable<TCast> Cast<TCast>(
            this IAsyncEnumerable<object> items)
        {
            await foreach(var item in items)
                yield return (TCast)item;
        }

        public static async IAsyncEnumerable<T> WhereParallelAsync<T>(
            this IEnumerable<T> items,
            Func<T, Task<bool>> asyncAction)
        {
            var itemsList = new List<T>();
            var tasks = new List<Task<bool>>();
            foreach (var item in items)
            {
                itemsList.Add(item);
                var task = asyncAction(item);
                tasks.Add(task);
            }

            var completedTaskCount = 0;
            while (completedTaskCount < tasks.Count)
            {
                var completedTask = await Task.WhenAny(tasks);
                var taskIndex = tasks.IndexOf(completedTask);
                if(completedTask.Result)
                    yield return itemsList[taskIndex];
                completedTaskCount++;
            }
        }


        public static async IAsyncEnumerable<T> WhereAsync<T>(
            this IEnumerable<T> items,
            Func<T, Task<bool>> asyncAction)
        {
            foreach (var item in items)
            {
                var isMatch = await asyncAction(item);
                if (isMatch)
                    yield return item;
            }
        }

        public static async IAsyncEnumerable<T> WhereAsync<T>(
            this IAsyncEnumerable<T> items,
            Func<T, Task<bool>> asyncAction)
        {
            await foreach (var item in items)
            {
                var isMatch = await asyncAction(item);
                if (isMatch)
                    yield return item;
            }
        }

        public static async IAsyncEnumerable<T> Where<T>(
            this IAsyncEnumerable<T> items,
            Func<T, bool> filter)
        {
            await foreach (var item in items)
            {
                var isMatch = filter(item);
                if (isMatch)
                    yield return item;
            }
        }

        public static async Task<bool> AnyAsync<T>(
            this IEnumerable<T> items,
            Func<T, Task<bool>> asyncAction)
        {
            foreach (var item in items)
            {
                var isMatch = await asyncAction(item);
                if (isMatch)
                    return true;
            }

            return false;
        }

        public static async Task<bool> AnyAsync<T>(
            this IAsyncEnumerable<T> items,
            Func<T, Task<bool>> asyncAction)
        {
            await foreach (var item in items)
            {
                var isMatch = await asyncAction(item);
                if (isMatch)
                    return true;
            }

            return false;
        }

        public static async Task<List<T>> ToListAsync<T>(
            this IAsyncEnumerable<T> items)
        {
            var list = new List<T>();
            await foreach (var item in items)
            {
                list.Add(item);
            }
            return list;
        }
        public static async Task<T> FirstOrDefaultAsync<T>(
            this IAsyncEnumerable<T> items)
        {
            await foreach (var item in items)
            {
                return item;
            }
            return default;
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(
            this IEnumerable<T> items,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }
}
