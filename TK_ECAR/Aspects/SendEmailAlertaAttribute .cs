using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections;
using System.Linq;
using System.Web;
using TK_ECAR.Application_Services;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TKClasesGenericas.Mail;
using TK_ECAR.Framework.Exceptions;
using TKUtilidades;
using TK_ECAR.Utils;

namespace TK_ECAR.Aspects
{
    [PSerializable]
    public sealed class SendEmailAlertaAttribute : OnMethodBoundaryAspect
    {
        public override void OnSuccess(MethodExecutionArgs args)
        {
            try
            {
                var user = (UserModel)Util.GetItemFromMemory("userProfile");

                new EmailService().SendEmailAlerta(user);

                //                var alerta = getAlertaSolicitada(user);               

                //                var htmlBody = new EmailService().FormatBodyToHTML(alerta);

                //                var service = new UsersService();

                //                var emails = service.GetReceiveEmail(alerta.Ceco);

                //                var to = new ArrayList();
                //#if DEBUG

                //                to.Add("david.tejedor-accuro@thyssenkrupp.com");
                //#else
                //                to.AddRange(emails);
                //#endif

                //                TKEMailMessage.SendMailMessage(to,null, null, "Solicitud de alerta", htmlBody , string.Empty);

            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message, ex); 
            }
        }


        //private AlertaEmail getAlertaSolicitada(UserModel user)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {


        //        return  (from x in unitOfWork.RepositoryT_G_ALERTAS.Fetch()
        //                where x.USUARIO_CREACION.Equals(user.Login)
        //                orderby x.ID_ALERTA descending
        //                select new AlertaEmail
        //                {
        //                    Tipo = x.T_M_TIPOS_ALERTAS.DESCRIPCION,
        //                    Estado = x.T_M_ESTADOS.DESCRIPCION,
        //                    Matricula = x.MATRICULA,
        //                    Accion = x.T_M_ACCIONES.DESCRIPCION,
        //                    Modelo = x.MODELO,
        //                    Ceco = x.ID_CECO,                          
        //                    Prioridad = x.T_M_TIPOS_ALERTAS.PRIORIDAD,
        //                    Usuario = user.Nombre
        //                }).FirstOrDefault();  
        //    }
        //}
        //private class AlertaEmail
        //{
        //    public string Tipo { get; set; }
        //    public string Estado { get; set; }
        //    public string Matricula { get; set; }
        //    public string Accion { get; set; }
        //    public string Modelo { get; set; }
        //    public string Ceco { get; set; }        
        //    public int Prioridad { get; set; }

        //    public string Usuario { get; set; }

        //}
       

       // private string FormatBodyToHTML(AlertaEmail alerta)
       // {

       //     var request = HttpContext.Current.Request;
       //     var address = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority) + VirtualPathUtility.ToAbsolute("~/Alertas"); 
       //     var tbody = "<tbody>";

       //     tbody += "<tr valign='top' class='small'>";            
       //     tbody += "<td>" + alerta.Prioridad + "</td>";
       //     tbody += "<td>" + alerta.Tipo + "</td>";
       //     tbody += "<td>" + alerta.Matricula + "</td>";
       //     tbody += "<td>" + alerta.Ceco + "</td>";
       //     tbody += "<td>" + alerta.Modelo + "</td>";           
       //     tbody += "<td>" + alerta.Estado + "</td>";
       //     tbody += "<td>" + alerta.Accion + "</td>";
       //     tbody += "</tr>";
       //     tbody += "</tbody>";


       //     // -- Iniciamos el Formato --
       //     string htmlbody =
       //     @"<html lang='en'>
       //     <head>
       //         <meta charset='utf-8'>   
       //         <title>Mail</title>    
       //         <style type='text/css'>
    
