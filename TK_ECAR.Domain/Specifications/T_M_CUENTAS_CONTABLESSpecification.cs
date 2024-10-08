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
    public partial class T_M_CUENTAS_CONTABLESSpecification : ISpecification<T_M_CUENTAS_CONTABLES>
    {
    
        public string CUENTA_CONTABLE
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> CUENTA_CONTABLEIN
    	{
    		get;
    		set;
    	}
    
    	public string CUENTA_CONTABLEContains
    	{
    		get;
    		set;
    	}
    	
    	public string CUENTA_CONTABLEStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string CUENTA_CONTABLEEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string NOMBRE_CUENTA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> NOMBRE_CUENTAIN
    	{
    		get;
    		set;
    	}
    
    	public string NOMBRE_CUENTAContains
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_CUENTAStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_CUENTAEndsWith
    	{
    		get;
    		set;
    	}
    
    
        #region Navigation Properties
    
    	public T_R_TIPOSCOSTE_CUENTA_CONTABLESpecification T_R_TIPOSCOSTE_CUENTA_CONTABLE
        {
            get;
    		set;
    	}

        #endregion

    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="T_M_CUENTAS_CONTABLESSpecification"/> class.
    	/// </summary>
    	public T_M_CUENTAS_CONTABLESSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="T_M_CUENTAS_CONTABLESSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public T_M_CUENTAS_CONTABLESSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    		this.T_R_TIPOSCOSTE_CUENTA_CONTABLE = new T_R_TIPOSCOSTE_CUENTA_CONTABLESpecification();
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<T_M_CUENTAS_CONTABLES, bool>> GetExpression()
    	{
    		Expression<Func<T_M_CUENTAS_CONTABLES, bool>> expression = x => true;
    	
    		if(!string.IsNullOrWhiteSpace(CUENTA_CONTABLE))  
    			expression = expression.And(x => x.CUENTA_CONTABLE.Equals(CUENTA_CONTABLE));
    			
    		if(!string.IsNullOrWhiteSpace(CUENTA_CONTABLEContains))  
    			expression = expression.And(x => x.CUENTA_CONTABLE.Contains(CUENTA_CONTABLEContains));
    			
    		if(!string.IsNullOrWhiteSpace(CUENTA_CONTABLEStartsWith))
    			expression = expression.And(x => x.CUENTA_CONTABLE.StartsWith(CUENTA_CONTABLEStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(CUENTA_CONTABLEEndsWith))
    			expression = expression.And(x => x.CUENTA_CONTABLE.EndsWith(CUENTA_CONTABLEEndsWith));
    
    		if(CUENTA_CONTABLEIN != null && CUENTA_CONTABLEIN.Count() > 0)
    			expression = expression.And(x => CUENTA_CONTABLEIN.Contains(x.CUENTA_CONTABLE));
    	
    		if(!string.IsNullOrWhiteSpace(NOMBRE_CUENTA))  
    			expression = expression.And(x => x.NOMBRE_CUENTA.Equals(NOMBRE_CUENTA));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_CUENTAContains))  
    			expression = expression.And(x => x.NOMBRE_CUENTA.Contains(NOMBRE_CUENTAContains));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_CUENTAStartsWith))
    			expression = expression.And(x => x.NOMBRE_CUENTA.StartsWith(NOMBRE_CUENTAStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(NOMBRE_CUENTAEndsWith))
    			expression = expression.And(x => x.NOMBRE_CUENTA.EndsWith(NOMBRE_CUENTAEndsWith));
    
    		if(NOMBRE_CUENTAIN != null && NOMBRE_CUENTAIN.Count() > 0)
    			expression = expression.And(x => NOMBRE_CUENTAIN.Contains(x.NOMBRE_CUENTA));
    		
    		//
    		// Navigation properties
    		//
    
    		if(this.T_R_TIPOSCOSTE_CUENTA_CONTABLE != null)
    		{
    			var subExpression = T_R_TIPOSCOSTE_CUENTA_CONTABLE.GetExpression();
    			expression = expression.And(x => x.T_R_TIPOSCOSTE_CUENTA_CONTABLE.AsQueryable().Any(subExpression));
    		}
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(T_M_CUENTAS_CONTABLES entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<T_M_CUENTAS_CONTABLES> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
