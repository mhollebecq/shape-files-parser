using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Linq
{
    public class ShapeQueryProvider : QueryProvider
    {
        public override object Execute(Expression expression)
        {
            return ShapeQueryContext.Execute(expression, false);
        }

        public override TResult Execute<TResult>(Expression expression)
        {
            bool isEnumerable = (typeof(TResult).Name == "IEnumerable`1");

            return (TResult)ShapeQueryContext.Execute(expression, isEnumerable);
        }

        public override string GetQueryText(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
