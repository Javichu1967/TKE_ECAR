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
    
    public partial class T_G_USUARIOS_DELEGACION
    {
        public int ID_USUARIO { get; set; }
        public string ID_DELEGACION { get; set; }
    
        public virtual T_G_USUARIOS T_G_USUARIOS { get; set; }
    }
}
