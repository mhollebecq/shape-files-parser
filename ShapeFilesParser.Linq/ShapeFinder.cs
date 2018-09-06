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
        private List<int> ids;

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


        public List<int> Ids
        {
            get
            {
                if (ids == null)
                {
                    ids = new List<int>();
                    this.Visit(this.expression);
                }
                return this.ids;
            }
        }


        protected override Expression VisitBinary(BinaryExpression be)
        {
            if(be.NodeType == ExpressionType.Equal)
            {
                var typeofRecord = typeof(Record<>);
                if (ExpressionTreeHelpers.IsMemberEqualsValueExpression(be, typeofRecord, "Index"))
                {
                    ids.Add(ExpressionTreeHelpers.GetValueFromEqualsExpression<int>(be, typeofRecord, "Index"));
                    return be;
                }
                else if(ExpressionTreeHelpers.IsMethodCallEqualsValueExpression(be,
                                                                                typeofRecord, 
                                                                                "Metadata",
                                                                                "get_Item"))
                {
                    var temp = ExpressionTreeHelpers.GetValueFromMethodCallExpression<string>(be, typeofRecord,
                                                                                "Metadata",
                                                                                "get_Item");
                }
                return base.VisitBinary(be);
            }
            
            return base.VisitBinary(be);
        }
    }
}
