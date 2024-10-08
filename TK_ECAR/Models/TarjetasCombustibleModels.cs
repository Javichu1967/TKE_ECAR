using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Filters;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class TarjetasCombustibleModels
    {
        public int IDTarjeta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCodigoTarjeta")]
        [Required(ErrorMessageResourceName = "RequiredCodigoTarjeta", ErrorMessageResourceType = typeof(resources))]
        [StringLength(19, ErrorMessageResourceName = "MaxLenCodigoTarjeta", ErrorMessageResourceType = typeof(resources))]
        public string CodTarjeta { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        [Required(ErrorMessageResourceName = "RequiredEmpresa", ErrorMessageResourceType = typeof(resources))]
        public int? IDEmpresa { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresaEmisora")]
        [Required(ErrorMessageResourceName = "RequiredEmpresaEmisora", ErrorMessageResourceType = typeof(resources))]
        public int? IDEmpresaEmisora { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaCaducidad")]
        [Required(ErrorMessageResourceName = "RequiredFechaCaducidad", ErrorMessageResourceType = typeof(resources))]
        [DataType(DataType.Date)]
        public DateTime? FechaCaducidad { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPIN")]
        //[Required(ErrorMessageResourceName = "RequiredPIN", ErrorMessageResourceType = typeof(resources))]
        [StringLength(15, ErrorMessageResourceName = "MaxLenPIN", ErrorMessageResourceType = typeof(resources))]
        [TarjetaCombustiblePinObligatorio(fieldEmpresa = "IDEmpresa", ErrorMessageResourceName = "RequiredPIN", ErrorMessageResourceType = typeof(resources))]
        public string PIN { get; set; }

        public bool Baja { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresa")]
        [UIHint("EmpresaChosen")]
        public string Empresa { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresaEmisora")]
        [UIHint("EmpresaEmisoraTarjeta")]
        public string EmpresaEmisora { get; set; }

        public string MatriculaAsociadaTarjeta { get; set; }

        public EnumAccionEntity Accion { get; set; }

        public int IdAlerta { get; set; } //Lo añado, porque usare este modelo en las alertas de Tarjeta de Combustible.
        public int IdEstado { get; set; } //Lo añado, porque usare este modelo en las alertas de Tarjeta de Combustible.
    }

    public class TarjetasCombustibleDataTableModel : TarjetasCombustibleModels
    {
        public string AccionDatatable { get; set; }
        public string NombreEmpresaEmisora { get; set; }
        public string NombreEmpresaTarjeta { get; set; }
    }

}
