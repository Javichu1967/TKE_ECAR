//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TK_ECAR.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class ECAR_Datos_Vehiculo
    {
        public ECAR_Datos_Vehiculo()
        {
            this.ECAR_Datos_ITV = new HashSet<ECAR_Datos_ITV>();
            this.T_G_DATOS_LEASING = new HashSet<T_G_DATOS_LEASING>();
            this.T_G_ALERTAS = new HashSet<T_G_ALERTAS>();
            this.ECAR_Datos_SolRed = new HashSet<ECAR_Datos_SolRed>();
        }
    
        public int Sociedad { get; set; }
        public string Matricula { get; set; }
        public Nullable<int> Marca { get; set; }
        public Nullable<int> Modelo { get; set; }
        public string Extras { get; set; }
        public Nullable<int> Tipo_Vehiculo { get; set; }
        public string Num_Bastidor { get; set; }
        public string CC { get; set; }
        public string Departamento { get; set; }
        public string Delegacion { get; set; }
        public Nullable<bool> F_D { get; set; }
        public Nullable<int> Directivo { get; set; }
        public Nullable<int> EmpresaLeasing { get; set; }
        public Nullable<int> Conductor { get; set; }
        public Nullable<int> Tipo_Seguro { get; set; }
        public Nullable<int> Cia_Seguro { get; set; }
        public string Poliza_Seguro { get; set; }
        public Nullable<double> Importe_Seguro { get; set; }
        public Nullable<System.DateTime> Vto_Seguro { get; set; }
        public string Veh_sustituido { get; set; }
        public string Num_Contrato { get; set; }
        public Nullable<System.DateTime> Fecha_Alta { get; set; }
        public Nullable<bool> Baja { get; set; }
        public Nullable<System.DateTime> Fecha_Baja { get; set; }
        public Nullable<System.DateTime> Fecha_Recibidos { get; set; }
        public Nullable<System.DateTime> Fecha_Devolucion { get; set; }
        public Nullable<System.DateTime> Fecha_Incorporacion { get; set; }
        public Nullable<int> Cuotas { get; set; }
        public Nullable<int> Km_Totales { get; set; }
        public Nullable<int> IDTipoLiquidacion { get; set; }
        public Nullable<int> Exceso_ajuste { get; set; }
        public Nullable<int> Coef_exceso { get; set; }
        public Nullable<int> Km_Exentos { get; set; }
        public Nullable<double> Abono { get; set; }
        public Nullable<double> Cargo { get; set; }
        public Nullable<int> IDCarburante { get; set; }
        public Nullable<bool> Orden { get; set; }
        public Nullable<int> Ubicacion { get; set; }
        public string Observaciones { get; set; }
        public Nullable<System.DateTime> Falta { get; set; }
        public Nullable<int> IDTipoRuta { get; set; }
        public Nullable<int> IDTarjetaCombustible { get; set; }
        public string LugarEntrega { get; set; }
        public Nullable<System.DateTime> FechaRenovacion { get; set; }
        public string Equipamiento { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioAlta { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Responsable { get; set; }
        public string PrioridadEntrega { get; set; }
        public string IdentificadorImportacion { get; set; }
        public Nullable<System.DateTime> FechaImportacion { get; set; }
        public Nullable<System.DateTime> FechaMatriculacion { get; set; }
        public string UsuarioImportacion { get; set; }
        public Nullable<System.DateTime> FechaPrevistaEntrega { get; set; }
        public string NOMBRE_ARCHIVO_IMPORTACION { get; set; }
        public Nullable<System.DateTime> Fecha_Vto_Contrato { get; set; }
    
        public virtual ECAR_Datos_Conductor ECAR_Datos_Conductor { get; set; }
        public virtual ECAR_Tipo_Liquidacion ECAR_Tipo_Liquidacion { get; set; }
        public virtual T_M_MARCA_VEHICULOS T_M_MARCA_VEHICULOS { get; set; }
        public virtual T_M_MODELOS_VEHICULO T_M_MODELOS_VEHICULO { get; set; }
        public virtual T_M_RUTA_VEHICULOS T_M_RUTA_VEHICULOS { get; set; }
        public virtual T_M_TIPO_SEGURO_VEHICULO T_M_TIPO_SEGURO_VEHICULO { get; set; }
        public virtual T_M_TIPOS_CARBURANTE T_M_TIPOS_CARBURANTE { get; set; }
        public virtual T_M_TIPOS_VEHICULO T_M_TIPOS_VEHICULO { get; set; }
        public virtual T_M_UBICACIONES T_M_UBICACIONES { get; set; }
        public virtual ICollection<ECAR_Datos_ITV> ECAR_Datos_ITV { get; set; }
        public virtual ICollection<T_G_DATOS_LEASING> T_G_DATOS_LEASING { get; set; }
        public virtual T_M_EMPRESAS_VEHICULOS T_M_EMPRESAS_VEHICULOS { get; set; }
        public virtual T_M_TARJETAS_CONBUSTIBLE T_M_TARJETAS_CONBUSTIBLE { get; set; }
        public virtual ICollection<T_G_ALERTAS> T_G_ALERTAS { get; set; }
        public virtual ICollection<ECAR_Datos_SolRed> ECAR_Datos_SolRed { get; set; }
        public virtual ECAR_Datos_Multas ECAR_Datos_Multas { get; set; }
    }
}
