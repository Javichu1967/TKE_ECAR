
using System;
using System.Linq;
using System.Linq.Expressions;
using TK_ECAR.Domain.DomainModel;


namespace TK_ECAR.Domain.Specifications
{
    /// <summary>
    /// Creates new specifications
    /// </summary>
    /// <typeparam name="T">Entity type of the specification</typeparam>
    /// <see cref="http://www.codeinsanity.com/2008/08/implementing-repository-and.html"/>
    [System.CodeDom.Compiler.GeneratedCode("GeneratedCode", "1.0")]
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Combines 2 specifications (And operation)
        /// </summary>
        /// <typeparam name="T">The Specification object type</typeparam>
        /// <param name="first">The first specification</param>
        /// <param name="second">The second specification</param>
        /// <returns>A new specification that combines the 2 specifications passed as parameter (And operation)</returns>
        public static ISpecification<T> And<T>(this ISpecification<T> first, ISpecification<T> second) where T : class
        {
            return new Specification<T>(
                first.GetExpression()
                .And(second.GetExpression()
                ));
        }

        /// <summary>
        /// Combines 2 specifications (Or operation)
        /// </summary>
        /// <typeparam name="T">The Specification object type</typeparam>
        /// <param name="first">The first specification</param>
        /// <param name="second">The second specification</param>
        /// <returns>A new specification that combines the 2 specifications passed as parameter (Or operation)</returns>
        public static ISpecification<T> Or<T>(this ISpecification<T> first, ISpecification<T> second) where T : class
        {
            
            return new Specification<T>(
                first.GetExpression()
                .Or(second.GetExpression()
                ));
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> first) where T : class
        {
            return new Specification<T>(Negate(first.GetExpression()));
        }

        private static Expression<TDelegate> Negate<TDelegate>(Expression<TDelegate> expression)
        {
            return Expression.Lambda<TDelegate>(Expression.Not(expression.Body), expression.Parameters);
        }

      
    }
}
