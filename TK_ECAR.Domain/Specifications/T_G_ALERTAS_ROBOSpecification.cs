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
    public partial class T_G_ALERTAS_ROBOSpecification : ISpecification<T_G_ALERTAS_ROBO>
    {
    
        public Nullable<int> ID_ALERTA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> ID_ALERTAIN
    	{
    		get;
    		set;
    	}
    
        public Nullable<System.DateTime> FECHA_ROBO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> FECHA_ROBOIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> FECHA_ROBOFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> FECHA_ROBOTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> FECHA_ROBOFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> FECHA_ROBOToOrNull
        {
            get;
            set;
        }
    		
    
        #region Navigation Properties
    
    	public T_G_ALERTASSpecification T_G_ALERTAS
        {
            get;
    		set;
    	}

        #endregion

    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="T_G_ALERTAS_ROBOSpecification"/> class.
    	/// </summary>
    	public T_G_ALERTAS_ROBOSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="T_G_ALERTAS_ROBOSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public T_G_ALERTAS_ROBOSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    		this.T_G_ALERTAS = new T_G_ALERTASSpecification();
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<T_G_ALERTAS_ROBO, bool>> GetExpression()
    	{
    		Expression<Func<T_G_ALERTAS_ROBO, bool>> expression = x => true;
    
    		if(ID_ALERTA.HasValue)
    			expression = expression.And(x => x.ID_ALERTA == ID_ALERTA.Value);
    
    		if(ID_ALERTAIN != null && ID_ALERTAIN.Count() > 0)
    			expression = expression.And(x => ID_ALERTAIN.Contains(x.ID_ALERTA));
    
    		if(FECHA_ROBO.HasValue)
    			expression = expression.And(x => x.FECHA_ROBO == FECHA_ROBO.Value);
    
    		if(FECHA_ROBOIN != null && FECHA_ROBOIN.Count() > 0)
    			expression = expression.And(x => FECHA_ROBOIN.Contains(x.FECHA_ROBO));
    	
    		if(FECHA_ROBOFrom.HasValue)
    			expression = expression.And(x => x.FECHA_ROBO >= FECHA_ROBOFrom.Value);
    			
    		if(FECHA_ROBOTo.HasValue)
    			expression = expression.And(x => x.FECHA_ROBO <= FECHA_ROBOTo.Value);
    				
    
    		if(FECHA_ROBOFromOrNull.HasValue)
                expression = expression.And(x => x.FECHA_ROBO >= FECHA_ROBOFromOrNull.Value || x.FECHA_ROBO == null);
    
            if(FECHA_ROBOToOrNull.HasValue)
                expression = expression.And(x => x.FECHA_ROBO <= FECHA_ROBOToOrNull.Value || x.FECHA_ROBO == null);
    		
    		//
    		// Navigation properties
    		//
    
    		if(this.T_G_ALERTAS != null)
    		{
    			var subExpression = T_G_ALERTAS.GetExpression();
    			expression = expression.And(x => (new[] { x.T_G_ALERTAS }).AsQueryable().Any(subExpression));
    		}
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(T_G_ALERTAS_ROBO entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<T_G_ALERTAS_ROBO> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
