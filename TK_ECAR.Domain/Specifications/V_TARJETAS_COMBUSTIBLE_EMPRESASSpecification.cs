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
    public partial class V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification : ISpecification<V_TARJETAS_COMBUSTIBLE_EMPRESAS>
    {
    
        public Nullable<int> ID_TARJETA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> ID_TARJETAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> ID_TARJETAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> ID_TARJETATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public string COD_TARJETA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> COD_TARJETAIN
    	{
    		get;
    		set;
    	}
    
    	public string COD_TARJETAContains
    	{
    		get;
    		set;
    	}
    	
    	public string COD_TARJETAStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string COD_TARJETAEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<int> ID_EMPRESA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> ID_EMPRESAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> ID_EMPRESAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> ID_EMPRESATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<int> ID_EMPRESA_EMISORA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> ID_EMPRESA_EMISORAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> ID_EMPRESA_EMISORAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> ID_EMPRESA_EMISORATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public string NOMBRE_EMPRESA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> NOMBRE_EMPRESAIN
    	{
    		get;
    		set;
    	}
    
    	public string NOMBRE_EMPRESAContains
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_EMPRESAStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_EMPRESAEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<System.DateTime> FECHA_CADUCIDAD
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> FECHA_CADUCIDADIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> FECHA_CADUCIDADFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> FECHA_CADUCIDADTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> FECHA_CADUCIDADFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> FECHA_CADUCIDADToOrNull
        {
            get;
            set;
        }
    		
    
        public string PIN
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> PININ
    	{
    		get;
    		set;
    	}
    
    	public string PINContains
    	{
    		get;
    		set;
    	}
    	
    	public string PINStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string PINEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<bool> BAJA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<bool>> BAJAIN
    	{
    		get;
    		set;
    	}
    
        public string NOMBRE_EMPRESA_EMISORA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> NOMBRE_EMPRESA_EMISORAIN
    	{
    		get;
    		set;
    	}
    
    	public string NOMBRE_EMPRESA_EMISORAContains
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_EMPRESA_EMISORAStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_EMPRESA_EMISORAEndsWith
    	{
    		get;
    		set;
    	}
    
    
    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification"/> class.
    	/// </summary>
    	public V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public V_TARJETAS_COMBUSTIBLE_EMPRESASSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<V_TARJETAS_COMBUSTIBLE_EMPRESAS, bool>> GetExpression()
    	{
    		Expression<Func<V_TARJETAS_COMBUSTIBLE_EMPRESAS, bool>> expression = x => true;
    
    		if(ID_TARJETA.HasValue)
    			expression = expression.And(x => x.ID_TARJETA == ID_TARJETA.Value);
    
    		if(ID_TARJETAIN != null && ID_TARJETAIN.Count() > 0)
    			expression = expression.And(x => ID_TARJETAIN.Contains(x.ID_TARJETA));
    	
    		if(ID_TARJETAFrom.HasValue)
    			expression = expression.And(x => x.ID_TARJETA >= ID_TARJETAFrom.Value);
    			
    		if(ID_TARJETATo.HasValue)
    			expression = expression.And(x => x.ID_TARJETA <= ID_TARJETATo.Value);
    				
    
    	
    		if(!string.IsNullOrWhiteSpace(COD_TARJETA))  
    			expression = expression.And(x => x.COD_TARJETA.Equals(COD_TARJETA));
    			
    		if(!string.IsNullOrWhiteSpace(COD_TARJETAContains))  
    			expression = expression.And(x => x.COD_TARJETA.Contains(COD_TARJETAContains));
    			
    		if(!string.IsNullOrWhiteSpace(COD_TARJETAStartsWith))
    			expression = expression.And(x => x.COD_TARJETA.StartsWith(COD_TARJETAStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(COD_TARJETAEndsWith))
    			expression = expression.And(x => x.COD_TARJETA.EndsWith(COD_TARJETAEndsWith));
    
    		if(COD_TARJETAIN != null && COD_TARJETAIN.Count() > 0)
    			expression = expression.And(x => COD_TARJETAIN.Contains(x.COD_TARJETA));
    
    		if(ID_EMPRESA.HasValue)
    			expression = expression.And(x => x.ID_EMPRESA == ID_EMPRESA.Value);
    
    		if(ID_EMPRESAIN != null && ID_EMPRESAIN.Count() > 0)
    			expression = expression.And(x => ID_EMPRESAIN.Contains(x.ID_EMPRESA));
    	
    		if(ID_EMPRESAFrom.HasValue)
    			expression = expression.And(x => x.ID_EMPRESA >= ID_EMPRESAFrom.Value);
    			
    		if(ID_EMPRESATo.HasValue)
    			expression = expression.And(x => x.ID_EMPRESA <= ID_EMPRESATo.Value);
    				
    
    
    		if(ID_EMPRESA_EMISORA.HasValue)
    			expression = expression.And(x => x.ID_EMPRESA_EMISORA == ID_EMPRESA_EMISORA.Value);
    
    		if(ID_EMPRESA_EMISORAIN != null && ID_EMPRESA_EMISORAIN.Count() > 0)
    			expression = expression.And(x => ID_EMPRESA_EMISORAIN.Contains(x.ID_EMPRESA_EMISORA));
    	
    		if(ID_EMPRESA_EMISORAFrom.HasValue)
    			expression = expression.And(x => x.ID_EMPRESA_EMISORA >= ID_EMPRESA_EMISORAFrom.Value);
    			
    		if(ID_EMPRESA_EMISORATo.HasValue)
    			expression = expression.And(x => x.ID_EMPRESA_EMISORA <= ID_EMPRESA_EMISORATo.Value);
    				
    
    	
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESA))  
    			expression = expression.And(x => x.NOMBRE_EMPRESA.Equals(NOMBRE_EMPRESA));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESAContains))  
    			expression = expression.And(x => x.NOMBRE_EMPRESA.Contains(NOMBRE_EMPRESAContains));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESAStartsWith))
    			expression = expression.And(x => x.NOMBRE_EMPRESA.StartsWith(NOMBRE_EMPRESAStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESAEndsWith))
    			expression = expression.And(x => x.NOMBRE_EMPRESA.EndsWith(NOMBRE_EMPRESAEndsWith));
    
    		if(NOMBRE_EMPRESAIN != null && NOMBRE_EMPRESAIN.Count() > 0)
    			expression = expression.And(x => NOMBRE_EMPRESAIN.Contains(x.NOMBRE_EMPRESA));
    
    		if(FECHA_CADUCIDAD.HasValue)
    			expression = expression.And(x => x.FECHA_CADUCIDAD == FECHA_CADUCIDAD.Value);
    
    		if(FECHA_CADUCIDADIN != null && FECHA_CADUCIDADIN.Count() > 0)
    			expression = expression.And(x => FECHA_CADUCIDADIN.Contains(x.FECHA_CADUCIDAD));
    	
    		if(FECHA_CADUCIDADFrom.HasValue)
    			expression = expression.And(x => x.FECHA_CADUCIDAD >= FECHA_CADUCIDADFrom.Value);
    			
    		if(FECHA_CADUCIDADTo.HasValue)
    			expression = expression.And(x => x.FECHA_CADUCIDAD <= FECHA_CADUCIDADTo.Value);
    				
    
    		if(FECHA_CADUCIDADFromOrNull.HasValue)
                expression = expression.And(x => x.FECHA_CADUCIDAD >= FECHA_CADUCIDADFromOrNull.Value || x.FECHA_CADUCIDAD == null);
    
            if(FECHA_CADUCIDADToOrNull.HasValue)
                expression = expression.And(x => x.FECHA_CADUCIDAD <= FECHA_CADUCIDADToOrNull.Value || x.FECHA_CADUCIDAD == null);
    	
    		if(!string.IsNullOrWhiteSpace(PIN))  
    			expression = expression.And(x => x.PIN.Equals(PIN));
    			
    		if(!string.IsNullOrWhiteSpace(PINContains))  
    			expression = expression.And(x => x.PIN.Contains(PINContains));
    			
    		if(!string.IsNullOrWhiteSpace(PINStartsWith))
    			expression = expression.And(x => x.PIN.StartsWith(PINStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(PINEndsWith))
    			expression = expression.And(x => x.PIN.EndsWith(PINEndsWith));
    
    		if(PININ != null && PININ.Count() > 0)
    			expression = expression.And(x => PININ.Contains(x.PIN));
    
    		if(BAJA.HasValue)
    			expression = expression.And(x => x.BAJA == BAJA.Value);
    
    		if(BAJAIN != null && BAJAIN.Count() > 0)
    			expression = expression.And(x => BAJAIN.Contains(x.BAJA));
    	
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESA_EMISORA))  
    			expression = expression.And(x => x.NOMBRE_EMPRESA_EMISORA.Equals(NOMBRE_EMPRESA_EMISORA));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESA_EMISORAContains))  
    			expression = expression.And(x => x.NOMBRE_EMPRESA_EMISORA.Contains(NOMBRE_EMPRESA_EMISORAContains));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESA_EMISORAStartsWith))
    			expression = expression.And(x => x.NOMBRE_EMPRESA_EMISORA.StartsWith(NOMBRE_EMPRESA_EMISORAStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(NOMBRE_EMPRESA_EMISORAEndsWith))
    			expression = expression.And(x => x.NOMBRE_EMPRESA_EMISORA.EndsWith(NOMBRE_EMPRESA_EMISORAEndsWith));
    
    		if(NOMBRE_EMPRESA_EMISORAIN != null && NOMBRE_EMPRESA_EMISORAIN.Count() > 0)
    			expression = expression.And(x => NOMBRE_EMPRESA_EMISORAIN.Contains(x.NOMBRE_EMPRESA_EMISORA));
    		
    		//
    		// Navigation properties
    		//
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(V_TARJETAS_COMBUSTIBLE_EMPRESAS entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<V_TARJETAS_COMBUSTIBLE_EMPRESAS> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
