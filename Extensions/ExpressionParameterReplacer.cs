﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Commons.Extensions
{
    /// <summary>
    /// From: https://stackoverflow.com/questions/9231569/exception-using-orelse-and-andalso-expression-methods
    /// </summary>
    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> parameterMap;

        public ExpressionParameterReplacer(
            IEnumerable<ParameterExpression> inputParameters,
            IEnumerable<ParameterExpression> outputParameters)
        {
            parameterMap = inputParameters
                .Zip(outputParameters, (a,b) => new { First = a, Second = b })
                .ToDictionary(x => x.First, x => x.Second);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            var replacedNode = parameterMap.TryGetValue(node, out var outputParameter) 
                ? outputParameter 
                : node;
            return base.VisitParameter(replacedNode);
        }
    }
}