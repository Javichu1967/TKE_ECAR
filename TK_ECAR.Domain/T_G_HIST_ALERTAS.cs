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
    
    public partial class T_G_HIST_ALERTAS
    {
        public int ID_HIST_ALERTA { get; set; }
        public int ID_ALERTA { get; set; }
        public int ID_ESTADO { get; set; }
        public Nullable<int> ID_ACCION { get; set; }
        public string USUARIO_CREACION { get; set; }
        public System.DateTime FECHA_CREACION { get; set; }
    
        public virtual T_G_ALERTAS T_G_ALERTAS { get; set; }
        public virtual T_M_ACCIONES T_M_ACCIONES { get; set; }
        public virtual T_M_ESTADOS T_M_ESTADOS { get; set; }
    }
}
