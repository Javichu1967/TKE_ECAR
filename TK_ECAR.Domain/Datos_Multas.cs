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
    
    public partial class Datos_Multas
    {
        public string Matricula { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> Conductor { get; set; }
        public Nullable<double> Importe { get; set; }
        public string Motivo { get; set; }
        public Nullable<bool> Contrario { get; set; }
        public Nullable<bool> Cobertura { get; set; }
        public Nullable<bool> EmitidaAlerta { get; set; }
        public Nullable<System.DateTime> FAlta { get; set; }
    }
}
