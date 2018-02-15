using ShapeFilesParser.Business;
using ShapeFilesParser.Business.Models;
using ShapeFilesParser.Business.Parsers;
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
            List<int> ids = sf.Ids;
            if (ids.Count == 0)
                throw new InvalidQueryException("You must specifiy at least one Index in your query");

            var recordType = expression.Type.GenericTypeArguments[0];// Record<T>
            var shapeType = recordType.GenericTypeArguments[0];// Shape type
            var objects = GetShapesFromType(shapeType, sourceName, ids);

            throw new NotImplementedException();
        }

        private static object GetShapesFromType(Type shapeType, string sourceName, List<int> ids)
        {
            ShapeManager shapeManager = new ShapeManager();
            ShapeIndexList shapeIndexList = shapeManager.GetInfos(sourceName);
            if (shapeType == typeof(Point))
            {
                return ids.Select(id => shapeManager.GetShape(sourceName, new PointParser(), shapeIndexList[id]));
            }
            else if (shapeType == typeof(PointZ))
            {
                return ids.Select(id => shapeManager.GetShape(sourceName, new PointZParser(), shapeIndexList[id])).ToList() ;
            }

            return null;
        }

        private static bool IsQueryOverDataSource(Expression expression)
        {
            return (expression is MethodCallExpression);
        }
    }
}
