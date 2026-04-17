using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Commons.Extensions;

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

        foreach (var task in tasks) // Wait for tasks in the order they were created
        {
            var completedTask = await Task.WhenAny(task);
            if (completedTask.IsFaulted)
            {
                throw completedTask.Exception!.InnerException!;
            }
            yield return completedTask.Result;
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

        foreach (var task in tasks) // Wait for tasks in the order they were created
        {
            var completedTask = await Task.WhenAny(task);
            if (completedTask.IsFaulted)
            {
                cancellationTokenSource.Cancel();
                throw completedTask.Exception!.InnerException!;
            }
            yield return completedTask.Result;
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

        for (var taskIndex = 0; taskIndex < tasks.Count; taskIndex++)
        {
            var task = tasks[taskIndex];
            var completedTask = await Task.WhenAny(task);
            if (completedTask.Result)
                yield return itemsList[taskIndex];
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

    public static async Task<Queue<T>> ToQueueAsync<T>(
        this IAsyncEnumerable<T> items)
    {
        var queue = new Queue<T>();
        await foreach (var item in items)
        {
            queue.Enqueue(item);
        }
        return queue;
    }

    public static async Task<Dictionary<TKey, List<T>>> GroupIntoDictionaryAsync<TKey, T>(
        this IAsyncEnumerable<T> items,
        Func<T, TKey> keySelector)
    {
        var dictionary = new Dictionary<TKey, List<T>>();
        await foreach (var item in items)
        {
            var key = keySelector(item);
            if(!dictionary.ContainsKey(key))
                dictionary.Add(key, []);
            dictionary[key].Add(item);
        }
        return dictionary;
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