using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TK_ECAR.Filters;
using TK_ECAR.Utils;
using TKSeguridad;

namespace TK_ECAR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(Models.AlertaModel), new AlertaModelBinder()); //Indicamos que para la clase AlertaModel, se ejecute este método
            ModelBinders.Binders.Add(typeof(Models.RenovarModel), new AlertaModelBinder()); //Indicamos que para la clase RenovarModel, se ejecute este método

            var path = Server.MapPath(ConfigurationManager.AppSettings["PathLog4net"]);

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            logger.Error(exception);
            Server.ClearError();
            if (exception.GetType() == typeof(HttpException) && ((HttpException)exception).GetHttpCode() == 404)
            {
                Response.Redirect("~/Error/Error404");//o por custom error
            }
            else
            {

                Response.Redirect("~/Error/Index");
            }
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string sCulture = Global.IdiomaPorDefecto();

            if (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.LANG] != null)
                sCulture = HttpContext.Current.Session[Constants.LANG].ToString();


            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(sCulture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        protected void Session_Start(object sender, EventArgs e)
        {


            if (!Request.IsLocal)
            {
                if (TKSeguridadWeb.ChequeaAutorizacionEntrada("WEB0106") == false)
                {
                    Response.Redirect(TKSeguridadWeb.PaginaNoAutorizacion);
                    return;
                }
            }


        }
    }


}
