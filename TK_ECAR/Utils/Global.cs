using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using TK_ECAR.Content.resources;
using TK_ECAR.Framework;

namespace TK_ECAR.Utils
{
    public class Global
    {
        public enum TipoDeLog { DEBUG, INFO, ERROR };

        private static string _idiomaPordefecto = string.Empty;

        public static string PorcentajeImpuestoPeninsula = ConfigurationManager.AppSettings["PORCENTAJE_IMPUESTO_PENINSULA"].ToString(); 

        public static string PathToUploadDocument = ConfigurationManager.AppSettings["PathToUploadDocument"].ToString();

        public static string PathToUploadDocumentMiVehiculo = ConfigurationManager.AppSettings["PathToUploadDocument"].ToString() + "Vehiculo/";

        public static string PathToUploadDocumentAlertas = ConfigurationManager.AppSettings["PathToUploadDocument"].ToString() +  "Alertas/";

        public static string PathToUploadFotoGestoresFlota = ConfigurationManager.AppSettings["PathToUploadFotoGestoresFlota"].ToString();

        public static string PathToUploadDocumentITV = $"{PathToUploadDocument}ITV/";

        public static string PathToUploadDocumentITV_TMP = $"{PathToUploadDocument}ITV_TMP/";

        public static string PathToUploadDocumentFacturacion = ConfigurationManager.AppSettings["PathToUploadDocumentFacturacion"].ToString(); 

        public static string PathToPathToImportFilesFlota = ConfigurationManager.AppSettings["PathToImportFiles"].ToString() + "FLOTA/";
        public static string PathToPathToImportFilesViaVerde = ConfigurationManager.AppSettings["PathToImportFiles"].ToString() + "VIAVERDE/";
        public static string PathToPathToImportFilesConsumoCombustible = ConfigurationManager.AppSettings["PathToImportFiles"].ToString() + "ConsumoCombustible/";
        public static string PathToPathToImportFilesRenting = ConfigurationManager.AppSettings["PathToImportFiles"].ToString() + "RENTING/";

        //public static string IdiomaPorDefecto = ConfigurationManager.AppSettings["IdiomaPorDefecto"].ToString();

        public static string codigoAplicacion = ConfigurationManager.AppSettings["appCode"].ToString(); 

        public static string appActiva = ConfigurationManager.AppSettings["AplicacionActiva"].ToString();

        public static string PermitirMantenimientoVehiculo = ConfigurationManager.AppSettings["PermitirMantenimientoVehiculo"].ToString(); 

        public static string GetPathToUploadDocumentMiVehiculo(string subDirectorios, string caracterAsustituir = "", string caracterNuevo = "") 
        {
            return PathToUploadDocumentMiVehiculo + (caracterAsustituir == "" ? subDirectorios : subDirectorios.Replace(caracterAsustituir, caracterNuevo)) + "/";
        }

        public static int ID_TURISMO_DIRECCION()
        {
            var valorReturn = 0;
            try
            {
                string idTurismoDireccion = ConfigurationManager.AppSettings["ID_TURISMO_DIRECCION"].ToString();

                valorReturn = Convert.ToInt32(idTurismoDireccion);
            }
            catch (Exception ex)
            {
                Global.EscribeLogApp(TipoDeLog.ERROR, $"<ID_TURISMO_DIRECCION>. {ex.Message}");
            }
            return valorReturn;
        }


        /// <summary>
        /// Devuelve una lista de nullable int
        /// </summary>
        /// <param name="valoresInt">Valores a los que se les hace split</param>
        /// <param name="caracterSeparacion"></param>
        /// <returns></returns>
        public static List<int?> GetSplitListNullableInt(string valoresInt, char caracterSeparacion)
        {
            List<int?> lInt = new List<int?>();
            foreach (string v in valoresInt.Split(caracterSeparacion))
            {
                lInt.Add(ToNullableInt(v));
            }
            return lInt;
        }

        private static int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public static bool EsNumerico(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return true;
            return false;
        }


        //public static IEnumerable<T> Flatten<T>(
        //    this IEnumerable<T> e,
        //    Func<T, IEnumerable<T>> f)
        //{
        //    return e.SelectMany(c => f(c).Flatten(f)).Concat(e);
        //}

        public static bool EstoyEnProduccion()
        {
            bool valorReturn = true;

            try
            {
                var produccion = ConfigurationManager.AppSettings["produccion"].ToString();

                if (produccion != "1")
                {
                    valorReturn = false;
                }
            }

            catch(Exception ex)
            {
                //No hacemos nada.
            }

            return valorReturn;
        }
        

        public static DateTime ConcatHoursAndMinutesToDate(DateTime fecha, int hora, int minutos)
        {
            return Convert.ToDateTime(string.Format("{0} {1}:{2}", fecha.ToString("dd/MM/yyyy"), hora.ToString("00"), minutos.ToString("00")));
        }

        public static string IdiomaPorDefecto()
        {
            if (string.IsNullOrEmpty(_idiomaPordefecto))
            {
                _idiomaPordefecto = ConfigurationManager.AppSettings["IdiomaPorDefecto"];
                if (_idiomaPordefecto == null)
                {
                    _idiomaPordefecto = Thread.CurrentThread.CurrentCulture.Name;
                }
            }

            return _idiomaPordefecto;
        }

