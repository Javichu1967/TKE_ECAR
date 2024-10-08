using System;
using System.Web.Mvc;
using log4net;
using System.Net;
using System.Web.Routing;
using TK_ECAR.Framework.Exceptions;

namespace TK_ECAR.Filters
{
    
    public class HandleCustomException : HandleErrorAttribute
    {
        readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(ExceptionContext filterContext)
        {
            //base.OnException(filterContext);


            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }
            logger.Error(filterContext.Exception);

            filterContext.HttpContext.Response.Clear();
            
            filterContext.ExceptionHandled = true;

            if (filterContext.HttpContext.Request.IsAjaxRequest())//es una petición ajax
            {
                if (filterContext.Exception.GetType() == (typeof(EmailException)))
                {
                    filterContext.HttpContext.Response.StatusCode = 278;
                }
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
              
            }
            else
            {
                //es un error de base de datos
                if (filterContext.Exception.GetType() == (typeof(System.Data.Entity.Core.EntityException)))
                {
                    
                    filterContextRedirect(filterContext, "Error", "EntityExpception");

                }                
                else
                {//otro tipo de excepción

                    var controllerName = (string)filterContext.RouteData.Values["controller"];
                    var actionName = (string)filterContext.RouteData.Values["action"];
                    var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                    filterContext.Result = new ViewResult
                    {
                        ViewName = View,
                        MasterName = Master,
                        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                        TempData = filterContext.Controller.TempData
                    };

                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                }

                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
    
        }


        private void filterContextRedirect(ExceptionContext filterContext, string controller, string action)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = controller,
                                action = action
                            })
                        );
        }

        //private bool IsAjax(ExceptionContext filterContext)
        //{
        //    return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        //}

    }
}
