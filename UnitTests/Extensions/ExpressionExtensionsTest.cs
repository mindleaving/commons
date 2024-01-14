using NUnit.Framework;
using System;
using System.Linq.Expressions;
using Commons.Extensions;

namespace CommonsTest.Extensions;

internal class ExpressionExtensionsTest
{
    [Test]
    public void IsAlwaysTrueReturnsTrueForConstantTrue()
    {
        Expression<Func<object, bool>> expression = x => true;
        Assert.That(expression.IsAlwaysTrue());
    }

    [Test]
    public void IsAlwaysTrueReturnsFalseForConstantFalse()
    {
        Expression<Func<object, bool>> expression = x => false;
        Assert.That(expression.IsAlwaysTrue(), Is.False);
    }

    [Test]
    public void IsAlwaysFalseReturnsTrueForConstantFalse()
    {
        Expression<Func<object, bool>> expression = x => false;
        Assert.That(expression.IsAlwaysFalse());
    }

    [Test]
    public void IsAlwaysFalseReturnsFalseForConstantTrue()
    {
        Expression<Func<object, bool>> expression = x => true;
        Assert.That(expression.IsAlwaysFalse(), Is.False);
    }
}

