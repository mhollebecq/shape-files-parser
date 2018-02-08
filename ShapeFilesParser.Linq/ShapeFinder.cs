using ShapeFilesParser.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Linq
{
    class ShapeFinder : ExpressionVisitor
    {
        private Expression expression;
        private List<string> names;

        public ShapeFinder(Expression exp)
        {
            this.expression = exp;
        }

        public List<string> Names
        {
            get
            {
                if (names == null)
                {
                    names = new List<string>();
                    this.Visit(this.expression);
                }
                return this.names;
            }
        }

        protected override Expression VisitBinary(BinaryExpression be)
        {
            if(be.NodeType == ExpressionType.Equal)
            {
                var typeofRecord = typeof(Record<>);
                if (ExpressionTreeHelpers.IsMemberEqualsValueExpression(be, typeofRecord, "Index"))
                {
                    names.Add(ExpressionTreeHelpers.GetValueFromEqualsExpression<int>(be, typeofRecord, "Index").ToString());
                    return be;
                }
                return base.VisitBinary(be);
            }
            
            return base.VisitBinary(be);
        }
    }
}
