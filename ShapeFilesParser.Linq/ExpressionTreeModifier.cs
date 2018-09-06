using ShapeFilesParser.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Linq
{
    public class ExpressionTreeModifier : ExpressionVisitor
    {
        private IQueryable queryableRecords;

        internal ExpressionTreeModifier(IQueryable records)
        {
            this.queryableRecords = records;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            // Replace the constant QueryableTerraServerData arg with the queryable Place collection. 
            if (c.Type.Name == typeof(Query<>).Name)
                return Expression.Constant(this.queryableRecords);
            else
                return c;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
        }
    }
}
