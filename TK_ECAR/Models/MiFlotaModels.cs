using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class MiFlotaDatatableModel
    {
        public int Sociedad { get; set; }

        public string Matricula { get; set; }

        public string Conductor { get; set; }

        public string CECO { get; set; }

        public string Modelo { get; set; }

        public DateTime? FechaAlta { get; set; }
        public int? Cuotas { get; set; }

        public DateTime? FechaRecogida { get; set; }

        public DateTime? FechaVtoRenting { get; set; }

        public string CiaRenting { get; set; }

        public DateTime? FechaProximaITV { get; set; }

        public string NumContrato { get; set; }

        public DateTime? FechaVtoSeguro { get; set; }

        public string Accion { get; set; }

        public bool? Baja { get; set; }

        public string EstadoDelVehiculo { get; set; }

    }









}
