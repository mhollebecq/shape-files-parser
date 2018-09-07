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
        private List<int> ids;
        private List<(string key, string value)> metadata;

        public ShapeFinder(Expression exp)
        {
            this.expression = exp;
        }


        public List<int> Ids
        {
            get
            {
                if (ids == null)
                {
                    InitializeFieldsAndVisit();
                }
                return this.ids;
            }
        }

        public List<(string key,string value)> Metadata
        {
            get
            {
                if(metadata == null)
                {
                    InitializeFieldsAndVisit();
                }
                return this.metadata;
            }
        }

        protected void InitializeFieldsAndVisit()
        {
            ids = new List<int>();
            metadata = new List<(string key, string value)>();
            this.Visit(this.expression);
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
                    metadata.Add(ExpressionTreeHelpers.GetValueFromMethodCallExpression<string,string>(be, typeofRecord,
                                                                                "Metadata",
                                                                                "get_Item"));
                    return be;
                }
                return base.VisitBinary(be);
            }
            
            return base.VisitBinary(be);
        }
    }
}
