//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;


using System.Collections.Generic;
using TK_ECAR.Domain.DomainModel;

namespace TK_ECAR.Domain.Specifications
{
    
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("GeneratedCode","1.0")]
    
    [Serializable]
    public partial class T_G_USUARIOS_DIR_TERRITORIALSpecification : ISpecification<T_G_USUARIOS_DIR_TERRITORIAL>
    {
    
        public Nullable<int> ID_USUARIO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> ID_USUARIOIN
    	{
    		get;
    		set;
    	}
    
        public string ID_DT
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> ID_DTIN
    	{
    		get;
    		set;
    	}
    
    	public string ID_DTContains
    	{
    		get;
    		set;
    	}
    	
    	public string ID_DTStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string ID_DTEndsWith
    	{
    		get;
    		set;
    	}
    
    
        #region Navigation Properties
    
    	public T_G_USUARIOSSpecification T_G_USUARIOS
        {
            get;
    		set;
    	}

        #endregion

    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="T_G_USUARIOS_DIR_TERRITORIALSpecification"/> class.
    	/// </summary>
    	public T_G_USUARIOS_DIR_TERRITORIALSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="T_G_USUARIOS_DIR_TERRITORIALSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public T_G_USUARIOS_DIR_TERRITORIALSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    		this.T_G_USUARIOS = new T_G_USUARIOSSpecification();
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<T_G_USUARIOS_DIR_TERRITORIAL, bool>> GetExpression()
    	{
    		Expression<Func<T_G_USUARIOS_DIR_TERRITORIAL, bool>> expression = x => true;
    
    		if(ID_USUARIO.HasValue)
    			expression = expression.And(x => x.ID_USUARIO == ID_USUARIO.Value);
    
    		if(ID_USUARIOIN != null && ID_USUARIOIN.Count() > 0)
    			expression = expression.And(x => ID_USUARIOIN.Contains(x.ID_USUARIO));
    	
    		if(!string.IsNullOrWhiteSpace(ID_DT))  
    			expression = expression.And(x => x.ID_DT.Equals(ID_DT));
    			
    		if(!string.IsNullOrWhiteSpace(ID_DTContains))  
    			expression = expression.And(x => x.ID_DT.Contains(ID_DTContains));
    			
    		if(!string.IsNullOrWhiteSpace(ID_DTStartsWith))
    			expression = expression.And(x => x.ID_DT.StartsWith(ID_DTStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(ID_DTEndsWith))
    			expression = expression.And(x => x.ID_DT.EndsWith(ID_DTEndsWith));
    
    		if(ID_DTIN != null && ID_DTIN.Count() > 0)
    			expression = expression.And(x => ID_DTIN.Contains(x.ID_DT));
    		
    		//
    		// Navigation properties
    		//
    
    		if(this.T_G_USUARIOS != null)
    		{
    			var subExpression = T_G_USUARIOS.GetExpression();
    			expression = expression.And(x => (new[] { x.T_G_USUARIOS }).AsQueryable().Any(subExpression));
    		}
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(T_G_USUARIOS_DIR_TERRITORIAL entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<T_G_USUARIOS_DIR_TERRITORIAL> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
