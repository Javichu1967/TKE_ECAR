using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class ResumenImportacionModels
    {
        public string Empresa { get; set; }
        public int TotalElementosImportados { get; set; }
        public int TotalElementosImportadosOK { get; set; }
        public int TotalElementosImportadosError { get; set; }
        public List<Incidencia> ListadoResumen { get; set; }
    }

    public class Incidencia
    {
        public EnumTipoLineaImportacion TipoLinea { get; set; }
        public string Texto { get; set; }
    }


}
