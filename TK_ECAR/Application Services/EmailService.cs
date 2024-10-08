using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Framework.Exceptions;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using TKClasesGenericas.Mail;
using TKUtilidades;

namespace TK_ECAR.Application_Services
{
    public class EmailService
    {
        #region enviar mail
        public void SendEmailrechazoAlerta(int idAlerta, string textoAsunto, string textoRechazo)
        {
            try
            {
                var alerta = getAlertaEmailById(idAlerta);

                SendEmail(alerta, textoAsunto, textoRechazo, true);

            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message, ex);
            }
        }

        public void SendEmailAlerta(int idAlerta)
        {
            try
            {
                var alerta = getAlertaEmailById(idAlerta);

                SendEmail(alerta);

            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message, ex);
            }
        }
        public void SendEmailAlerta(UserModel user)
        {
            try
            {
                var alerta = getAlertaLastAlertaByUser(user);

                SendEmail(alerta);

            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message, ex);
            }
        }


        private void SendEmail(AlertaEmail Alerta, string textoAsunto = "", string TextoCuerpo = "", bool esRechazo = false)
        {
            bool sinDestinatarios = false;
            string mensajeNoDestinatarios = "";
            try
            {
                var service = new UsersService();

                var emails = service.GetReceiveEmail(Alerta.Ceco);

                if (emails == null)
                {
                    sinDestinatarios = true;
                }
                else
                {
                    if (emails.Count() == 0)
                    {
                        sinDestinatarios = true;
                    }
                }

                if (sinDestinatarios)
                {
                    emails = service.GetReceiveEmailGestores();
                    mensajeNoDestinatarios = " (No se han encontrado usuarios activos para recibir mensajes con el Centro de Coste (" + Alerta.Ceco + "))";
                }

                var htmlBody = FormatBodyToHTML(Alerta, TextoCuerpo, esRechazo);

                var to = new ArrayList();
                //#if DEBUG
                if (!Global.EstoyEnProduccion())
                {
                    to.Add("francisco.botas.indra@thyssenkrupp.com");
                    //#else         
                }
                else
                {
                    to.AddRange(emails);
                }
                //#endif

                var asunto = $"E-CAR. Solicitud de alerta {textoAsunto} {mensajeNoDestinatarios}";

                TKEMailMessage.SendMailMessage(to, null, null, asunto, htmlBody, string.Empty);

            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message, ex);
            }
        }


        private AlertaEmail getAlertaEmailById(int idAlerta)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var alerta = (from x in unitOfWork.RepositoryT_G_ALERTAS.Fetch()
                              where x.ID_ALERTA.Equals(idAlerta)
                              select new AlertaEmail
                              {
                                  Tipo = x.T_M_TIPOS_ALERTAS.DESCRIPCION,
                                  Estado = x.T_M_ESTADOS.DESCRIPCION,
                                  Matricula = x.MATRICULA,
                                  Accion = x.T_M_ACCIONES.DESCRIPCION,
                                  Modelo = x.MODELO,
                                  Ceco = x.ID_CECO,
                                  Prioridad = x.T_M_TIPOS_ALERTAS.PRIORIDAD,
                                  Login = x.USUARIO_CREACION,
                              }).FirstOrDefault();
                if (alerta != null)
                {
                    SAPHR_UsuariosSAPSpecification spec = new SAPHR_UsuariosSAPSpecification
                    {
                        Logon = alerta.Login,
                    };

                    var usuario = unitOfWork.RepositorySAPHR_UsuariosSAP.Where(spec).FirstOrDefault();
                    alerta.Usuario = string.Format("{0} {1} {2}", usuario.Nombre, usuario.Apellido1, usuario.Apellido2);
                }

                return alerta;
            }
        }

        private AlertaEmail getAlertaLastAlertaByUser(UserModel user)
        {
            using (var unitOfWork = new UnitOfWork())
            {


                return (from x in unitOfWork.RepositoryT_G_ALERTAS.Fetch()
                        where x.USUARIO_CREACION.Equals(user.Login)
                        orderby x.ID_ALERTA descending
                        select new AlertaEmail
                        {
                            Tipo = x.T_M_TIPOS_ALERTAS.DESCRIPCION,
                            Estado = x.T_M_ESTADOS.DESCRIPCION,
                            Matricula = x.MATRICULA,
                            Accion = x.T_M_ACCIONES.DESCRIPCION,
                            Modelo = x.MODELO,
                            Ceco = x.ID_CECO,
                            Prioridad = x.T_M_TIPOS_ALERTAS.PRIORIDAD,
                            Usuario = user.Nombre
                        }).FirstOrDefault();
            }
        }

        private List<htmlEmail> getEmailParametersHtml()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                return (from x in unitOfWork.RepositoryGENERAL_PARAMETROS_APLICACIONES.Fetch()
                        where x.AGRUPACION.ToUpper().Equals("EMAIL")
                        orderby x.NUM_ORDEN
                        select new htmlEmail
                        {
                            AGRUPACION = x.AGRUPACION,
                            PARAMETRO = x.PARAMETRO,
                            VALOR = x.VALOR,
                        }
                        ).ToList();
            }
        }

        private string FormatBodyToHTML(AlertaEmail alerta, string textoCuerpo = "", bool esRechazo = false)
        {
            var request = HttpContext.Current.Request;
            var address = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority) + VirtualPathUtility.ToAbsolute("~/Alertas");
            var tbody = "<tbody>";

            tbody += "<tr valign='top' class='small'>";
            tbody += "<td>" + alerta.Prioridad + "</td>";
            tbody += "<td>" + alerta.Tipo + "</td>";
            tbody += "<td>" + alerta.Matricula + "</td>";
            tbody += "<td>" + alerta.Ceco + "</td>";
            tbody += "<td>" + alerta.Modelo + "</td>";
            tbody += "<td>" + alerta.Estado + "</td>";
            tbody += "<td>" + alerta.Accion + "</td>";
            tbody += "</tr>";
            tbody += "</tbody>";

            List<htmlEmail> parametrosHtml = new List<htmlEmail>();

            parametrosHtml = getEmailParametersHtml();

            string htmlMail = parametrosHtml.Where(o => o.PARAMETRO.ToUpper() == "HTML").FirstOrDefault().HtmlValor;

            string htmlHead = parametrosHtml.Where(o => o.PARAMETRO.ToUpper() == "HEAD").FirstOrDefault().HtmlValor.Replace("SustituirPorNombreaplicacion", "E-CAR");

            string htmlBody = @" <tr valign='top' bgcolor='#ffffff' >
                                        <td>
    	                                    <br>
                                            <span class='normal'> Solicitud de alerta: </span>
                                            <br>                        
                                            <span class='normal'> Usuario solicitante: </span> <span class='negrita'> " + alerta.Usuario +
                                                @"</span>";
            if (esRechazo)
            {
                htmlBody = htmlBody + @"<br>                        
                                            <span class='normal'> Usuario Rechazo: </span> <span class='negrita'> " + ((UserModel)Util.GetItemFromMemory("userProfile")).Nombre +
                                                @"</span>";
            }

            htmlBody = htmlBody + @"</td>
                                    </tr>
	                                <tr valign='top' bgcolor='#ffffff'>
    	                                <td>
		                                    <br>		  	
		                                    <span class='small'> Información de la alerta:</span> 
                                            <table width='100%' border='1' cellpadding='0' cellspacing='0' >
		                                        <thead class='cabecera' >
                                                <tr valign='top' align='center'>
                                                    <th class='colorth'>Prioridad</th>
                                                    <th class='colorth'>Alerta</th>    			                       
                                                    <th class='colorth'>Matricula</th>
    			                                    <th class='colorth'>Ceco</th>    			               
    			                                    <th class='colorth'>Modelo</th>    			                      
    			                                    <th class='colorth'>Estado</th>
    			                                    <th class='colorth'>Acción</td>    			                
                                                </tr>
		                                        </thead>" + tbody +
                                                @"</table>  	
    	                                </td>
	                                </tr>";
            if (textoCuerpo !="")
            {
                htmlBody = htmlBody + @"<tr valign='top' bgcolor='#ffffff' >
                                        <td>
    	                                    <br />
								            <span class='normal'>" + textoCuerpo + @"</span><br><br>
                                        </td>
                                    </tr>";
            }

            htmlBody = htmlBody + @"<tr valign='top' bgcolor='#ffffff' >
                                        <td>
    	                                    <br />
								            <span class='normal'><a  href=" + address + @">Pulse aquí para ir alertas</a></span><span class='negrita'></span><br><br>
                                        </td>
                                    </tr>";

            string htmlFooter = parametrosHtml.Where(o => o.PARAMETRO.ToUpper() == "FOOTER").FirstOrDefault().HtmlValor;

            return htmlMail + htmlHead + htmlBody + htmlFooter;
        }

        private class htmlEmail
        {
            public string AGRUPACION { get; set; }
            public string PARAMETRO { get; set; }
            public string VALOR { get; set; }

            public string HtmlValor
            { get
                {
                    return WebUtility.HtmlDecode(VALOR);
                }
            }
            

        }

        #endregion

        /// <summary>
        /// Método que envía el email con la incidencia
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <param name="emailUsuario"></param>
        /// <param name="descripcionIncidencia"></param>
        /// <param name="codigoApp"></param>
        /// <param name="nombreApp"></param>
        /// <param name="fichero"></param>
        /// <returns></returns>
        public string EnviarEmail(string loginUsuario, string nombreUsuario, string emailUsuario, string emailCAU, string codApp, string nombreApp, string descripcionIncidencia, string path)
        {
            try
            {
                List<string> listaEmailUser = new List<string>();
                listaEmailUser.Add(emailUsuario);

                MonitorizacionCorreoModel modelCorreo = new MonitorizacionCorreoModel
                {
                    CodigoAplicacion = codApp,
                    NombreAplicacion = nombreApp,
                    EmailFrom = emailUsuario,
                    EmailTo = emailCAU,
                    EmailsCC = ConfigurationManager.AppSettings["Entorno"].ToLower().Equals("pruebas")
                                ? (ConfigurationManager.AppSettings["emailsCC"] != "" ? ConfigurationManager.AppSettings["emailsCC"].Split(';').ToList() : new List<string>())
                                : listaEmailUser,
                    EmailsCCO = ConfigurationManager.AppSettings["emailsCCO"] != "" ? ConfigurationManager.AppSettings["emailsCCO"].Split(';').ToList() : new List<string>(),
                    AsuntoEmail = "INCIDENCIA " + codApp + " - " + nombreApp,
                    CuerpoEmail = "Por favor, asignar la incidencia a <b>DESARROLLO INTRANET</b>. <br/><br/> <u>Descripción de la incidencia</u> :  <br/><br/>" + descripcionIncidencia.Trim(),
                    FechaEnvioCorreo = DateTime.Now,
                    LoginUsuario = loginUsuario,
                    UsuarioCreacion = nombreUsuario,
                    FechaActualizacion = DateTime.Now,
                    UsuarioModificacion = nombreUsuario,
                    Prioridad = ConfigurationManager.AppSettings["prioridad"],
                    UrlApp = ConfigurationManager.AppSettings["urlApp"] != "" ? ConfigurationManager.AppSettings["urlApp"] : "",
                    Path = path
                };

                string respuesta = RepositorioWebApiService.GuardarLogCorreo(modelCorreo);

                if (respuesta.Contains("OK"))
                {
                    string resultado = RepositorioWebApiService.EnviarEmail(modelCorreo);

                    if (resultado.Contains("OK"))
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

    }
}