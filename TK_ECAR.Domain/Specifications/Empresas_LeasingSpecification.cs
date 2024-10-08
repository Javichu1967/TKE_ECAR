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
    public partial class Empresas_LeasingSpecification : ISpecification<Empresas_Leasing>
    {
    
        public Nullable<int> Cod_Empresa
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> Cod_EmpresaIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> Cod_EmpresaFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> Cod_EmpresaTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public string Nombre
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> NombreIN
    	{
    		get;
    		set;
    	}
    
    	public string NombreContains
    	{
    		get;
    		set;
    	}
    	
    	public string NombreStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string NombreEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Direccion
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> DireccionIN
    	{
    		get;
    		set;
    	}
    
    	public string DireccionContains
    	{
    		get;
    		set;
    	}
    	
    	public string DireccionStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string DireccionEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Cod_Postal
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> Cod_PostalIN
    	{
    		get;
    		set;
    	}
    
    	public string Cod_PostalContains
    	{
    		get;
    		set;
    	}
    	
    	public string Cod_PostalStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string Cod_PostalEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Poblacion
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> PoblacionIN
    	{
    		get;
    		set;
    	}
    
    	public string PoblacionContains
    	{
    		get;
    		set;
    	}
    	
    	public string PoblacionStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string PoblacionEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Domiciliacion
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> DomiciliacionIN
    	{
    		get;
    		set;
    	}
    
    	public string DomiciliacionContains
    	{
    		get;
    		set;
    	}
    	
    	public string DomiciliacionStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string DomiciliacionEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Persona_Contacto
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> Persona_ContactoIN
    	{
    		get;
    		set;
    	}
    
    	public string Persona_ContactoContains
    	{
    		get;
    		set;
    	}
    	
    	public string Persona_ContactoStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string Persona_ContactoEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Tlf_Contacto
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> Tlf_ContactoIN
    	{
    		get;
    		set;
    	}
    
    	public string Tlf_ContactoContains
    	{
    		get;
    		set;
    	}
    	
    	public string Tlf_ContactoStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string Tlf_ContactoEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<bool> POA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<bool>> POAIN
    	{
    		get;
    		set;
    	}
    
    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="Empresas_LeasingSpecification"/> class.
    	/// </summary>
    	public Empresas_LeasingSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="Empresas_LeasingSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public Empresas_LeasingSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<Empresas_Leasing, bool>> GetExpression()
    	{
    		Expression<Func<Empresas_Leasing, bool>> expression = x => true;
    
    		if(Cod_Empresa.HasValue)
    			expression = expression.And(x => x.Cod_Empresa == Cod_Empresa.Value);
    
    		if(Cod_EmpresaIN != null && Cod_EmpresaIN.Count() > 0)
    			expression = expression.And(x => Cod_EmpresaIN.Contains(x.Cod_Empresa));
    	
    		if(Cod_EmpresaFrom.HasValue)
    			expression = expression.And(x => x.Cod_Empresa >= Cod_EmpresaFrom.Value);
    			
    		if(Cod_EmpresaTo.HasValue)
    			expression = expression.And(x => x.Cod_Empresa <= Cod_EmpresaTo.Value);
    				
    
    	
    		if(!string.IsNullOrWhiteSpace(Nombre))  
    			expression = expression.And(x => x.Nombre.Equals(Nombre));
    			
    		if(!string.IsNullOrWhiteSpace(NombreContains))  
    			expression = expression.And(x => x.Nombre.Contains(NombreContains));
    			
    		if(!string.IsNullOrWhiteSpace(NombreStartsWith))
    			expression = expression.And(x => x.Nombre.StartsWith(NombreStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(NombreEndsWith))
    			expression = expression.And(x => x.Nombre.EndsWith(NombreEndsWith));
    
    		if(NombreIN != null && NombreIN.Count() > 0)
    			expression = expression.And(x => NombreIN.Contains(x.Nombre));
    	
    		if(!string.IsNullOrWhiteSpace(Direccion))  
    			expression = expression.And(x => x.Direccion.Equals(Direccion));
    			
    		if(!string.IsNullOrWhiteSpace(DireccionContains))  
    			expression = expression.And(x => x.Direccion.Contains(DireccionContains));
    			
    		if(!string.IsNullOrWhiteSpace(DireccionStartsWith))
    			expression = expression.And(x => x.Direccion.StartsWith(DireccionStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(DireccionEndsWith))
    			expression = expression.And(x => x.Direccion.EndsWith(DireccionEndsWith));
    
    		if(DireccionIN != null && DireccionIN.Count() > 0)
    			expression = expression.And(x => DireccionIN.Contains(x.Direccion));
    	
    		if(!string.IsNullOrWhiteSpace(Cod_Postal))  
    			expression = expression.And(x => x.Cod_Postal.Equals(Cod_Postal));
    			
    		if(!string.IsNullOrWhiteSpace(Cod_PostalContains))  
    			expression = expression.And(x => x.Cod_Postal.Contains(Cod_PostalContains));
    			
    		if(!string.IsNullOrWhiteSpace(Cod_PostalStartsWith))
    			expression = expression.And(x => x.Cod_Postal.StartsWith(Cod_PostalStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(Cod_PostalEndsWith))
    			expression = expression.And(x => x.Cod_Postal.EndsWith(Cod_PostalEndsWith));
    
    		if(Cod_PostalIN != null && Cod_PostalIN.Count() > 0)
    			expression = expression.And(x => Cod_PostalIN.Contains(x.Cod_Postal));
    	
    		if(!string.IsNullOrWhiteSpace(Poblacion))  
    			expression = expression.And(x => x.Poblacion.Equals(Poblacion));
    			
    		if(!string.IsNullOrWhiteSpace(PoblacionContains))  
    			expression = expression.And(x => x.Poblacion.Contains(PoblacionContains));
    			
    		if(!string.IsNullOrWhiteSpace(PoblacionStartsWith))
    			expression = expression.And(x => x.Poblacion.StartsWith(PoblacionStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(PoblacionEndsWith))
    			expression = expression.And(x => x.Poblacion.EndsWith(PoblacionEndsWith));
    
    		if(PoblacionIN != null && PoblacionIN.Count() > 0)
    			expression = expression.And(x => PoblacionIN.Contains(x.Poblacion));
    	
    		if(!string.IsNullOrWhiteSpace(Domiciliacion))  
    			expression = expression.And(x => x.Domiciliacion.Equals(Domiciliacion));
    			
    		if(!string.IsNullOrWhiteSpace(DomiciliacionContains))  
    			expression = expression.And(x => x.Domiciliacion.Contains(DomiciliacionContains));
    			
    		if(!string.IsNullOrWhiteSpace(DomiciliacionStartsWith))
    			expression = expression.And(x => x.Domiciliacion.StartsWith(DomiciliacionStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(DomiciliacionEndsWith))
    			expression = expression.And(x => x.Domiciliacion.EndsWith(DomiciliacionEndsWith));
    
    		if(DomiciliacionIN != null && DomiciliacionIN.Count() > 0)
    			expression = expression.And(x => DomiciliacionIN.Contains(x.Domiciliacion));
    	
    		if(!string.IsNullOrWhiteSpace(Persona_Contacto))  
    			expression = expression.And(x => x.Persona_Contacto.Equals(Persona_Contacto));
    			
    		if(!string.IsNullOrWhiteSpace(Persona_ContactoContains))  
    			expression = expression.And(x => x.Persona_Contacto.Contains(Persona_ContactoContains));
    			
    		if(!string.IsNullOrWhiteSpace(Persona_ContactoStartsWith))
    			expression = expression.And(x => x.Persona_Contacto.StartsWith(Persona_ContactoStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(Persona_ContactoEndsWith))
    			expression = expression.And(x => x.Persona_Contacto.EndsWith(Persona_ContactoEndsWith));
    
    		if(Persona_ContactoIN != null && Persona_ContactoIN.Count() > 0)
    			expression = expression.And(x => Persona_ContactoIN.Contains(x.Persona_Contacto));
    	
    		if(!string.IsNullOrWhiteSpace(Tlf_Contacto))  
    			expression = expression.And(x => x.Tlf_Contacto.Equals(Tlf_Contacto));
    			
    		if(!string.IsNullOrWhiteSpace(Tlf_ContactoContains))  
    			expression = expression.And(x => x.Tlf_Contacto.Contains(Tlf_ContactoContains));
    			
    		if(!string.IsNullOrWhiteSpace(Tlf_ContactoStartsWith))
    			expression = expression.And(x => x.Tlf_Contacto.StartsWith(Tlf_ContactoStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(Tlf_ContactoEndsWith))
    			expression = expression.And(x => x.Tlf_Contacto.EndsWith(Tlf_ContactoEndsWith));
    
    		if(Tlf_ContactoIN != null && Tlf_ContactoIN.Count() > 0)
    			expression = expression.And(x => Tlf_ContactoIN.Contains(x.Tlf_Contacto));
    
    		if(POA.HasValue)
    			expression = expression.And(x => x.POA == POA.Value);
    
    		if(POAIN != null && POAIN.Count() > 0)
    			expression = expression.And(x => POAIN.Contains(x.POA));
    		
    		//
    		// Navigation properties
    		//
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(Empresas_Leasing entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<Empresas_Leasing> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
