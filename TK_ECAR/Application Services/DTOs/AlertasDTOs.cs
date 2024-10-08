using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TK_ECAR.Application_Services.DTOs
{
    public class AlertaDataTableModel
    {
        public int? IdAlerta { get; set; }

        public int IdTipoAlerta { get; set; }

        public int? Prioridad { get; set; }
        public string Alerta { get; set; }

        public string Matricula { get; set; }

        public string Ceco { get; set; }

        public string Modelo { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public int? IdEstado { get; set; }

        public string Estado { get; set; }

        public int? IdAccion { get; set; }

        public string Accion { get; set; }

        public bool EnableCancelar { get; set; }

        public bool EnableRechazar { get; set; }

        public string ConductorConfirmado { get; set; }
        public int? IdPerfil { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool EstadoInicial { get; set; }
        public bool Automatica { get; set; }
        public string NombreUsuarioCreacion { get; set; }
        public string idDelegacion { get; set; }
        public string DescDelegacion { get; set; }
        public int? Sociedad { get; set; }
    }


    public class DatosAlerta
    {
        public int IdAlerta { get; set; }

        public int IdTipoAlerta { get; set; }

        public int IdEstado { get; set; }

        public string Matricula { get; set; }

        public string Observaciones { get; set; }

        public string Fichero { get; set; }

        public DatosCambioConductor DatosCambioConductor { get; set; }

        public DatosMulta DatosMulta { get; set; }

        public DatosRobo DatosRobo { get; set; }

        public DatosOtraNotificacion DatosOtraNotificacion { get; set; }
        

        public DatosSolicitudSOLRED DatosSolicitudSOLRED { get; set; }

        public DatosRenovacionITV DatosRenovacionITV { get; set; }

        public DatosRenovacionCarnet DatosRenovacionCarnet { get; set; }

        public ConductorConfirmadoMulta ConductorConfirmadoMulta { get; set; }

        public ConductorConfirmadoRenting ConductorConfirmadoRenting { get; set; }

    }

    public class DatosCambioConductor
    {

        public string Nombre { get; set; }


        public int NumEmpleado { get; set; }


        public string Dni { get; set; }


        public string Provincia { get; set; }

        public string Poblacion { get; set; }


        public string Domicilio { get; set; }


        public string CodigoPostal { get; set; }

        public string Motivo { get; set; }

        public DateTime? FechaNacimiento { get; set; }


        public DateTime? FechaVencimientoCarnet { get; set; }

        public string Observaciones { get; set; }

        public string FicheroEmiteRenting { get; set; }
        public DateTime FechaEfecto { get; set; }
    }

    public class DatosMulta
    {
        public string Expendiente { get; set; }

        public DateTime FechaDenuncia { get; set; }
        public int HoraDenuncia { get; set; }
        public int MinutosDenuncia { get; set; }

        public string Infracion { get; set; }

        public string Lugar { get; set; }

        public decimal? Importe { get; set; }
        public int? IDTipoDocIdentificacion { get; set; }
        public string NumeroDocIdentificacion { get; set; }

    }

    public class DatosRobo
    {

        public DateTime FECHA_ROBO { get; set; }

    }

    public class DatosOtraNotificacion
    {

        public int IdTipoClasificacion { get; set; }

        public string Observaciones { get; set; }

        public string Fichero { get; set; }

    }
    public class DatosSolicitudSOLRED
    {

        public DateTime FechaSolicitud { get; set; }
        public int IdTipoSolicitud { get; set; }

        public string Descripcion { get; set; }
    }


    public class DatosRenovacionITV
    {
        public DateTime FechaITV { get; set; }

        public DateTime FechaCaducidadITV { get; set; }

        public string FicheroITV { get; set; }
    }

    public class DatosRenovacionCarnet
    {
        public DateTime FechaCaducidadCarnet { get; set; }

        public string FicheroCarnet { get; set; }

        public string Conductor { get; set; }

        public int CodigoConductor { get; set; }

        public string DNI { get; set; }

    }

    public class ConductorConfirmadoMulta
    {

        public string Nombre { get; set; }

        public string DNI { get; set; }

        public string NumPermisoConducir { get; set; }

        public string NacionalidadPermiso { get; set; }

        public bool? ValidezPermisoESP { get; set; }

        public string Pais { get; set; }


        public string Provincia { get; set; }


        public string Poblacion { get; set; }


        public string Domicilio { get; set; }

        public string CodigoPostal { get; set; }


        public bool? AutorizacionPermiso { get; set; }


        public string FicheroCarnet { get; set; }


    }

    public class ConductorConfirmadoRenting
    {
        public int CodigoConductor { get; set; }
        public string Nombre { get; set; }

        public string DNI { get; set; }

        public string Provincia { get; set; }


        public string Poblacion { get; set; }


        public string Direccion { get; set; }

        public string CodigoPostal { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public DateTime? FechaVencimientoCarnet { get; set; }

    }
}