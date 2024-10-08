using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Models;
using TK_ECAR.Application_Services;
using TKUtilidades;
using TK_ECAR.Framework;
using TK_ECAR.Utils;

namespace TK_ECAR.Filters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private bool perfilSinDefinir = false;
        private bool navegadorCompatible = true;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            bool retornarNulo = false;

            if (Global.EsNavegadorCompatible(httpContext))
            {
                var usersPru = System.Configuration.ConfigurationManager.AppSettings["usersPruebas"];
                var aUsersPru = usersPru.Split(',');
                UserModel user = (UserModel)Util.GetItemFromMemory("userProfile");
                if (user == null)
                {
                    var userService = new UsersService();
#if DEBUG
                    //var username = "CREYES"; //es Administrativo
                    //               var username = "JLANZAS";
                    //var username = "lalcaide"; //es delegado de zona 01

                    var username = "JGONZALEZFE"; //es superusuario

                    // var username = "ASANCHEZU"; //es director territorial

#else
                    var username = httpContext.User.Identity.Name.Split('\\').Last();
               
                if (aUsersPru.Any(x=>x.Trim().Equals(username.ToUpper())))
                {
                    username = "JGONZALEZFE";  
                }
#endif

                    // var username = httpContext.User.Identity.Name.Split('\\').Last();

                    if (Global.appActiva.ToUpper() == "SI")
                    {

                        user = userService.GetUser(username);

                        if (user != null)
                        {
                            if (user.IdPerfil != Constants.PERFIL_SIN_DEFINIR)
                            {
                                user.DescripcionPerfil = EnumUtils<EnumTipoPerfil>.GetDescription((EnumTipoPerfil)user.IdPerfil);
                                Util.AddItemToMemory("userProfile", user);
                            }
                            else
                            {
                                perfilSinDefinir = true;
                                retornarNulo = true;
                            }
                        }
                    }
                    else
                    {
                        user = null;
                    }

                }
                return user != null ? (retornarNulo == true ? false : true) : false;
            }
            else
            {
                navegadorCompatible = false;
                return false;
            }

        }
         
        
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {


            RedirectToRouteResult valorReturn = null;

            if (!navegadorCompatible)
            {
                valorReturn = GetRedirectToRouteResult("Error", "BrowserNotSupported");
            }
            else if (Global.appActiva.ToUpper() == "NO")
            {
                valorReturn = GetRedirectToRouteResult("Error", "ApplicationNotAvailable");
            }
            else if (perfilSinDefinir)
            {
                valorReturn = GetRedirectToRouteResult("Error", "ProfileNotDefined");
            }
            else
            {
                valorReturn = GetRedirectToRouteResult("Error", "UnauthorizedAccess");
            }

            filterContext.Result = valorReturn;
        }


        private RedirectToRouteResult GetRedirectToRouteResult(string controlador, string vista)
        {
            RedirectToRouteResult valorReturn = null;

            valorReturn = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = controlador,
                            action = vista
                        })
                    );

            return valorReturn;
        }
    }

}
