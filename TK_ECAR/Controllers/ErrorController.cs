using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using TK_ECAR.Application_Services;

namespace TK_ECAR.Controllers
{
   
    public class ErrorController : Controller
    {       
        public ViewResult Error404()
        {
            return View();
        }

        public ViewResult EntityExpception()
        { 
            return View();
        }

        public ViewResult Index()
        {
            return View("Error");
        }

        public ViewResult UnauthorizedAccess()
        {

            var UsersAcceso = ConfigurationManager.AppSettings["usersPeticionAcceso"].ToString();
            List<string> lUsers = new List<string>();
            foreach (string user in UsersAcceso.Split(','))
            {
                lUsers.Add(user.Trim());
            }

            ViewData["usuariosPeticion"] = new UsersService().GetUsuarioSAP_PeticionAcceso(lUsers);

            return View();
        }

        public ViewResult ApplicationNotAvailable()
        {
            return View();
        }
        public ViewResult ProfileNotDefined()
        {
            return View();
        }

        public ViewResult BrowserNotSupported()
        {
            return View();
        }

    }
}