        /// <summary>
        /// Escribe una línea de log usando Log4Net.
        /// </summary>
        /// <param name="tipolog">Enumerador de tipos de log. Si es DEBUG y estamos en producción, no lo escribe</param>
        /// <param name="mensaje">Mensaje que queremos escribir en el log</param>
        /// <returns></returns>
        public static bool EscribeLogApp(TipoDeLog tipolog, string mensaje)
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            bool valorReturn = true;

            try { 
                switch (tipolog)
                {
                    case TipoDeLog.ERROR:
                        logger.Error(mensaje);
                        break;
                    case TipoDeLog.INFO:
                        logger.Info(mensaje);
                        break;
                    case TipoDeLog.DEBUG:
                        logger.Debug(mensaje);
                        break;
                }
            }

            catch (Exception ex)
            {

                valorReturn = false;
            }

            return valorReturn;
        }


        /// <summary>
        /// Devuelve los literales de los perfiles, según el idioma, separados por comas.
        /// </summary>
        /// <returns></returns>
        public static string GetLiteralesPerfilesSeparadosPorComas()
        {
            var ListaPerfiles = "";
            //int i = 0;
            int i = 1;
            foreach (EnumTipoPerfil perfil in Enum.GetValues(typeof(EnumTipoPerfil)))
            {
                ListaPerfiles = ListaPerfiles + (i > 1 ? "," : "") + TK_ECAR_Resource.ResourceManager.GetString("Perfil_" + i.ToString());
                //ListaPerfiles = ListaPerfiles + (i>0 ? "," : "") + EnumUtils<EnumTipoPerfil>.GetDescription((EnumTipoPerfil)(int)perfil);
                i++;
            }

            return ListaPerfiles;
        }

        public static EnumAccionEntity GetValueFromEnumAccionEntity(string accion)
        {
            EnumAccionEntity valorReturn = EnumAccionEntity.SinAccion;

            foreach (EnumAccionEntity perfil in Enum.GetValues(typeof(EnumAccionEntity)))
            {
                if (accion.ToUpper() == perfil.ToString().ToUpper())
                {
                    return perfil;
                }
            }

            return valorReturn;
        }

        public static string GetMessageError(Exception ex)
        {
            string valorReturn = string.Empty;

            valorReturn = ex.Message;
            if (ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                {
                    if (!string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                    {
                        valorReturn = ex.InnerException.InnerException.Message;
                    }
                }
            }


            return valorReturn;
        }

        #region Excel
        public static string DevuelveTextoFromExcel(object valorCelda, bool quitarTodosLosEspaciosEnBlanco = false, bool esMatricula = false, bool esMatriculaEspañola = false)
        {
            string valorReturn = null;

            if (valorCelda != null)
            {
                if (quitarTodosLosEspaciosEnBlanco)
                {
                    valorReturn = Convert.ToString(valorCelda).Trim();
                }
                else
                {
                    valorReturn = Convert.ToString(valorCelda);
                }
                if (esMatricula)
                {
                    if (!esMatriculaEspañola)
                    {
                        if (valorReturn.IndexOf('-') == -1)
                        {
                            valorReturn = $"{valorReturn.Substring(0, 2)}-{valorReturn.Substring(2, 2)}-{valorReturn.Substring(4)}";
                        }
                    }
                    else
                    {
                        if (valorReturn.IndexOf('-') == -1)
                        {
                            valorReturn = $"{valorReturn.Substring(0, 4)}-{valorReturn.Substring(4)}";
                        }
                    }
                }
            }

            return valorReturn;
        }

        public static DateTime? DevuelveFechaFromExcel(object fecha)
        {
            DateTime? valorReturn = null;

            if (fecha != null)
            {
                valorReturn = Convert.ToDateTime(fecha);
            }

            return valorReturn;
        }

        public static DateTime GetFormatedFechaFromString(string fecha)
        {
            return Convert.ToDateTime($"{fecha.Substring(6)}/{fecha.Substring(4, 2)}/{fecha.Substring(0, 4)}");
        }

        public static string GetFormatedHourString(string hora)
        {
            return $"{hora.Substring(0,2)}:{hora.Substring(3)}";
        }

        public static int? DevuelveIntFromExcel(object valorCelda)
        {
            int? valorReturn = null;

            if (valorCelda != null)
            {
                valorReturn = Convert.ToInt32(valorCelda);
            }

            return valorReturn;
        }

        public static double? DevuelveDoubleFromExcel(object valorCelda)
        {
            double? valorReturn = null;

            if (valorCelda != null)
            {
                valorReturn = Convert.ToDouble(valorCelda);
            }

            return valorReturn;
        }
        #endregion Excel

        #region Numbers
        public static double Truncate(double value, int places)
        {
            // not sure if you care to handle negative numbers...       
            var f = Math.Pow(10, places);
            return Math.Truncate(value * f) / f;
        }
        #endregion Numbers

        #region Comprueba Navegador
        public static bool EsNavegadorCompatible(HttpContextBase httpContext)
        {
            var valorReturn = true;

            if (httpContext.Request.Browser.Browser.ToLower() == "internetexplorer" &&
                (httpContext.Request.Browser.Version.Contains("7") ||
                httpContext.Request.Browser.Version.Contains("8")))
            {
                valorReturn = false;
            }

            return valorReturn;
        }

        #endregion Comprueba Navegador
    }
}