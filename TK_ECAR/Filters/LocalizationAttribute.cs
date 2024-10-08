using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Utils;

namespace TK_ECAR.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LocalizationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //AQUÍ FJBA. PARA PONER EL IDIOMA.

            //Accedo a la sesion para obtener los datos relativos al idioma.
            string sCulture = Global.IdiomaPorDefecto();
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.LANG] != null)
                sCulture = HttpContext.Current.Session[Constants.LANG].ToString();

            // Pongo la cultura
            SetCultureOnThread(sCulture);

            //hago lo que tendría que hacer...

            base.OnActionExecuting(filterContext);
        }

        public static void SetCultureOnThread(string language)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture; 
            //Thread.CurrentThread.CurrentUICulture.DateTimeFormat ;
        }
    }
}