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
    public partial class T_G_VIA_VERDE_EXTRACTOSSpecification : ISpecification<T_G_VIA_VERDE_EXTRACTOS>
    {
    
        public string ID_EXTRACTO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> ID_EXTRACTOIN
    	{
    		get;
    		set;
    	}
    
    	public string ID_EXTRACTOContains
    	{
    		get;
    		set;
    	}
    	
    	public string ID_EXTRACTOStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string ID_EXTRACTOEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<int> MES_EMISION
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> MES_EMISIONIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> MES_EMISIONFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> MES_EMISIONTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<int> AÑO_EMISION
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> AÑO_EMISIONIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> AÑO_EMISIONFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> AÑO_EMISIONTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public string ID_CLIENTE
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> ID_CLIENTEIN
    	{
    		get;
    		set;
    	}
    
    	public string ID_CLIENTEContains
    	{
    		get;
    		set;
    	}
    	
    	public string ID_CLIENTEStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string ID_CLIENTEEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<decimal> TOTAL
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<decimal>> TOTALIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<decimal> TOTALFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<decimal> TOTALTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<decimal> TOTAL_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<decimal>> TOTAL_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<decimal> TOTAL_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<decimal> TOTAL_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public string NOMBRE_ARCHIVO_IMPORTACION
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> NOMBRE_ARCHIVO_IMPORTACIONIN
    	{
    		get;
    		set;
    	}
    
    	public string NOMBRE_ARCHIVO_IMPORTACIONContains
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_ARCHIVO_IMPORTACIONStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBRE_ARCHIVO_IMPORTACIONEndsWith
    	{
    		get;
    		set;
    	}
    
    
        #region Navigation Properties
    
    	public T_M_CLIENTESSpecification T_M_CLIENTES
        {
            get;
    		set;
    	}

        #endregion

    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="T_G_VIA_VERDE_EXTRACTOSSpecification"/> class.
    	/// </summary>
    	public T_G_VIA_VERDE_EXTRACTOSSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="T_G_VIA_VERDE_EXTRACTOSSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public T_G_VIA_VERDE_EXTRACTOSSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    		this.T_M_CLIENTES = new T_M_CLIENTESSpecification();
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<T_G_VIA_VERDE_EXTRACTOS, bool>> GetExpression()
    	{
    		Expression<Func<T_G_VIA_VERDE_EXTRACTOS, bool>> expression = x => true;
    	
    		if(!string.IsNullOrWhiteSpace(ID_EXTRACTO))  
    			expression = expression.And(x => x.ID_EXTRACTO.Equals(ID_EXTRACTO));
    			
    		if(!string.IsNullOrWhiteSpace(ID_EXTRACTOContains))  
    			expression = expression.And(x => x.ID_EXTRACTO.Contains(ID_EXTRACTOContains));
    			
    		if(!string.IsNullOrWhiteSpace(ID_EXTRACTOStartsWith))
    			expression = expression.And(x => x.ID_EXTRACTO.StartsWith(ID_EXTRACTOStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(ID_EXTRACTOEndsWith))
    			expression = expression.And(x => x.ID_EXTRACTO.EndsWith(ID_EXTRACTOEndsWith));
    
    		if(ID_EXTRACTOIN != null && ID_EXTRACTOIN.Count() > 0)
    			expression = expression.And(x => ID_EXTRACTOIN.Contains(x.ID_EXTRACTO));
    
    		if(MES_EMISION.HasValue)
    			expression = expression.And(x => x.MES_EMISION == MES_EMISION.Value);
    
    		if(MES_EMISIONIN != null && MES_EMISIONIN.Count() > 0)
    			expression = expression.And(x => MES_EMISIONIN.Contains(x.MES_EMISION));
    	
    		if(MES_EMISIONFrom.HasValue)
    			expression = expression.And(x => x.MES_EMISION >= MES_EMISIONFrom.Value);
    			
    		if(MES_EMISIONTo.HasValue)
    			expression = expression.And(x => x.MES_EMISION <= MES_EMISIONTo.Value);
    				
    
    
    		if(AÑO_EMISION.HasValue)
    			expression = expression.And(x => x.AÑO_EMISION == AÑO_EMISION.Value);
    
    		if(AÑO_EMISIONIN != null && AÑO_EMISIONIN.Count() > 0)
    			expression = expression.And(x => AÑO_EMISIONIN.Contains(x.AÑO_EMISION));
    	
    		if(AÑO_EMISIONFrom.HasValue)
    			expression = expression.And(x => x.AÑO_EMISION >= AÑO_EMISIONFrom.Value);
    			
    		if(AÑO_EMISIONTo.HasValue)
    			expression = expression.And(x => x.AÑO_EMISION <= AÑO_EMISIONTo.Value);
    				
    
    	
    		if(!string.IsNullOrWhiteSpace(ID_CLIENTE))  
    			expression = expression.And(x => x.ID_CLIENTE.Equals(ID_CLIENTE));
    			
    		if(!string.IsNullOrWhiteSpace(ID_CLIENTEContains))  
    			expression = expression.And(x => x.ID_CLIENTE.Contains(ID_CLIENTEContains));
    			
    		if(!string.IsNullOrWhiteSpace(ID_CLIENTEStartsWith))
    			expression = expression.And(x => x.ID_CLIENTE.StartsWith(ID_CLIENTEStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(ID_CLIENTEEndsWith))
    			expression = expression.And(x => x.ID_CLIENTE.EndsWith(ID_CLIENTEEndsWith));
    
    		if(ID_CLIENTEIN != null && ID_CLIENTEIN.Count() > 0)
    			expression = expression.And(x => ID_CLIENTEIN.Contains(x.ID_CLIENTE));
    
    		if(TOTAL.HasValue)
    			expression = expression.And(x => x.TOTAL == TOTAL.Value);
    
    		if(TOTALIN != null && TOTALIN.Count() > 0)
    			expression = expression.And(x => TOTALIN.Contains(x.TOTAL));
    	
    		if(TOTALFrom.HasValue)
    			expression = expression.And(x => x.TOTAL >= TOTALFrom.Value);
    			
    		if(TOTALTo.HasValue)
    			expression = expression.And(x => x.TOTAL <= TOTALTo.Value);
    				
    
    
    		if(TOTAL_IVA.HasValue)
    			expression = expression.And(x => x.TOTAL_IVA == TOTAL_IVA.Value);
    
    		if(TOTAL_IVAIN != null && TOTAL_IVAIN.Count() > 0)
    			expression = expression.And(x => TOTAL_IVAIN.Contains(x.TOTAL_IVA));
    	
    		if(TOTAL_IVAFrom.HasValue)
    			expression = expression.And(x => x.TOTAL_IVA >= TOTAL_IVAFrom.Value);
    			
    		if(TOTAL_IVATo.HasValue)
    			expression = expression.And(x => x.TOTAL_IVA <= TOTAL_IVATo.Value);
    				
    
    	
    		if(!string.IsNullOrWhiteSpace(NOMBRE_ARCHIVO_IMPORTACION))  
    			expression = expression.And(x => x.NOMBRE_ARCHIVO_IMPORTACION.Equals(NOMBRE_ARCHIVO_IMPORTACION));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_ARCHIVO_IMPORTACIONContains))  
    			expression = expression.And(x => x.NOMBRE_ARCHIVO_IMPORTACION.Contains(NOMBRE_ARCHIVO_IMPORTACIONContains));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBRE_ARCHIVO_IMPORTACIONStartsWith))
    			expression = expression.And(x => x.NOMBRE_ARCHIVO_IMPORTACION.StartsWith(NOMBRE_ARCHIVO_IMPORTACIONStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(NOMBRE_ARCHIVO_IMPORTACIONEndsWith))
    			expression = expression.And(x => x.NOMBRE_ARCHIVO_IMPORTACION.EndsWith(NOMBRE_ARCHIVO_IMPORTACIONEndsWith));
    
    		if(NOMBRE_ARCHIVO_IMPORTACIONIN != null && NOMBRE_ARCHIVO_IMPORTACIONIN.Count() > 0)
    			expression = expression.And(x => NOMBRE_ARCHIVO_IMPORTACIONIN.Contains(x.NOMBRE_ARCHIVO_IMPORTACION));
    		
    		//
    		// Navigation properties
    		//
    
    		if(this.T_M_CLIENTES != null)
    		{
    			var subExpression = T_M_CLIENTES.GetExpression();
    			expression = expression.And(x => (new[] { x.T_M_CLIENTES }).AsQueryable().Any(subExpression));
    		}
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(T_G_VIA_VERDE_EXTRACTOS entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<T_G_VIA_VERDE_EXTRACTOS> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
