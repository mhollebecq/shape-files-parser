using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFilesParser.Linq
{
    public abstract class QueryProvider : IQueryProvider
    {
        protected QueryProvider()
        {

        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Query<TElement>(this, expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return this.Execute<TResult>(expression);
        }

        public abstract string GetQueryText(Expression expression);

        public abstract object Execute(Expression expression);

        public abstract TResult Execute<TResult>(Expression expression);
    }
}
