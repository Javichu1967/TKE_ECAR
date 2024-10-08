using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class GeneracionFacturacionModels
    {
        //[Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        //[UIHint("EmpresaChosen")]
        public string Empresa { get; set; }
        //[Required(ErrorMessageResourceName = "RequiredEmpresa", ErrorMessageResourceType = typeof(resources))]
        public int IDEmpresa { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresaLeasing")]
        [UIHint("LeasingSeguroChosen")]
        public string EmpresaLeasing { get; set; }
        [Required(ErrorMessageResourceName = "RequiredEmpresaLeasing", ErrorMessageResourceType = typeof(resources))]
        public int IDEmpresaLeasing { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFechaFacturacion", ErrorMessageResourceType = typeof(resources))]
        [Display(ResourceType = typeof(resources), Name = "lblFechaFacturacion")]
        [UIHint("CalendarMonthYear")]
        public string F_Facturacion { get; set; }
        public DateTime? FechaFacturacion { get; set; }

    }
}
