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
            var enumerableRecords = GetShapesFromType(shapeType, sourceName, ids);

            var queryableRecords = enumerableRecords.AsQueryable();

            ExpressionTreeModifier treeCopier = new ExpressionTreeModifier(queryableRecords);
            Expression newExpressionTree = treeCopier.Visit(expression);

            if (isEnumerable)
                return queryableRecords.Provider.CreateQuery(newExpressionTree);
            else
                return queryableRecords.Provider.Execute(newExpressionTree);
        }

        private static System.Collections.IEnumerable GetShapesFromType(Type shapeType, string sourceName, List<int> ids)
        {
            ShapeManager shapeManager = new ShapeManager();
            ShapeIndexList shapeIndexList = shapeManager.GetInfos(sourceName);
            GeometryType geometryType = GetGeometryTypeEnumFromClrType(shapeType);
            if (geometryType != shapeIndexList.GlobalShapeType)
                throw new ArgumentException($"source file contains {shapeIndexList.GlobalShapeType} instead of {geometryType}");
            var filteredId = ids.Where(id => id > 0 && id <= shapeIndexList.Count).Select(id => id - 1);
            System.Collections.IEnumerable enumerable = null;
            switch (geometryType)
            {
                case GeometryType.Point:
                    enumerable= filteredId.Select(id => shapeManager.GetShape(sourceName, new PointParser(), shapeIndexList[id]));
                    break;
                case GeometryType.PolyLine:
                    break;
                case GeometryType.Polygon:
                    break;
                case GeometryType.MultiPoint:
                    break;
                case GeometryType.PointZ:
                    enumerable = filteredId.Select(id => shapeManager.GetShape(sourceName, new PointZParser(), shapeIndexList[id])).ToList();
                    break;
                case GeometryType.PolyLineZ:
                    break;
                case GeometryType.PolygonZ:
                    break;
                case GeometryType.MultiPointZ:
                    break;
                case GeometryType.PointM:
                    break;
                case GeometryType.PolyLineM:
                    break;
                case GeometryType.PolygonM:
                    break;
                case GeometryType.MultiPointM:
                    break;
                case GeometryType.MultiPath:
                    break;
                default:
                    break;
            }

            return enumerable;
        }

        private static GeometryType GetGeometryTypeEnumFromClrType(Type shapeType)
        {
            if (shapeType == typeof(Point))
                return GeometryType.Point;
            else if (shapeType == typeof(PointZ))
                return GeometryType.PointZ;
            //else if (shapeType == typeof(PointM))
            //    return GeometryType.PointM;
            else if (shapeType == typeof(Polygon))
                return GeometryType.Polygon;
            else if (shapeType == typeof(PolygonZ))
                return GeometryType.PolygonZ;
            //else if (shapeType == typeof(PolygonM))
            //    return GeometryType.PolygonM;
            else if (shapeType == typeof(Polyline))
                return GeometryType.PolyLine;
            else if (shapeType == typeof(PolylineZ))
                return GeometryType.PolyLineZ;
            //else if (shapeType == typeof(PolylineM))
            //    return GeometryType.PolyLineM;

            throw new ArgumentException($"expected type {shapeType} can not be retrieved", "shapeType");
        }

        private static bool IsQueryOverDataSource(Expression expression)
        {
            return (expression is MethodCallExpression);
        }
    }
}
