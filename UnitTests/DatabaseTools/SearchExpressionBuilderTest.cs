using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Commons.DatabaseTools;
using NUnit.Framework;

namespace CommonsTest.DatabaseTools;

public class SearchExpressionBuilderTest
{
    [Test]
    public void CanBuildAnyIntersect()
    {
        var item = new ClassWithNumberArray
        {
            Numbers = [ 1, 4, 8, -3 ]
        };
        var collection1 = new[] { 2, 8, 44 }; 
        
        var expression1 = SearchExpressionBuilder.AnyIntersect<ClassWithNumberArray, int>(x => x.Numbers, collection1);
        Assert.That(expression1.Compile().Invoke(item), Is.True);
        
        var collection2 = new[] { 2, 7, 44 };
        var expression2 = SearchExpressionBuilder.AnyIntersect<ClassWithNumberArray, int>(x => x.Numbers, collection2);
        Assert.That(expression2.Compile().Invoke(item), Is.False);
    }
    
    private class ClassWithNumberArray
    {
        public List<int> Numbers { get; set; }
    }
}