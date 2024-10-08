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
    public partial class T_G_ALERTAS_CAMBIO_CONDUCTORSpecification : ISpecification<T_G_ALERTAS_CAMBIO_CONDUCTOR>
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
    
        public Nullable<int> NUM_EMPLEADO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> NUM_EMPLEADOIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> NUM_EMPLEADOFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> NUM_EMPLEADOTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public string DNI
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> DNIIN
    	{
    		get;
    		set;
    	}
    
    	public string DNIContains
    	{
    		get;
    		set;
    	}
    	
    	public string DNIStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string DNIEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string NOMBRE
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> NOMBREIN
    	{
    		get;
    		set;
    	}
    
    	public string NOMBREContains
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBREStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string NOMBREEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string DOMICILIO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> DOMICILIOIN
    	{
    		get;
    		set;
    	}
    
    	public string DOMICILIOContains
    	{
    		get;
    		set;
    	}
    	
    	public string DOMICILIOStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string DOMICILIOEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string CP
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> CPIN
    	{
    		get;
    		set;
    	}
    
    	public string CPContains
    	{
    		get;
    		set;
    	}
    	
    	public string CPStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string CPEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string PROVINCIA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> PROVINCIAIN
    	{
    		get;
    		set;
    	}
    
    	public string PROVINCIAContains
    	{
    		get;
    		set;
    	}
    	
    	public string PROVINCIAStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string PROVINCIAEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string POBLACION
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> POBLACIONIN
    	{
    		get;
    		set;
    	}
    
    	public string POBLACIONContains
    	{
    		get;
    		set;
    	}
    	
    	public string POBLACIONStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string POBLACIONEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<System.DateTime> FECHA_NACIMIENTO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> FECHA_NACIMIENTOIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> FECHA_NACIMIENTOFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> FECHA_NACIMIENTOTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> FECHA_NACIMIENTOFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> FECHA_NACIMIENTOToOrNull
        {
            get;
            set;
        }
    		
    
        public Nullable<System.DateTime> FECHA_VENCIMIENTO_CARNET
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> FECHA_VENCIMIENTO_CARNETIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> FECHA_VENCIMIENTO_CARNETFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> FECHA_VENCIMIENTO_CARNETTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> FECHA_VENCIMIENTO_CARNETFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> FECHA_VENCIMIENTO_CARNETToOrNull
        {
            get;
            set;
        }
    		
    
        public string MOTIVO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> MOTIVOIN
    	{
    		get;
    		set;
    	}
    
    	public string MOTIVOContains
    	{
    		get;
    		set;
    	}
    	
    	public string MOTIVOStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string MOTIVOEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string FICHERO_RENTING
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> FICHERO_RENTINGIN
    	{
    		get;
    		set;
    	}
    
    	public string FICHERO_RENTINGContains
    	{
    		get;
    		set;
    	}
    	
    	public string FICHERO_RENTINGStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string FICHERO_RENTINGEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string OBSERVACIONES
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> OBSERVACIONESIN
    	{
    		get;
    		set;
    	}
    
    	public string OBSERVACIONESContains
    	{
    		get;
    		set;
    	}
    	
    	public string OBSERVACIONESStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string OBSERVACIONESEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<System.DateTime> FECHA_EFECTO
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> FECHA_EFECTOIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> FECHA_EFECTOFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> FECHA_EFECTOTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> FECHA_EFECTOFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> FECHA_EFECTOToOrNull
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
    	/// Initializes a new instance of the <see cref="T_G_ALERTAS_CAMBIO_CONDUCTORSpecification"/> class.
    	/// </summary>
    	public T_G_ALERTAS_CAMBIO_CONDUCTORSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="T_G_ALERTAS_CAMBIO_CONDUCTORSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public T_G_ALERTAS_CAMBIO_CONDUCTORSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    		this.T_G_ALERTAS = new T_G_ALERTASSpecification();
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<T_G_ALERTAS_CAMBIO_CONDUCTOR, bool>> GetExpression()
    	{
    		Expression<Func<T_G_ALERTAS_CAMBIO_CONDUCTOR, bool>> expression = x => true;
    
    		if(ID_ALERTA.HasValue)
    			expression = expression.And(x => x.ID_ALERTA == ID_ALERTA.Value);
    
    		if(ID_ALERTAIN != null && ID_ALERTAIN.Count() > 0)
    			expression = expression.And(x => ID_ALERTAIN.Contains(x.ID_ALERTA));
    
    		if(NUM_EMPLEADO.HasValue)
    			expression = expression.And(x => x.NUM_EMPLEADO == NUM_EMPLEADO.Value);
    
    		if(NUM_EMPLEADOIN != null && NUM_EMPLEADOIN.Count() > 0)
    			expression = expression.And(x => NUM_EMPLEADOIN.Contains(x.NUM_EMPLEADO));
    	
    		if(NUM_EMPLEADOFrom.HasValue)
    			expression = expression.And(x => x.NUM_EMPLEADO >= NUM_EMPLEADOFrom.Value);
    			
    		if(NUM_EMPLEADOTo.HasValue)
    			expression = expression.And(x => x.NUM_EMPLEADO <= NUM_EMPLEADOTo.Value);
    				
    
    	
    		if(!string.IsNullOrWhiteSpace(DNI))  
    			expression = expression.And(x => x.DNI.Equals(DNI));
    			
    		if(!string.IsNullOrWhiteSpace(DNIContains))  
    			expression = expression.And(x => x.DNI.Contains(DNIContains));
    			
    		if(!string.IsNullOrWhiteSpace(DNIStartsWith))
    			expression = expression.And(x => x.DNI.StartsWith(DNIStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(DNIEndsWith))
    			expression = expression.And(x => x.DNI.EndsWith(DNIEndsWith));
    
    		if(DNIIN != null && DNIIN.Count() > 0)
    			expression = expression.And(x => DNIIN.Contains(x.DNI));
    	
    		if(!string.IsNullOrWhiteSpace(NOMBRE))  
    			expression = expression.And(x => x.NOMBRE.Equals(NOMBRE));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBREContains))  
    			expression = expression.And(x => x.NOMBRE.Contains(NOMBREContains));
    			
    		if(!string.IsNullOrWhiteSpace(NOMBREStartsWith))
    			expression = expression.And(x => x.NOMBRE.StartsWith(NOMBREStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(NOMBREEndsWith))
    			expression = expression.And(x => x.NOMBRE.EndsWith(NOMBREEndsWith));
    
    		if(NOMBREIN != null && NOMBREIN.Count() > 0)
    			expression = expression.And(x => NOMBREIN.Contains(x.NOMBRE));
    	
    		if(!string.IsNullOrWhiteSpace(DOMICILIO))  
    			expression = expression.And(x => x.DOMICILIO.Equals(DOMICILIO));
    			
    		if(!string.IsNullOrWhiteSpace(DOMICILIOContains))  
    			expression = expression.And(x => x.DOMICILIO.Contains(DOMICILIOContains));
    			
    		if(!string.IsNullOrWhiteSpace(DOMICILIOStartsWith))
    			expression = expression.And(x => x.DOMICILIO.StartsWith(DOMICILIOStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(DOMICILIOEndsWith))
    			expression = expression.And(x => x.DOMICILIO.EndsWith(DOMICILIOEndsWith));
    
    		if(DOMICILIOIN != null && DOMICILIOIN.Count() > 0)
    			expression = expression.And(x => DOMICILIOIN.Contains(x.DOMICILIO));
    	
    		if(!string.IsNullOrWhiteSpace(CP))  
    			expression = expression.And(x => x.CP.Equals(CP));
    			
    		if(!string.IsNullOrWhiteSpace(CPContains))  
    			expression = expression.And(x => x.CP.Contains(CPContains));
    			
    		if(!string.IsNullOrWhiteSpace(CPStartsWith))
    			expression = expression.And(x => x.CP.StartsWith(CPStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(CPEndsWith))
    			expression = expression.And(x => x.CP.EndsWith(CPEndsWith));
    
    		if(CPIN != null && CPIN.Count() > 0)
    			expression = expression.And(x => CPIN.Contains(x.CP));
    	
    		if(!string.IsNullOrWhiteSpace(PROVINCIA))  
    			expression = expression.And(x => x.PROVINCIA.Equals(PROVINCIA));
    			
    		if(!string.IsNullOrWhiteSpace(PROVINCIAContains))  
    			expression = expression.And(x => x.PROVINCIA.Contains(PROVINCIAContains));
    			
    		if(!string.IsNullOrWhiteSpace(PROVINCIAStartsWith))
    			expression = expression.And(x => x.PROVINCIA.StartsWith(PROVINCIAStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(PROVINCIAEndsWith))
    			expression = expression.And(x => x.PROVINCIA.EndsWith(PROVINCIAEndsWith));
    
    		if(PROVINCIAIN != null && PROVINCIAIN.Count() > 0)
    			expression = expression.And(x => PROVINCIAIN.Contains(x.PROVINCIA));
    	
    		if(!string.IsNullOrWhiteSpace(POBLACION))  
    			expression = expression.And(x => x.POBLACION.Equals(POBLACION));
    			
    		if(!string.IsNullOrWhiteSpace(POBLACIONContains))  
    			expression = expression.And(x => x.POBLACION.Contains(POBLACIONContains));
    			
    		if(!string.IsNullOrWhiteSpace(POBLACIONStartsWith))
    			expression = expression.And(x => x.POBLACION.StartsWith(POBLACIONStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(POBLACIONEndsWith))
    			expression = expression.And(x => x.POBLACION.EndsWith(POBLACIONEndsWith));
    
    		if(POBLACIONIN != null && POBLACIONIN.Count() > 0)
    			expression = expression.And(x => POBLACIONIN.Contains(x.POBLACION));
    
    		if(FECHA_NACIMIENTO.HasValue)
    			expression = expression.And(x => x.FECHA_NACIMIENTO == FECHA_NACIMIENTO.Value);
    
    		if(FECHA_NACIMIENTOIN != null && FECHA_NACIMIENTOIN.Count() > 0)
    			expression = expression.And(x => FECHA_NACIMIENTOIN.Contains(x.FECHA_NACIMIENTO));
    	
    		if(FECHA_NACIMIENTOFrom.HasValue)
    			expression = expression.And(x => x.FECHA_NACIMIENTO >= FECHA_NACIMIENTOFrom.Value);
    			
    		if(FECHA_NACIMIENTOTo.HasValue)
    			expression = expression.And(x => x.FECHA_NACIMIENTO <= FECHA_NACIMIENTOTo.Value);
    				
    
    		if(FECHA_NACIMIENTOFromOrNull.HasValue)
                expression = expression.And(x => x.FECHA_NACIMIENTO >= FECHA_NACIMIENTOFromOrNull.Value || x.FECHA_NACIMIENTO == null);
    
            if(FECHA_NACIMIENTOToOrNull.HasValue)
                expression = expression.And(x => x.FECHA_NACIMIENTO <= FECHA_NACIMIENTOToOrNull.Value || x.FECHA_NACIMIENTO == null);
    
    		if(FECHA_VENCIMIENTO_CARNET.HasValue)
    			expression = expression.And(x => x.FECHA_VENCIMIENTO_CARNET == FECHA_VENCIMIENTO_CARNET.Value);
    
    		if(FECHA_VENCIMIENTO_CARNETIN != null && FECHA_VENCIMIENTO_CARNETIN.Count() > 0)
    			expression = expression.And(x => FECHA_VENCIMIENTO_CARNETIN.Contains(x.FECHA_VENCIMIENTO_CARNET));
    	
    		if(FECHA_VENCIMIENTO_CARNETFrom.HasValue)
    			expression = expression.And(x => x.FECHA_VENCIMIENTO_CARNET >= FECHA_VENCIMIENTO_CARNETFrom.Value);
    			
    		if(FECHA_VENCIMIENTO_CARNETTo.HasValue)
    			expression = expression.And(x => x.FECHA_VENCIMIENTO_CARNET <= FECHA_VENCIMIENTO_CARNETTo.Value);
    				
    
    		if(FECHA_VENCIMIENTO_CARNETFromOrNull.HasValue)
                expression = expression.And(x => x.FECHA_VENCIMIENTO_CARNET >= FECHA_VENCIMIENTO_CARNETFromOrNull.Value || x.FECHA_VENCIMIENTO_CARNET == null);
    
            if(FECHA_VENCIMIENTO_CARNETToOrNull.HasValue)
                expression = expression.And(x => x.FECHA_VENCIMIENTO_CARNET <= FECHA_VENCIMIENTO_CARNETToOrNull.Value || x.FECHA_VENCIMIENTO_CARNET == null);
    	
    		if(!string.IsNullOrWhiteSpace(MOTIVO))  
    			expression = expression.And(x => x.MOTIVO.Equals(MOTIVO));
    			
    		if(!string.IsNullOrWhiteSpace(MOTIVOContains))  
    			expression = expression.And(x => x.MOTIVO.Contains(MOTIVOContains));
    			
    		if(!string.IsNullOrWhiteSpace(MOTIVOStartsWith))
    			expression = expression.And(x => x.MOTIVO.StartsWith(MOTIVOStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(MOTIVOEndsWith))
    			expression = expression.And(x => x.MOTIVO.EndsWith(MOTIVOEndsWith));
    
    		if(MOTIVOIN != null && MOTIVOIN.Count() > 0)
    			expression = expression.And(x => MOTIVOIN.Contains(x.MOTIVO));
    	
    		if(!string.IsNullOrWhiteSpace(FICHERO_RENTING))  
    			expression = expression.And(x => x.FICHERO_RENTING.Equals(FICHERO_RENTING));
    			
    		if(!string.IsNullOrWhiteSpace(FICHERO_RENTINGContains))  
    			expression = expression.And(x => x.FICHERO_RENTING.Contains(FICHERO_RENTINGContains));
    			
    		if(!string.IsNullOrWhiteSpace(FICHERO_RENTINGStartsWith))
    			expression = expression.And(x => x.FICHERO_RENTING.StartsWith(FICHERO_RENTINGStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(FICHERO_RENTINGEndsWith))
    			expression = expression.And(x => x.FICHERO_RENTING.EndsWith(FICHERO_RENTINGEndsWith));
    
    		if(FICHERO_RENTINGIN != null && FICHERO_RENTINGIN.Count() > 0)
    			expression = expression.And(x => FICHERO_RENTINGIN.Contains(x.FICHERO_RENTING));
    	
    		if(!string.IsNullOrWhiteSpace(OBSERVACIONES))  
    			expression = expression.And(x => x.OBSERVACIONES.Equals(OBSERVACIONES));
    			
    		if(!string.IsNullOrWhiteSpace(OBSERVACIONESContains))  
    			expression = expression.And(x => x.OBSERVACIONES.Contains(OBSERVACIONESContains));
    			
    		if(!string.IsNullOrWhiteSpace(OBSERVACIONESStartsWith))
    			expression = expression.And(x => x.OBSERVACIONES.StartsWith(OBSERVACIONESStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(OBSERVACIONESEndsWith))
    			expression = expression.And(x => x.OBSERVACIONES.EndsWith(OBSERVACIONESEndsWith));
    
    		if(OBSERVACIONESIN != null && OBSERVACIONESIN.Count() > 0)
    			expression = expression.And(x => OBSERVACIONESIN.Contains(x.OBSERVACIONES));
    
    		if(FECHA_EFECTO.HasValue)
    			expression = expression.And(x => x.FECHA_EFECTO == FECHA_EFECTO.Value);
    
    		if(FECHA_EFECTOIN != null && FECHA_EFECTOIN.Count() > 0)
    			expression = expression.And(x => FECHA_EFECTOIN.Contains(x.FECHA_EFECTO));
    	
    		if(FECHA_EFECTOFrom.HasValue)
    			expression = expression.And(x => x.FECHA_EFECTO >= FECHA_EFECTOFrom.Value);
    			
    		if(FECHA_EFECTOTo.HasValue)
    			expression = expression.And(x => x.FECHA_EFECTO <= FECHA_EFECTOTo.Value);
    				
    
    		if(FECHA_EFECTOFromOrNull.HasValue)
                expression = expression.And(x => x.FECHA_EFECTO >= FECHA_EFECTOFromOrNull.Value || x.FECHA_EFECTO == null);
    
            if(FECHA_EFECTOToOrNull.HasValue)
                expression = expression.And(x => x.FECHA_EFECTO <= FECHA_EFECTOToOrNull.Value || x.FECHA_EFECTO == null);
    		
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
    	
    	public bool IsSatisfiedBy(T_G_ALERTAS_CAMBIO_CONDUCTOR entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<T_G_ALERTAS_CAMBIO_CONDUCTOR> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
