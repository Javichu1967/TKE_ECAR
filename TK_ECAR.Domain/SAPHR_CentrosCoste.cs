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
    
    public partial class SAPHR_CentrosCoste
    {
        public string IdCeco { get; set; }
        public string Nombre { get; set; }
        public int Empresa { get; set; }
        public bool Baja { get; set; }
        public Nullable<System.DateTime> FechaBaja { get; set; }
        public string IdDelegacion { get; set; }
        public string IdDT { get; set; }
    
        public virtual SAPHR_Delegaciones SAPHR_Delegaciones { get; set; }
        public virtual SAPHR_DireccionesTerritoriales SAPHR_DireccionesTerritoriales { get; set; }
        public virtual SAPHR_Empresas SAPHR_Empresas { get; set; }
    }
}
