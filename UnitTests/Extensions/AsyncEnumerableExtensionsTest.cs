using System.Threading;
using System.Threading.Tasks;
using Commons.Extensions;
using NUnit.Framework;

namespace CommonsTest.Extensions;

public class AsyncEnumerableExtensionsTest
{
    [Test]
    public async Task SelectParallelPreservesOrder()
    {
        var items = new[] { 200, 500, 100 };

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var actual = await items.ToAsyncEnumerable(cancellationToken)
            .SelectParallelAsync(async (number, cancellationToken) =>
            {
                await Task.Delay(number, cancellationToken);
                return number;
            })
            .ToListAsync();
        Assert.That(actual, Is.EqualTo(items));
    }

    [Test]
    public async Task WhereParallelPreservesOrder()
    {
        var items = new[] { 200, 500, 100 };

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var actual = await items
            .WhereParallelAsync(async (number) =>
            {
                await Task.Delay(number, cancellationToken);
                return true;
            })
            .ToListAsync();
        Assert.That(actual, Is.EqualTo(items));
    }

}