using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Linq
{
    class ShapeQueryContext
    {
        internal static object Execute(Expression expression, bool isEnumerable, string sourceName)
        {
            if (!IsQueryOverDataSource(expression))
                throw new InvalidProgramException("No query over the data source was specified");

            InnerMostWhereFinder whereFinder = new InnerMostWhereFinder();
            MethodCallExpression whereExpression = whereFinder.GetInnermostWhere(expression);
            LambdaExpression lambdaExpression = (LambdaExpression)((UnaryExpression)(whereExpression.Arguments[1])).Operand;

            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);
            ShapeFinder sf = new ShapeFinder(lambdaExpression.Body);
            List<string> names = sf.Names;
            if (names.Count == 0)
                throw new InvalidQueryException("You must specifiy at least one Index in your query");
            throw new NotImplementedException();
        }
        
        private static bool IsQueryOverDataSource(Expression expression)
        {
            return (expression is MethodCallExpression);
        }
    }
}
