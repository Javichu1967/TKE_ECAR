using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Content.resources;
using TK_ECAR.Framework;
using TK_ECAR.Models;

namespace TK_ECAR.Controllers
{
    public class BorrarImportacionController : BaseController
    {
        #region index - Sustituyo el método Index, por cada uno de los mantenimientos, para que se remarque en el menú, el que está activo.
        public ActionResult BorrarImportacionFlota(int tipoBorrado)
        {
            BorrarImportacionModels modelo = new BorrarImportacionModels();
            modelo.TipoBorrado = (EnumTipoBorradoImportacion)tipoBorrado;

            return View("Index", modelo);
        }

        public ActionResult BorrarImportacionFacturacion(int tipoBorrado)
        {
            BorrarImportacionModels modelo = new BorrarImportacionModels();
            modelo.TipoBorrado = (EnumTipoBorradoImportacion)tipoBorrado;

            return View("Index", modelo);
        }

        public ActionResult BorrarImportacionViaVerde(int tipoBorrado)
        {
            BorrarImportacionModels modelo = new BorrarImportacionModels();
            modelo.TipoBorrado = (EnumTipoBorradoImportacion)tipoBorrado;

            return View("Index", modelo);
        }

        public ActionResult BorrarImportacionCombustible(int tipoBorrado)
        {
            BorrarImportacionModels modelo = new BorrarImportacionModels();
            modelo.TipoBorrado = (EnumTipoBorradoImportacion)tipoBorrado;

            return View("Index", modelo);
        }

        #endregion

        #region Borrado

        public ActionResult BorrarImportacionArchivo(BorrarImportacionModels modelo)
        {
            string valorReturn = "OK";

            if (!new BorradoImportacionService().BorraDatosImportacion(modelo.TipoBorrado, modelo.NombreArchivoParaBorrar))
            {
                valorReturn = "ERROR";
            }

            return Json(valorReturn, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region carga archivos chosen

        public JsonResult GetNombreArchivoParaBorrar(EnumTipoBorradoImportacion tipoBorrado)
        {
            var seleccion = new BorradoImportacionService().GetArchivosBorradoChosen(tipoBorrado);

            return Json(seleccion, JsonRequestBehavior.AllowGet);
        }
        #endregion carga archivos chosen
    }
}