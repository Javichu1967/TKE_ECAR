using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class PreguntasModel
    {
        public int idPregunta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCategoria")]
        [Required(ErrorMessageResourceName = "RequiredCategoria", ErrorMessageResourceType = typeof(resources))]
        public int idCategoria { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPregunta")]
        [Required(ErrorMessageResourceName = "RequiredPregunta", ErrorMessageResourceType = typeof(resources))]
        public string Pregunta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblRespuesta")]
        [Required(ErrorMessageResourceName = "RequiredRespuesta", ErrorMessageResourceType = typeof(resources))]
        public string Respuesta { get; set; }

        public EnumAccionEntity Accion { get; set; }
        public int numOrdenCategoria { get; set; }

    }

    public class PreguntasDataTableModel : PreguntasModel
    {
        public string expnad { get; set; }

        public string Categoria { get; set; }
        public int ID_Empresa { get; set; }
        public string DescEmpresa { get; set; }
        public string AccionDatatable { get; set; }

    }


    public class CategoriaPregunta
    {
        public int idCategoria { get; set; }

        public string Descripcion { get; set; }
        public int numOrdenCategoria { get; set; }
    }

}