       //         body {
       //           color:#000000;
       //           font-family: tktype, Arial, Verdana, sans-serif;
       //           background-color:#FFFFFF;    
       //           background-repeat:no-repeat;
       //         }
       //         a  { color:#0000FF; }
       //         a:visited { color:#800080; }
       //         a:hover { color:#008000; }
       //         a:active { color:#FF0000; }
	      //      .table{
		     //       border-color:#00A0F0;
		     //       color:#ffffff;
		     //       font-size: medium;
	      //       }
	      //      .negrita{ 
	 		   //     color:black; 
			    //    font-family:tktype, Arial, Verdana, sans-serif;
			    //    font-weight:bold;
			    //    font-size:0.8em;
			    //}

	      //      .normal{ 
	 		   //     color:black; 
			    //    font-family:tktype, Arial, Verdana, sans-serif;
			    //    font-size:0.8em;
			    //}
	      //      .small{
	 		   //     color:black; 
			    //    font-family:tktype, Arial, Verdana, sans-serif;
			    //    font-size:0.7em;
	      //       }
	      //      .vsmall{
			    //    font-family:tktype, Arial, Verdana, sans-serif;
			    //    font-size:0.5em;
			    //    color:#ffffff;
	      //       }

	      //       .cabecera{
	 		   //     font-family:tktype, Arial, Verdana, sans-serif;
			    //    font-size:0.7em;
       //             font-weight:bold;
			    //    color:#ffffff;
		     
	      //        }
       //          .colorth{ 
       //             background-color:#00A0F0; 
       //          }    
       //     </style>  
       //   </head>
       //   <body>
       //     <div> 
       //         <table width='100%' cellpadding='0'   class='table' bgcolor='#00A0F0' cellspacing='0' >
       //             <thead bgcolor='#00A0F0'>
       //                 <tr>
       //                     <td><img src='cid:image1Id'  width='70px' height='70px'/></td>
       //                     <td><span class='cabecera'> E-CAR</span></td>
       //                 </tr>
       //             </thead>
       //             <tbody> 
       //                 <tr valign='top' bgcolor='#ffffff' >
       //                     <td>
    	  //                      <br>
       //                         <span class='normal'> Solicitud de alerta: </span>
       //                         <br>                        
       //                         <span class='normal'> Usuario solicitante: </span> <span class='negrita'> " + alerta.Usuario + 
       //                             @"</span>
       //                     </td>
       //                 </tr>
	      //              <tr valign='top' bgcolor='#ffffff'>
    	  //                  <td>
		     //                   <br>		  	
		     //                   <span class='small'> Información de la alerta:</span> 
       //                         <table width='100%' border='1' cellpadding='0' cellspacing='0' >
		     //                       <thead class='cabecera' >
       //                             <tr valign='top' align='center'>
       //                                 <th class='colorth'>Prioridad</th>
       //                                 <th class='colorth'>Alerta</th>    			                       
       //                                 <th class='colorth'>Matricula</th>
    			//                        <th class='colorth'>Ceco</th>    			               
    			//                        <th class='colorth'>Modelo</th>    			                      
    			//                        <th class='colorth'>Estado</th>
    			//                        <th class='colorth'>Acción</td>    			                
       //                             </tr>
		     //                       </thead>" + tbody +
       //                             @"</table>  	
    	  //                  </td>
	      //              </tr>
	      //              <tr valign='top' bgcolor='#ffffff' >
       //                     <td>
    	  //                      <br />
							//	<span class='normal'><a  href=" + address + @">Pulse aquí para ir alertas</a></span><span class='negrita'></span><br><br>
       //                     </td>
       //                 </tr>
       //                 <tr valign='center' >
    	  //                  <td>
		     //                   <br />
       //                         <span class='vsmall'>
       //                             This e-mail has been generated by an automatic system as a result of an action linked to your entity. Please, do not use sender's email to reply this email as its reception is not guaranteed.
		     //                       <br /> 	
		     //                       <br />
       //                         </span>	
    	  //                  </td>
	      //              </tr>
       //             </tbody>
       //         </table>
       //     </div>
       //   </body>
       // </html>";


       //     return htmlbody;
       // }

    }
}