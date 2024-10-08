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
    public partial class T_G_DATOS_LEASINGSpecification : ISpecification<T_G_DATOS_LEASING>
    {
    
        public Nullable<int> Id
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> IdIN
    	{
    		get;
    		set;
    	}
    
        public Nullable<int> EmpresaLeasing
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> EmpresaLeasingIN
    	{
    		get;
    		set;
    	}
    
        public Nullable<System.DateTime> Fecha_Factura
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> Fecha_FacturaIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> Fecha_FacturaFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> Fecha_FacturaTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> Fecha_FacturaFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> Fecha_FacturaToOrNull
        {
            get;
            set;
        }
    		
    
        public string Num_Factura
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> Num_FacturaIN
    	{
    		get;
    		set;
    	}
    
    	public string Num_FacturaContains
    	{
    		get;
    		set;
    	}
    	
    	public string Num_FacturaStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string Num_FacturaEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Matricula
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> MatriculaIN
    	{
    		get;
    		set;
    	}
    
    	public string MatriculaContains
    	{
    		get;
    		set;
    	}
    	
    	public string MatriculaStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string MatriculaEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Ejercicio
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> EjercicioIN
    	{
    		get;
    		set;
    	}
    
    	public string EjercicioContains
    	{
    		get;
    		set;
    	}
    	
    	public string EjercicioStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string EjercicioEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public string Trimestre
        {
            get; 
            set;
        }
    		
    	public IEnumerable<string> TrimestreIN
    	{
    		get;
    		set;
    	}
    
    	public string TrimestreContains
    	{
    		get;
    		set;
    	}
    	
    	public string TrimestreStartsWith
    	{
    		get;
    		set;
    	}
    	
    	public string TrimestreEndsWith
    	{
    		get;
    		set;
    	}
    
    
        public Nullable<double> Imp_Circulacion
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Imp_CirculacionIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Imp_CirculacionFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Imp_CirculacionTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Imp_Circulacion_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Imp_Circulacion_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Imp_Circulacion_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Imp_Circulacion_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Imp_Matriculacion
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Imp_MatriculacionIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Imp_MatriculacionFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Imp_MatriculacionTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Imp_Matriculacion_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Imp_Matriculacion_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Imp_Matriculacion_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Imp_Matriculacion_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Alquiler
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> AlquilerIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> AlquilerFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> AlquilerTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Alquiler_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Alquiler_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Alquiler_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Alquiler_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Mantenimiento
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> MantenimientoIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> MantenimientoFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> MantenimientoTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Mantenimiento_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Mantenimiento_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Mantenimiento_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Mantenimiento_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Neumaticos
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> NeumaticosIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> NeumaticosFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> NeumaticosTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Neumaticos_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Neumaticos_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Neumaticos_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Neumaticos_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Administracion
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> AdministracionIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> AdministracionFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> AdministracionTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Administracion_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Administracion_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Administracion_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Administracion_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Seguro
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> SeguroIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> SeguroFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> SeguroTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Seguro_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Seguro_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Seguro_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Seguro_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Asistencia_Carretera
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Asistencia_CarreteraIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Asistencia_CarreteraFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Asistencia_CarreteraTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Asistencia_Carretera_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Asistencia_Carretera_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Asistencia_Carretera_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Asistencia_Carretera_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Intereses_Prepagados
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Intereses_PrepagadosIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Intereses_PrepagadosFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Intereses_PrepagadosTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> Intereses_Prepagados_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> Intereses_Prepagados_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> Intereses_Prepagados_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> Intereses_Prepagados_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<bool> Canarias
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<bool>> CanariasIN
    	{
    		get;
    		set;
    	}
    
        public Nullable<bool> Directivo
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<bool>> DirectivoIN
    	{
    		get;
    		set;
    	}
    
        public Nullable<System.DateTime> Fecha_Servicio
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> Fecha_ServicioIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> Fecha_ServicioFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> Fecha_ServicioTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> Fecha_ServicioFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> Fecha_ServicioToOrNull
        {
            get;
            set;
        }
    		
    
        public Nullable<System.DateTime> Fecha_Importacion
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> Fecha_ImportacionIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> Fecha_ImportacionFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> Fecha_ImportacionTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> Fecha_ImportacionFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> Fecha_ImportacionToOrNull
        {
            get;
            set;
        }
    		
    
        public Nullable<double> ITV
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> ITVIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> ITVFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> ITVTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<double> ITV_IVA
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<double>> ITV_IVAIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<double> ITV_IVAFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<double> ITV_IVATo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<decimal> Impuesto
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<decimal>> ImpuestoIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<decimal> ImpuestoFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<decimal> ImpuestoTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<int> Sociedad
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<int>> SociedadIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<int> SociedadFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<int> SociedadTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    
        public Nullable<System.DateTime> FechaAlta
        {
            get; 
            set;
        }
    		
    	public IEnumerable<Nullable<System.DateTime>> FechaAltaIN
    	{
    		get;
    		set;
    	}
    
    	public Nullable<System.DateTime> FechaAltaFrom
    	{
    		get;
    		set;
    	}
    	
    	public Nullable<System.DateTime> FechaAltaTo
    	{
    		get;
    		set;
    	}
    	 
        
    
    	public Nullable<System.DateTime> FechaAltaFromOrNull
        {
            get;
            set;
        }
    
                    
        public Nullable<System.DateTime> FechaAltaToOrNull
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
    
    	public ECAR_Datos_VehiculoSpecification ECAR_Datos_Vehiculo
        {
            get;
    		set;
    	}
    
    	public T_M_EMPRESAS_VEHICULOSSpecification T_M_EMPRESAS_VEHICULOS
        {
            get;
    		set;
    	}

        #endregion

    
    	/// <summary>
    	/// Default constructor (needed for serialization)
    	/// Initializes a new instance of the <see cref="T_G_DATOS_LEASINGSpecification"/> class.
    	/// </summary>
    	public T_G_DATOS_LEASINGSpecification()
    	{
    
    	}
    
    	/// <summary>
    	/// Initializes a new instance of the <see cref="T_G_DATOS_LEASINGSpecification"/> class.
    	/// </summary>
    	/// <param name="initializeNavigationProperties">if set to <c>true</c> initialize navigation properties.</param>
    	public T_G_DATOS_LEASINGSpecification(bool initializeNavigationProperties)
    	{
    		if(!initializeNavigationProperties)
    			return;
    
    		this.ECAR_Datos_Vehiculo = new ECAR_Datos_VehiculoSpecification();
    		this.T_M_EMPRESAS_VEHICULOS = new T_M_EMPRESAS_VEHICULOSSpecification();
    	}
    
        #region ISpecification Members
    
    	public Expression<Func<T_G_DATOS_LEASING, bool>> GetExpression()
    	{
    		Expression<Func<T_G_DATOS_LEASING, bool>> expression = x => true;
    
    		if(Id.HasValue)
    			expression = expression.And(x => x.Id == Id.Value);
    
    		if(IdIN != null && IdIN.Count() > 0)
    			expression = expression.And(x => IdIN.Contains(x.Id));
    
    		if(EmpresaLeasing.HasValue)
    			expression = expression.And(x => x.EmpresaLeasing == EmpresaLeasing.Value);
    
    		if(EmpresaLeasingIN != null && EmpresaLeasingIN.Count() > 0)
    			expression = expression.And(x => EmpresaLeasingIN.Contains(x.EmpresaLeasing));
    
    		if(Fecha_Factura.HasValue)
    			expression = expression.And(x => x.Fecha_Factura == Fecha_Factura.Value);
    
    		if(Fecha_FacturaIN != null && Fecha_FacturaIN.Count() > 0)
    			expression = expression.And(x => Fecha_FacturaIN.Contains(x.Fecha_Factura));
    	
    		if(Fecha_FacturaFrom.HasValue)
    			expression = expression.And(x => x.Fecha_Factura >= Fecha_FacturaFrom.Value);
    			
    		if(Fecha_FacturaTo.HasValue)
    			expression = expression.And(x => x.Fecha_Factura <= Fecha_FacturaTo.Value);
    				
    
    		if(Fecha_FacturaFromOrNull.HasValue)
                expression = expression.And(x => x.Fecha_Factura >= Fecha_FacturaFromOrNull.Value || x.Fecha_Factura == null);
    
            if(Fecha_FacturaToOrNull.HasValue)
                expression = expression.And(x => x.Fecha_Factura <= Fecha_FacturaToOrNull.Value || x.Fecha_Factura == null);
    	
    		if(!string.IsNullOrWhiteSpace(Num_Factura))  
    			expression = expression.And(x => x.Num_Factura.Equals(Num_Factura));
    			
    		if(!string.IsNullOrWhiteSpace(Num_FacturaContains))  
    			expression = expression.And(x => x.Num_Factura.Contains(Num_FacturaContains));
    			
    		if(!string.IsNullOrWhiteSpace(Num_FacturaStartsWith))
    			expression = expression.And(x => x.Num_Factura.StartsWith(Num_FacturaStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(Num_FacturaEndsWith))
    			expression = expression.And(x => x.Num_Factura.EndsWith(Num_FacturaEndsWith));
    
    		if(Num_FacturaIN != null && Num_FacturaIN.Count() > 0)
    			expression = expression.And(x => Num_FacturaIN.Contains(x.Num_Factura));
    	
    		if(!string.IsNullOrWhiteSpace(Matricula))  
    			expression = expression.And(x => x.Matricula.Equals(Matricula));
    			
    		if(!string.IsNullOrWhiteSpace(MatriculaContains))  
    			expression = expression.And(x => x.Matricula.Contains(MatriculaContains));
    			
    		if(!string.IsNullOrWhiteSpace(MatriculaStartsWith))
    			expression = expression.And(x => x.Matricula.StartsWith(MatriculaStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(MatriculaEndsWith))
    			expression = expression.And(x => x.Matricula.EndsWith(MatriculaEndsWith));
    
    		if(MatriculaIN != null && MatriculaIN.Count() > 0)
    			expression = expression.And(x => MatriculaIN.Contains(x.Matricula));
    	
    		if(!string.IsNullOrWhiteSpace(Ejercicio))  
    			expression = expression.And(x => x.Ejercicio.Equals(Ejercicio));
    			
    		if(!string.IsNullOrWhiteSpace(EjercicioContains))  
    			expression = expression.And(x => x.Ejercicio.Contains(EjercicioContains));
    			
    		if(!string.IsNullOrWhiteSpace(EjercicioStartsWith))
    			expression = expression.And(x => x.Ejercicio.StartsWith(EjercicioStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(EjercicioEndsWith))
    			expression = expression.And(x => x.Ejercicio.EndsWith(EjercicioEndsWith));
    
    		if(EjercicioIN != null && EjercicioIN.Count() > 0)
    			expression = expression.And(x => EjercicioIN.Contains(x.Ejercicio));
    	
    		if(!string.IsNullOrWhiteSpace(Trimestre))  
    			expression = expression.And(x => x.Trimestre.Equals(Trimestre));
    			
    		if(!string.IsNullOrWhiteSpace(TrimestreContains))  
    			expression = expression.And(x => x.Trimestre.Contains(TrimestreContains));
    			
    		if(!string.IsNullOrWhiteSpace(TrimestreStartsWith))
    			expression = expression.And(x => x.Trimestre.StartsWith(TrimestreStartsWith));
    
    		if(!string.IsNullOrWhiteSpace(TrimestreEndsWith))
    			expression = expression.And(x => x.Trimestre.EndsWith(TrimestreEndsWith));
    
    		if(TrimestreIN != null && TrimestreIN.Count() > 0)
    			expression = expression.And(x => TrimestreIN.Contains(x.Trimestre));
    
    		if(Imp_Circulacion.HasValue)
    			expression = expression.And(x => x.Imp_Circulacion == Imp_Circulacion.Value);
    
    		if(Imp_CirculacionIN != null && Imp_CirculacionIN.Count() > 0)
    			expression = expression.And(x => Imp_CirculacionIN.Contains(x.Imp_Circulacion));
    	
    		if(Imp_CirculacionFrom.HasValue)
    			expression = expression.And(x => x.Imp_Circulacion >= Imp_CirculacionFrom.Value);
    			
    		if(Imp_CirculacionTo.HasValue)
    			expression = expression.And(x => x.Imp_Circulacion <= Imp_CirculacionTo.Value);
    				
    
    
    		if(Imp_Circulacion_IVA.HasValue)
    			expression = expression.And(x => x.Imp_Circulacion_IVA == Imp_Circulacion_IVA.Value);
    
    		if(Imp_Circulacion_IVAIN != null && Imp_Circulacion_IVAIN.Count() > 0)
    			expression = expression.And(x => Imp_Circulacion_IVAIN.Contains(x.Imp_Circulacion_IVA));
    	
    		if(Imp_Circulacion_IVAFrom.HasValue)
    			expression = expression.And(x => x.Imp_Circulacion_IVA >= Imp_Circulacion_IVAFrom.Value);
    			
    		if(Imp_Circulacion_IVATo.HasValue)
    			expression = expression.And(x => x.Imp_Circulacion_IVA <= Imp_Circulacion_IVATo.Value);
    				
    
    
    		if(Imp_Matriculacion.HasValue)
    			expression = expression.And(x => x.Imp_Matriculacion == Imp_Matriculacion.Value);
    
    		if(Imp_MatriculacionIN != null && Imp_MatriculacionIN.Count() > 0)
    			expression = expression.And(x => Imp_MatriculacionIN.Contains(x.Imp_Matriculacion));
    	
    		if(Imp_MatriculacionFrom.HasValue)
    			expression = expression.And(x => x.Imp_Matriculacion >= Imp_MatriculacionFrom.Value);
    			
    		if(Imp_MatriculacionTo.HasValue)
    			expression = expression.And(x => x.Imp_Matriculacion <= Imp_MatriculacionTo.Value);
    				
    
    
    		if(Imp_Matriculacion_IVA.HasValue)
    			expression = expression.And(x => x.Imp_Matriculacion_IVA == Imp_Matriculacion_IVA.Value);
    
    		if(Imp_Matriculacion_IVAIN != null && Imp_Matriculacion_IVAIN.Count() > 0)
    			expression = expression.And(x => Imp_Matriculacion_IVAIN.Contains(x.Imp_Matriculacion_IVA));
    	
    		if(Imp_Matriculacion_IVAFrom.HasValue)
    			expression = expression.And(x => x.Imp_Matriculacion_IVA >= Imp_Matriculacion_IVAFrom.Value);
    			
    		if(Imp_Matriculacion_IVATo.HasValue)
    			expression = expression.And(x => x.Imp_Matriculacion_IVA <= Imp_Matriculacion_IVATo.Value);
    				
    
    
    		if(Alquiler.HasValue)
    			expression = expression.And(x => x.Alquiler == Alquiler.Value);
    
    		if(AlquilerIN != null && AlquilerIN.Count() > 0)
    			expression = expression.And(x => AlquilerIN.Contains(x.Alquiler));
    	
    		if(AlquilerFrom.HasValue)
    			expression = expression.And(x => x.Alquiler >= AlquilerFrom.Value);
    			
    		if(AlquilerTo.HasValue)
    			expression = expression.And(x => x.Alquiler <= AlquilerTo.Value);
    				
    
    
    		if(Alquiler_IVA.HasValue)
    			expression = expression.And(x => x.Alquiler_IVA == Alquiler_IVA.Value);
    
    		if(Alquiler_IVAIN != null && Alquiler_IVAIN.Count() > 0)
    			expression = expression.And(x => Alquiler_IVAIN.Contains(x.Alquiler_IVA));
    	
    		if(Alquiler_IVAFrom.HasValue)
    			expression = expression.And(x => x.Alquiler_IVA >= Alquiler_IVAFrom.Value);
    			
    		if(Alquiler_IVATo.HasValue)
    			expression = expression.And(x => x.Alquiler_IVA <= Alquiler_IVATo.Value);
    				
    
    
    		if(Mantenimiento.HasValue)
    			expression = expression.And(x => x.Mantenimiento == Mantenimiento.Value);
    
    		if(MantenimientoIN != null && MantenimientoIN.Count() > 0)
    			expression = expression.And(x => MantenimientoIN.Contains(x.Mantenimiento));
    	
    		if(MantenimientoFrom.HasValue)
    			expression = expression.And(x => x.Mantenimiento >= MantenimientoFrom.Value);
    			
    		if(MantenimientoTo.HasValue)
    			expression = expression.And(x => x.Mantenimiento <= MantenimientoTo.Value);
    				
    
    
    		if(Mantenimiento_IVA.HasValue)
    			expression = expression.And(x => x.Mantenimiento_IVA == Mantenimiento_IVA.Value);
    
    		if(Mantenimiento_IVAIN != null && Mantenimiento_IVAIN.Count() > 0)
    			expression = expression.And(x => Mantenimiento_IVAIN.Contains(x.Mantenimiento_IVA));
    	
    		if(Mantenimiento_IVAFrom.HasValue)
    			expression = expression.And(x => x.Mantenimiento_IVA >= Mantenimiento_IVAFrom.Value);
    			
    		if(Mantenimiento_IVATo.HasValue)
    			expression = expression.And(x => x.Mantenimiento_IVA <= Mantenimiento_IVATo.Value);
    				
    
    
    		if(Neumaticos.HasValue)
    			expression = expression.And(x => x.Neumaticos == Neumaticos.Value);
    
    		if(NeumaticosIN != null && NeumaticosIN.Count() > 0)
    			expression = expression.And(x => NeumaticosIN.Contains(x.Neumaticos));
    	
    		if(NeumaticosFrom.HasValue)
    			expression = expression.And(x => x.Neumaticos >= NeumaticosFrom.Value);
    			
    		if(NeumaticosTo.HasValue)
    			expression = expression.And(x => x.Neumaticos <= NeumaticosTo.Value);
    				
    
    
    		if(Neumaticos_IVA.HasValue)
    			expression = expression.And(x => x.Neumaticos_IVA == Neumaticos_IVA.Value);
    
    		if(Neumaticos_IVAIN != null && Neumaticos_IVAIN.Count() > 0)
    			expression = expression.And(x => Neumaticos_IVAIN.Contains(x.Neumaticos_IVA));
    	
    		if(Neumaticos_IVAFrom.HasValue)
    			expression = expression.And(x => x.Neumaticos_IVA >= Neumaticos_IVAFrom.Value);
    			
    		if(Neumaticos_IVATo.HasValue)
    			expression = expression.And(x => x.Neumaticos_IVA <= Neumaticos_IVATo.Value);
    				
    
    
    		if(Administracion.HasValue)
    			expression = expression.And(x => x.Administracion == Administracion.Value);
    
    		if(AdministracionIN != null && AdministracionIN.Count() > 0)
    			expression = expression.And(x => AdministracionIN.Contains(x.Administracion));
    	
    		if(AdministracionFrom.HasValue)
    			expression = expression.And(x => x.Administracion >= AdministracionFrom.Value);
    			
    		if(AdministracionTo.HasValue)
    			expression = expression.And(x => x.Administracion <= AdministracionTo.Value);
    				
    
    
    		if(Administracion_IVA.HasValue)
    			expression = expression.And(x => x.Administracion_IVA == Administracion_IVA.Value);
    
    		if(Administracion_IVAIN != null && Administracion_IVAIN.Count() > 0)
    			expression = expression.And(x => Administracion_IVAIN.Contains(x.Administracion_IVA));
    	
    		if(Administracion_IVAFrom.HasValue)
    			expression = expression.And(x => x.Administracion_IVA >= Administracion_IVAFrom.Value);
    			
    		if(Administracion_IVATo.HasValue)
    			expression = expression.And(x => x.Administracion_IVA <= Administracion_IVATo.Value);
    				
    
    
    		if(Seguro.HasValue)
    			expression = expression.And(x => x.Seguro == Seguro.Value);
    
    		if(SeguroIN != null && SeguroIN.Count() > 0)
    			expression = expression.And(x => SeguroIN.Contains(x.Seguro));
    	
    		if(SeguroFrom.HasValue)
    			expression = expression.And(x => x.Seguro >= SeguroFrom.Value);
    			
    		if(SeguroTo.HasValue)
    			expression = expression.And(x => x.Seguro <= SeguroTo.Value);
    				
    
    
    		if(Seguro_IVA.HasValue)
    			expression = expression.And(x => x.Seguro_IVA == Seguro_IVA.Value);
    
    		if(Seguro_IVAIN != null && Seguro_IVAIN.Count() > 0)
    			expression = expression.And(x => Seguro_IVAIN.Contains(x.Seguro_IVA));
    	
    		if(Seguro_IVAFrom.HasValue)
    			expression = expression.And(x => x.Seguro_IVA >= Seguro_IVAFrom.Value);
    			
    		if(Seguro_IVATo.HasValue)
    			expression = expression.And(x => x.Seguro_IVA <= Seguro_IVATo.Value);
    				
    
    
    		if(Asistencia_Carretera.HasValue)
    			expression = expression.And(x => x.Asistencia_Carretera == Asistencia_Carretera.Value);
    
    		if(Asistencia_CarreteraIN != null && Asistencia_CarreteraIN.Count() > 0)
    			expression = expression.And(x => Asistencia_CarreteraIN.Contains(x.Asistencia_Carretera));
    	
    		if(Asistencia_CarreteraFrom.HasValue)
    			expression = expression.And(x => x.Asistencia_Carretera >= Asistencia_CarreteraFrom.Value);
    			
    		if(Asistencia_CarreteraTo.HasValue)
    			expression = expression.And(x => x.Asistencia_Carretera <= Asistencia_CarreteraTo.Value);
    				
    
    
    		if(Asistencia_Carretera_IVA.HasValue)
    			expression = expression.And(x => x.Asistencia_Carretera_IVA == Asistencia_Carretera_IVA.Value);
    
    		if(Asistencia_Carretera_IVAIN != null && Asistencia_Carretera_IVAIN.Count() > 0)
    			expression = expression.And(x => Asistencia_Carretera_IVAIN.Contains(x.Asistencia_Carretera_IVA));
    	
    		if(Asistencia_Carretera_IVAFrom.HasValue)
    			expression = expression.And(x => x.Asistencia_Carretera_IVA >= Asistencia_Carretera_IVAFrom.Value);
    			
    		if(Asistencia_Carretera_IVATo.HasValue)
    			expression = expression.And(x => x.Asistencia_Carretera_IVA <= Asistencia_Carretera_IVATo.Value);
    				
    
    
    		if(Intereses_Prepagados.HasValue)
    			expression = expression.And(x => x.Intereses_Prepagados == Intereses_Prepagados.Value);
    
    		if(Intereses_PrepagadosIN != null && Intereses_PrepagadosIN.Count() > 0)
    			expression = expression.And(x => Intereses_PrepagadosIN.Contains(x.Intereses_Prepagados));
    	
    		if(Intereses_PrepagadosFrom.HasValue)
    			expression = expression.And(x => x.Intereses_Prepagados >= Intereses_PrepagadosFrom.Value);
    			
    		if(Intereses_PrepagadosTo.HasValue)
    			expression = expression.And(x => x.Intereses_Prepagados <= Intereses_PrepagadosTo.Value);
    				
    
    
    		if(Intereses_Prepagados_IVA.HasValue)
    			expression = expression.And(x => x.Intereses_Prepagados_IVA == Intereses_Prepagados_IVA.Value);
    
    		if(Intereses_Prepagados_IVAIN != null && Intereses_Prepagados_IVAIN.Count() > 0)
    			expression = expression.And(x => Intereses_Prepagados_IVAIN.Contains(x.Intereses_Prepagados_IVA));
    	
    		if(Intereses_Prepagados_IVAFrom.HasValue)
    			expression = expression.And(x => x.Intereses_Prepagados_IVA >= Intereses_Prepagados_IVAFrom.Value);
    			
    		if(Intereses_Prepagados_IVATo.HasValue)
    			expression = expression.And(x => x.Intereses_Prepagados_IVA <= Intereses_Prepagados_IVATo.Value);
    				
    
    
    		if(Canarias.HasValue)
    			expression = expression.And(x => x.Canarias == Canarias.Value);
    
    		if(CanariasIN != null && CanariasIN.Count() > 0)
    			expression = expression.And(x => CanariasIN.Contains(x.Canarias));
    
    		if(Directivo.HasValue)
    			expression = expression.And(x => x.Directivo == Directivo.Value);
    
    		if(DirectivoIN != null && DirectivoIN.Count() > 0)
    			expression = expression.And(x => DirectivoIN.Contains(x.Directivo));
    
    		if(Fecha_Servicio.HasValue)
    			expression = expression.And(x => x.Fecha_Servicio == Fecha_Servicio.Value);
    
    		if(Fecha_ServicioIN != null && Fecha_ServicioIN.Count() > 0)
    			expression = expression.And(x => Fecha_ServicioIN.Contains(x.Fecha_Servicio));
    	
    		if(Fecha_ServicioFrom.HasValue)
    			expression = expression.And(x => x.Fecha_Servicio >= Fecha_ServicioFrom.Value);
    			
    		if(Fecha_ServicioTo.HasValue)
    			expression = expression.And(x => x.Fecha_Servicio <= Fecha_ServicioTo.Value);
    				
    
    		if(Fecha_ServicioFromOrNull.HasValue)
                expression = expression.And(x => x.Fecha_Servicio >= Fecha_ServicioFromOrNull.Value || x.Fecha_Servicio == null);
    
            if(Fecha_ServicioToOrNull.HasValue)
                expression = expression.And(x => x.Fecha_Servicio <= Fecha_ServicioToOrNull.Value || x.Fecha_Servicio == null);
    
    		if(Fecha_Importacion.HasValue)
    			expression = expression.And(x => x.Fecha_Importacion == Fecha_Importacion.Value);
    
    		if(Fecha_ImportacionIN != null && Fecha_ImportacionIN.Count() > 0)
    			expression = expression.And(x => Fecha_ImportacionIN.Contains(x.Fecha_Importacion));
    	
    		if(Fecha_ImportacionFrom.HasValue)
    			expression = expression.And(x => x.Fecha_Importacion >= Fecha_ImportacionFrom.Value);
    			
    		if(Fecha_ImportacionTo.HasValue)
    			expression = expression.And(x => x.Fecha_Importacion <= Fecha_ImportacionTo.Value);
    				
    
    		if(Fecha_ImportacionFromOrNull.HasValue)
                expression = expression.And(x => x.Fecha_Importacion >= Fecha_ImportacionFromOrNull.Value || x.Fecha_Importacion == null);
    
            if(Fecha_ImportacionToOrNull.HasValue)
                expression = expression.And(x => x.Fecha_Importacion <= Fecha_ImportacionToOrNull.Value || x.Fecha_Importacion == null);
    
    		if(ITV.HasValue)
    			expression = expression.And(x => x.ITV == ITV.Value);
    
    		if(ITVIN != null && ITVIN.Count() > 0)
    			expression = expression.And(x => ITVIN.Contains(x.ITV));
    	
    		if(ITVFrom.HasValue)
    			expression = expression.And(x => x.ITV >= ITVFrom.Value);
    			
    		if(ITVTo.HasValue)
    			expression = expression.And(x => x.ITV <= ITVTo.Value);
    				
    
    
    		if(ITV_IVA.HasValue)
    			expression = expression.And(x => x.ITV_IVA == ITV_IVA.Value);
    
    		if(ITV_IVAIN != null && ITV_IVAIN.Count() > 0)
    			expression = expression.And(x => ITV_IVAIN.Contains(x.ITV_IVA));
    	
    		if(ITV_IVAFrom.HasValue)
    			expression = expression.And(x => x.ITV_IVA >= ITV_IVAFrom.Value);
    			
    		if(ITV_IVATo.HasValue)
    			expression = expression.And(x => x.ITV_IVA <= ITV_IVATo.Value);
    				
    
    
    		if(Impuesto.HasValue)
    			expression = expression.And(x => x.Impuesto == Impuesto.Value);
    
    		if(ImpuestoIN != null && ImpuestoIN.Count() > 0)
    			expression = expression.And(x => ImpuestoIN.Contains(x.Impuesto));
    	
    		if(ImpuestoFrom.HasValue)
    			expression = expression.And(x => x.Impuesto >= ImpuestoFrom.Value);
    			
    		if(ImpuestoTo.HasValue)
    			expression = expression.And(x => x.Impuesto <= ImpuestoTo.Value);
    				
    
    
    		if(Sociedad.HasValue)
    			expression = expression.And(x => x.Sociedad == Sociedad.Value);
    
    		if(SociedadIN != null && SociedadIN.Count() > 0)
    			expression = expression.And(x => SociedadIN.Contains(x.Sociedad));
    	
    		if(SociedadFrom.HasValue)
    			expression = expression.And(x => x.Sociedad >= SociedadFrom.Value);
    			
    		if(SociedadTo.HasValue)
    			expression = expression.And(x => x.Sociedad <= SociedadTo.Value);
    				
    
    
    		if(FechaAlta.HasValue)
    			expression = expression.And(x => x.FechaAlta == FechaAlta.Value);
    
    		if(FechaAltaIN != null && FechaAltaIN.Count() > 0)
    			expression = expression.And(x => FechaAltaIN.Contains(x.FechaAlta));
    	
    		if(FechaAltaFrom.HasValue)
    			expression = expression.And(x => x.FechaAlta >= FechaAltaFrom.Value);
    			
    		if(FechaAltaTo.HasValue)
    			expression = expression.And(x => x.FechaAlta <= FechaAltaTo.Value);
    				
    
    		if(FechaAltaFromOrNull.HasValue)
                expression = expression.And(x => x.FechaAlta >= FechaAltaFromOrNull.Value || x.FechaAlta == null);
    
            if(FechaAltaToOrNull.HasValue)
                expression = expression.And(x => x.FechaAlta <= FechaAltaToOrNull.Value || x.FechaAlta == null);
    	
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
    
    		if(this.ECAR_Datos_Vehiculo != null)
    		{
    			var subExpression = ECAR_Datos_Vehiculo.GetExpression();
    			expression = expression.And(x => (new[] { x.ECAR_Datos_Vehiculo }).AsQueryable().Any(subExpression));
    		}
    
    		if(this.T_M_EMPRESAS_VEHICULOS != null)
    		{
    			var subExpression = T_M_EMPRESAS_VEHICULOS.GetExpression();
    			expression = expression.And(x => (new[] { x.T_M_EMPRESAS_VEHICULOS }).AsQueryable().Any(subExpression));
    		}
    	
    		return expression;
    	}
    	
    	public bool IsSatisfiedBy(T_G_DATOS_LEASING entity)
    	{
    		// convert single entity to a IQueryable object, 
    		// in order to be able to use lambda expressions
    		IQueryable<T_G_DATOS_LEASING> entities = (new[] { entity }).AsQueryable();
    		
    		return entities.Any(this.GetExpression());
    	}
    
        public override string ToString()
        {
            return Evaluator.PartialEval(this.GetExpression()).ToString();
        }

        #endregion

    }
}
