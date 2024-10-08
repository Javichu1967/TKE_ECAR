using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosConductorModel
    {
        [Display(ResourceType = typeof(resources), Name = "lblMatricula")]
        public string Matricula { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblApellidos")]
        public string Apellidos { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDireccion")]
        public string Direccion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCodigoPostal")]
        public string CodPostal { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblPoblacion")]
        public string Poblacion { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblProvincia")]
        public string Provincia { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblTelefono")]
        public string Telefono { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblMovil")]
        public string Movil { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaNacimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblFechaVtoCarnet")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaVtoCarnet { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblCECO")]
        public string CECO { get; set; }


        [Display(ResourceType = typeof(resources), Name = "lblCarnetConducir")]
        public string CarnetConducir { get; set; }


        [Display(ResourceType = typeof(resources), Name = "lblNombre")]
        public string NombreCompleto { get { return Nombre + " " + Apellidos; } }

        [Display(ResourceType = typeof(resources), Name = "lblDomicilio")]
        public string DireccionCompleta {
            get {
                return Direccion + ". " + (!string.IsNullOrEmpty(CodPostal) ? " (" + CodPostal + "); " :  "") +
                         (!string.IsNullOrEmpty(Provincia) ? Provincia + "; " : "") +
                         (!string.IsNullOrEmpty(Poblacion) ? Poblacion : "");
            }
        }

        public int idAlertaCarnet { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblDni")]
        public string DNI { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblNumEmpleado")]
        public string NumEmpleado { get; set; }

        //En la importación de vehículos si el conductor no tiene nº empleado en la Hoja Excel,
        //  se marca este campo.
        [Display(ResourceType = typeof(resources), Name = "lblPendienteDefinir")]
        public string PendienteDefinir { get; set; }
    }


    public class ConductoresVehiculoModel : DatosConductorModel
    {
        public List<DatoHistoricoConductores> ListaConductores { get; set; }

    }


    public class DatoHistoricoConductores
    {
        public string Conductor { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Fecha { get; set; }
    }

}
