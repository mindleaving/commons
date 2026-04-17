using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Commons.Extensions;

namespace Commons.DatabaseTools;

public static class SearchExpressionBuilder
{
    public static Expression<Func<T, bool>> ContainsAll<T>(
        Expression<Func<T, string>> selector,
        params string[] searchTerms)
    {
        if (searchTerms.Length == 0)
            throw new ArgumentException("At least one search term must be passed to " + nameof(ContainsAll));
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
        Expression expression = Expression.Call(selector.Body, containsMethod, Expression.Constant(searchTerms[0], typeof(string)));
        foreach (var searchTerm in searchTerms.Skip(1))
        {
            expression = Expression.AndAlso(
                expression, 
                Expression.Call(selector.Body, containsMethod, Expression.Constant(searchTerm, typeof(string))));
        }
        var result = Expression.Lambda<Func<T,bool>>(expression, selector.Parameters[0]);
        return result;
    }

    public static Expression<Func<T, bool>> ContainsAny<T>(
        Expression<Func<T, string>> selector,
        params string[] searchTerms)
    {
        if (searchTerms.Length == 0)
            throw new ArgumentException("At least one search term must be passed to " + nameof(ContainsAny));
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
        Expression expression = Expression.Call(selector.Body, containsMethod, Expression.Constant(searchTerms[0], typeof(string)));
        foreach (var searchTerm in searchTerms.Skip(1))
        {
            expression = Expression.OrElse(
                expression, 
                Expression.Call(selector.Body, containsMethod, Expression.Constant(searchTerm, typeof(string))));
        }
        var result = Expression.Lambda<Func<T,bool>>(expression, selector.Parameters[0]);
        return result;
    }

    public static Expression<Func<T, bool>> And<T>(params Expression<Func<T, bool>>[] expressions)
    {
        if (!expressions.Any())
            return x => true;
        if (expressions.Length == 1)
            return expressions[0];
        Expression<Func<T, bool>> left = expressions[0];
        for (int i = 1; i < expressions.Length; i++)
        {
            var right = expressions[i];
            var parameterReplacedRight = new ExpressionParameterReplacer(right.Parameters, left.Parameters)
                .Visit(right.Body);
            var andExpression = Expression.AndAlso(left.Body, parameterReplacedRight);
            left = Expression.Lambda<Func<T,bool>>(andExpression, left.Parameters);
        }
        return left;
    }

    public static Expression<Func<T, bool>> Or<T>(params Expression<Func<T, bool>>[] expressions)
    {
        if (!expressions.Any())
            return x => true;
        if (expressions.Length == 1)
            return expressions[0];
        Expression<Func<T, bool>> left = expressions[0];
        for (int i = 1; i < expressions.Length; i++)
        {
            var right = expressions[i];
            var parameterReplacedRight = new ExpressionParameterReplacer(right.Parameters, left.Parameters)
                .Visit(right.Body);
            var andExpression = Expression.OrElse(left.Body, parameterReplacedRight);
            left = Expression.Lambda<Func<T,bool>>(andExpression, left.Parameters);
        }
        return left;
    }

    public static Expression<Func<T, bool>> AnyIntersect<T, TItem>(
        Expression<Func<T, List<TItem>>> collectionSelector,
        IList<TItem> otherCollection)
    {
        if (otherCollection.Count == 0)
            return x => false;
        var containsMethod = typeof(ICollection<TItem>).GetMethod("Contains", [typeof(TItem)])!;
        Expression expression = Expression.Call(collectionSelector.Body, containsMethod, Expression.Constant(otherCollection[0], typeof(TItem)));
        foreach (var item in otherCollection.Skip(1))
        {
            expression = Expression.OrElse(
                expression, 
                Expression.Call(collectionSelector.Body, containsMethod, Expression.Constant(item, typeof(TItem))));
        }
        var result = Expression.Lambda<Func<T,bool>>(expression, collectionSelector.Parameters[0]);
        return result;
    }
}