using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class FilterModel
    {
        [Display(ResourceType = typeof(resources), Name = "lblSelecionEmpresa")]
        [UIHint("Empresas")]
        public string Empresa { get; set; }
        public string EmpresasSeleccionadas { get; set; }
        //public List<Empresas> ListaEmpresas { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblSelecionDireccionTerritorial")]
        [UIHint("DireccionesTerritoriales")]
        public string DireccionTerritorial { get; set; }
        public string DireccionesTerritorialesSeleccionadas { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblSelecionDelegaciones")]
        [UIHint("Delegaciones")]
        public string Delegacion { get; set; }
        public string DelegacionesSeleccionadas { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblSelecionCentrosCoste")]
        [UIHint("CentrosCoste")]
        public string CentroCoste { get; set; }
        public string CentrosCosteSeleccionados { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblSelecionAgrupacion")]
        //[UIHint("AgrupacionesFiltro")]
        public List<SelectListItem> AgrupacionesFiltro { get; set; }
        public string AgrupacionesFiltroSeleccionadas { get; set; }

        public EnumAltasBajasAmbas EstadoVehiculo { get; set; }

        public string AccionFiltroActiva { get; set; }

        //public List<Empresas> ListaEmpresas { get; set; }
        //public List<DireccionesTerritoriales> ListaDireccionesTerritoriales { get; set; }
        //public List<Delegaciones> ListaDelegaciones { get; set; }
        //public List<CentrosCoste> ListaCentrosCoste { get; set; }

    }

    public class FilterAlertaModel : FilterModel
    {

        [UIHint("TiposAlertas")]
        [Display(ResourceType = typeof(resources), Name = "lblTipoAlertas")]
        public string TiposAlertas { get; set; }
        public string TiposAlertasSeleccionadas { get; set; }

        [UIHint("TiposEstadosAlertas")]
        [Display(ResourceType = typeof(resources), Name = "lblEstadoAlertas")]
        public string TiposEstadosAlertas { get; set; }
        public string TiposEstadosAlertasSeleccionadas { get; set; }

        [UIHint("TiposAccionAlerta")]
        [Display(ResourceType = typeof(resources), Name = "lblAccionAlertas")]
        public string TiposAccionAlerta { get; set; }
        public string TiposAccionAlertaSeleccionadas { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(resources), Name = "lblFechaCreacionDesde")]
        public DateTime? FechaCreacionAlertaDesde { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(resources), Name = "lblFechaCreacionHasta")]
        public DateTime? FechaCreacionAlertaHasta { get; set; }
    }


    public class FilterInformeFlotaModel : FilterModel
    {
        [Display(ResourceType = typeof(resources), Name = "lblMarca")]
        [UIHint("MarcaVehiculoMultiChosen")]
        public string Marcas { get; set; }
        public string MarcasSeleccionadas { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblModelo")]
        [UIHint("ModeloVehiculoMultiChosen")]
        public string Modelos { get; set; }
        public string ModelosSeleccionados { get; set; }

        [Display(ResourceType = typeof(resources), Name = "lblEmpresaLeasing")]
        [UIHint("LeasingMultiChosen")]
        public string EmpresasLeasing { get; set; }//****EmpresaLeasing
        public string EmpresasLeasingSeleccionadas { get; set; }//****EmpresaLeasing

        [Display(ResourceType = typeof(resources), Name = "lblFechaContDesde")]
        [Required(ErrorMessageResourceName = "RequiredFechaContDesde", ErrorMessageResourceType = typeof(resources))]
        [UIHint("CalendarMonthYear")]
        public string F_Desde { get; set; }
        public DateTime? FechaDesde { get; set; }

        public string MesAñoFechaDesdeText
        {
            get
            {
                if (FechaDesde == null)
                {
                    return "";
                }
                else
                {
                    return $"{FechaDesde.Value.ToString("MMMM", Thread.CurrentThread.CurrentCulture)} {FechaDesde.Value.Year.ToString()}";
                }
            }
        }


        [Display(ResourceType = typeof(resources), Name = "lblFechaContHasta")]
        [UIHint("CalendarMonthYear")]
        public string F_Hasta { get; set; }
        public DateTime? FechaHasta { get; set; }

        public string MesAñoFechaHastaText
        {
            get
            {
                if (FechaHasta == null)
                {
                    return "";
                }
                else
                {
                    return $"{FechaHasta.Value.ToString("MMMM", Thread.CurrentThread.CurrentCulture)} {FechaHasta.Value.Year.ToString()}";
                }
            }
        }

    }


    public class SelectChosen
    {
        private string _text = string.Empty;
        private string _value = string.Empty;
        private bool _ponerValuePorDelanteDeTexto = true;
        private bool _devolverValueFormateado = true;

        public bool PonerValuePorDelanteDeTexto
        {
            get
            {
                return _ponerValuePorDelanteDeTexto;
            }
            set
            {
                _ponerValuePorDelanteDeTexto = value;
            }
        }

        public bool DevolverValueFormateado
        {
            get
            {
                return _devolverValueFormateado;
            }
            set
            {
                _devolverValueFormateado = value;
            }
        }

        public string text
        {
            get
            {
                if (_ponerValuePorDelanteDeTexto)
                {
                    return textFormated;
                }
                else
                {
                    return _text;
                }
            }
            set
            {
                _text = value;
            }
         }
        public string value
        {
            get
            {
                if (_devolverValueFormateado)
                {
                    return valueFormated;
                }
                else
                {
                    return _value;
                }
            }
            set
            {
                _value = value;
            }
        }


        public string valueFormated
        {
            get
            {

                if (!string.IsNullOrEmpty(_value))
                {
                    return _value.TrimStart('0');
                }
                else
                {
                    return "nulo";
                }
            }
        }

        public string textFormated
        {
            get
            {
                if (!string.IsNullOrEmpty(_value) && !string.IsNullOrEmpty(_text))
                {
                    return string.Format("{0} - {1}", _value.TrimStart('0'), _text);
                }
                else
                {
                    return string.Format("{0} - {1}", "nulo", "nulo");
                }
            }
        }
    }

    public class Empresas
    {
        public int idEmpresa { get; set; }
        public string NombreEmpresa { get; set; }

    }


    public class DireccionesTerritoriales
    {
        public string idDireccionTerritorial { get; set; }
        public string NombreDireccionTerritorial { get; set; }
    }


    public class Delegaciones
    {
        public string idDelegacion { get; set; }
        public string idDireccionTerritorial { get; set; }
        public string NombreDelegacion { get; set; }
    }


    public class CentrosCoste
    {
        public string idCentroCoste { get; set; }
        public string idDelegacion { get; set; }
        public string idDireccionTerritorial { get; set; }
        public string NombreCentroCoste { get; set; }
    }
}
