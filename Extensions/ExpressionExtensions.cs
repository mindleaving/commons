using System;
using System.Linq.Expressions;

namespace Commons.Extensions
{
    public static class ExpressionExtensions
    {
        public static bool IsAlwaysTrue<T>(
            this Expression<Func<T, bool>> expression)
        {
            if (expression.Body.NodeType != ExpressionType.Constant)
                return false;
            var constantExpression = (ConstantExpression)expression.Body;
            return constantExpression.Value is bool b && b == true;
        }

        public static bool IsAlwaysFalse<T>(
            this Expression<Func<T, bool>> expression)
        {
            if (expression.Body.NodeType != ExpressionType.Constant)
                return false;
            var constantExpression = (ConstantExpression)expression.Body;
            return constantExpression.Value is bool b && b == false;
        }
    }
}
