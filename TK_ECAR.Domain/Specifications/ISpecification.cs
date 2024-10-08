using System;
using System.Linq;
using System.Linq.Expressions;

namespace TK_ECAR.Domain.Specifications
{
    public interface ISpecification
    {

    }
    /// <summary>
    /// Specification design pattern.
    /// The specification pattern is a particular software design pattern, 
    /// whereby business logic can be recombined by chaining the business logic together using boolean logic.
    /// </summary>
    /// <typeparam name="T">Entity type of the specification</typeparam>
    /// <see cref="http://en.wikipedia.org/wiki/Specification_pattern"/>
    public interface ISpecification<T> : ISpecification //where T : ISignatureObject
    {
        /// <summary>
        /// Returns the expression that defines the specification 
        /// </summary>
        Expression<Func<T, bool>> GetExpression();

        /// <summary>
        /// Determines whether the specification is satisfied by an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if the specification is satisfied by an entity; otherwise, <c>false</c></returns>
        bool IsSatisfiedBy(T entity);
    }
}
