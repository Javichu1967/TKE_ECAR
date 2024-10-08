using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.IO;

namespace TKClasesGenericas.Mail
{
    [Serializable]
    public class TKEMailMessage
    {

        /// <summary>
        /// Envía un mensaje de Correo Electrónico formateado usando el protocolo SMTP
        /// </summary>
        /// <param name="to">Lista de usuarios a los que se envía el correo </param>
        /// <param name="bcc">Lista de usuarios a los que se envía el correo -en copia oculta-</param>
        /// <param name="cc">Lista de usuarios a los que se envía el correo -en copia-</param>
        /// <param name="subject">asunto</param>
        /// <param name="subject">asunto</param>
        /// <param name="htmlbody">Mensaje en html que irá en el email.</param>
        public static void SendMailMessage(ArrayList to, ArrayList bcc, ArrayList cc, string subject, string  htmlbody,  string archivo )
        {
            try
            {
                MailMessage mMailMessage = new MailMessage();
                 
                //Se añaden los destinatarios
                for (int i = 0; i < to.Count; i++)
                    {
                        if (to[i].ToString() != null && (to[i].ToString() != string.Empty))
                        {
                            foreach (string aux in to[i].ToString().Split(';'))
                            {
                                mMailMessage.To.Add(new MailAddress(aux));
                            }
                        }
                    }

                if (cc != null)
                {
                    // Destinatarios en copia
                    for (int i = 0; i < cc.Count; i++)
                    {
                        if (cc[i].ToString() != null && (cc[i].ToString() != string.Empty))
                        {
                            foreach (string aux in cc[i].ToString().Split(';'))
                            {
                                mMailMessage.CC.Add(new MailAddress(aux));
                            }
                        }
                    }
                }
                if (bcc != null)
                {

                    // Destinatarios en copia oculta
                    for (int i = 0; i < bcc.Count; i++)
                    {
                        if (bcc[i].ToString() != null && (bcc[i].ToString() != string.Empty))
                        {
                            foreach (string aux in bcc[i].ToString().Split(';'))
                            {
                                mMailMessage.Bcc.Add(new MailAddress(aux));
                            }
                        }
                    }
                }
              

                // Asunto del mensaje
                mMailMessage.Subject = subject;
             
              
                //Para poder insertar una imagen
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlbody, null, "text/html");
                //Se añaden las rutas donde se encuentrar las imágenes
                //string sPathFile1 = HttpContext.Current.Server.MapPath(@"~/Content/img/Application/Logo-TK_2x_mail.png");
                Uri uriAddress = new Uri($"{ConfigurationManager.AppSettings["baseUrl"]}/Content/img/layout/Logo-TK_2x_mail.png");

                HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(uriAddress);
                HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

                Stream sPathFile1 = aResponse.GetResponseStream(); //HttpContext.Current.Server.MapPath($"{ConfigurationManager.AppSettings["baseUrl"]}/Content/img/layout/Logo-TK_2x_mail.png");

                string sType = "image/png";
                LinkedResource imageLink1 = GetLinkImage(sPathFile1, sType, "image1Id");

                htmlView.LinkedResources.Add(imageLink1);
                mMailMessage.AlternateViews.Add(htmlView);

                if (archivo!="")
                {
                    Attachment data = new Attachment(archivo, MediaTypeNames.Application.Octet);
                    mMailMessage.Attachments.Add(data);
                }

               

                mMailMessage.Priority = MailPriority.Normal;
                SmtpClient mSmtpClient = new SmtpClient();

                /// Send the mail message
                mSmtpClient.Send(mMailMessage);

            }

            catch (Exception ex)
            {
                //Se captura el error y se guarda una traza en el Registro de eventos del sistema
                throw ex;
            }
        }

        /// <summary>
        /// adjunta una imagen en el correo
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="sType"></param>
        /// <param name="sContentId"></param>
        /// <returns></returns>
        internal static LinkedResource GetLinkImage(Stream sFileName, string sType, string sContentId)
        {
            LinkedResource imagelink = new LinkedResource(sFileName, sType);
            imagelink.ContentId = sContentId;
            imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            return imagelink;
        }

      
   

    }
}
