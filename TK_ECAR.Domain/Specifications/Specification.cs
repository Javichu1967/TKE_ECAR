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
    public class Specification<T> : ISpecification<T> where T : class
    {
        /// <summary>
        /// Gets the specification predicate.
        /// </summary>
        private readonly Expression<Func<T, bool>> expression;

        public Expression<Func<T, bool>> GetExpression()
        {
            return expression;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="expression">The specification lambda expression.</param>
        public Specification(Expression<Func<T, bool>> expression)
        {
            this.expression = expression;
        }

        public override string ToString()
        {
            return   Evaluator.PartialEval(expression).ToString();
        }

        /// <summary>
        /// Determines whether the specification is satisfied by the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if the specification is satisfied by the specified entity; otherwise, <c>false</c></returns>
        public bool IsSatisfiedBy(T entity)
        {
            var query = (new[] { entity }).AsQueryable();

            return query.Any(this.expression);
        }
    }
}
