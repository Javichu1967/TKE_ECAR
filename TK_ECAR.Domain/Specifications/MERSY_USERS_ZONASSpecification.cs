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
    public partial class MERSY_USERS_ZONASSpecification : ISpecification<MERSY_USERS_ZONAS>
    {
    
        public string User_Logon
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> User_LogonIN
    	{
    		get;
    		set;
    	}
    
    	public string User_LogonContains
    	{
    		get;
    		set;
    	}
    	
    	public string User_LogonStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string User_LogonEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string IDZona
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> IDZonaIN
    	{
    		get;
    		set;
    	}
    
    	public string IDZonaContains
    	{
    		get;
    		set;
    	}
    	
    	public string IDZonaStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string IDZonaEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<System.DateTime> FechaHoraAlta
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> FechaHoraAltaIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> FechaHoraAltaFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> FechaHoraAltaTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> FechaHoraAltaFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> FechaHoraAltaToOrNull
        {
            get;
            set;
        }
    		
    
        #region Navigation Properties
    
    	public MERSY_ZONASSpecification MERSY_ZONAS
        {
            get;
    		set;
    	}

        #endregion

    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="MERSY_USERS_ZONASSpecification"/> class.
    	/// </summary>
    	public MERSY_USERS_ZONASSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="MERSY_USERS_ZONASSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public MERSY_USERS_ZONASSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    		this.MERSY_ZONAS = new MERSY_ZONASSpecification();
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<MERSY_USERS_ZONAS, bool>> GetExpression()
    	{
    		Expression<Func<MERSY_USERS_ZONAS, bool>> expression = x => true;
    	
    		if(!string.IsNullOrWhiteSpace(User_Logon))  
    			expression = expression.And(x => x.User_Logon.Equals(User_Logon));
    			
    		if(!string.IsNullOrWhiteSpace(User_LogonContains))  
    			expression = expression.And(x => x.User_Logon.Contains(User_LogonContains));
    			
    		if(!string.IsNullOrWhiteSpace(User_LogonStartsWith))
    			expression = expression.And(x => x.User_Logon.StartsWith(User_LogonStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(User_LogonEndsWith))
    			expression = expression.And(x => x.User_Logon.EndsWith(User_LogonEndsWith));
    
    		if(User_LogonIN != null && User_LogonIN.Count() > 0)
    			expression = expression.And(x => User_LogonIN.Contains(x.User_Logon));
    	
    		if(!string.IsNullOrWhiteSpace(IDZona))  
    			expression = expression.And(x => x.IDZona.Equals(IDZona));
    			
    		if(!string.IsNullOrWhiteSpace(IDZonaContains))  
    			expression = expression.And(x => x.IDZona.Contains(IDZonaContains));
    			
    		if(!string.IsNullOrWhiteSpace(IDZonaStartsWith))
    			expression = expression.And(x => x.IDZona.StartsWith(IDZonaStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(IDZonaEndsWith))
    			expression = expression.And(x => x.IDZona.EndsWith(IDZonaEndsWith));
    
    		if(IDZonaIN != null && IDZonaIN.Count() > 0)
    			expression = expression.And(x => IDZonaIN.Contains(x.IDZona));
    
    		if(FechaHoraAlta.HasValue)
    			expression = expression.And(x => x.FechaHoraAlta == FechaHoraAlta.Value);
    
    		if(FechaHoraAltaIN != null && FechaHoraAltaIN.Count() > 0)
    			expression = expression.And(x => FechaHoraAltaIN.Contains(x.FechaHoraAlta));
    	
    		if(FechaHoraAltaFrom.HasValue)
    			expression = expression.And(x => x.FechaHoraAlta >= FechaHoraAltaFrom.Value);
    			
    		if(FechaHoraAltaTo.HasValue)
    			expression = expression.And(x => x.FechaHoraAlta <= FechaHoraAltaTo.Value);
    				
    
    		if(FechaHoraAltaFromOrNull.HasValue)
                expression = expression.And(x => x.FechaHoraAlta >= FechaHoraAltaFromOrNull.Value || x.FechaHoraAlta == null);
    
            if(FechaHoraAltaToOrNull.HasValue)
                expression = expression.And(x => x.FechaHoraAlta <= FechaHoraAltaToOrNull.Value || x.FechaHoraAlta == null);
    		
    		//
    		// Navigation properties
    		//
    
    		if(this.MERSY_ZONAS != null)
    		{
    			var subExpression = MERSY_ZONAS.GetExpression();
    			expression = expression.And(x => (new[] { x.MERSY_ZONAS }).AsQueryable().Any(subExpression));
    		}
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(MERSY_USERS_ZONAS entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<MERSY_USERS_ZONAS> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
