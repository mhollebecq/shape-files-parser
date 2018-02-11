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
        string _sourceName;

        public ShapeQueryProvider(string sourceName)
        {
            _sourceName = sourceName;
        }

        public override object Execute(Expression expression)
        {
            return ShapeQueryContext.Execute(expression, false, _sourceName);
        }

        public override TResult Execute<TResult>(Expression expression)
        {
            bool isEnumerable = (typeof(TResult).Name == "IEnumerable`1");

            return (TResult)ShapeQueryContext.Execute(expression, isEnumerable, _sourceName);
        }

        public override string GetQueryText(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
