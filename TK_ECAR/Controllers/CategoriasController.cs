using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;

namespace TK_ECAR.Controllers
{
    public class CategoriasController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NuevaCategoria()
        {
            CategoriasModel categorias = new CategoriasModel();
            categorias.Accion = Framework.EnumAccionEntity.Alta;
            categorias.Ordenacion = new CategoriasService().OrdenacionCategorias(Framework.EnumAccionEntity.Alta).Count();
            return PartialView("_MantenimientoCategorias", categorias);
        }

        public ActionResult EditarCategorias(int idCategoria)
        {

            CategoriasService serviceCategorias = new CategoriasService();
            var categoria = serviceCategorias.GetCategoria(idCategoria);
            categoria.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoCategorias", categoria);
        }


        public ActionResult BorraCategoriaDocumento(int idCategoria)
        {

            CategoriasService serviceCategorias = new CategoriasService();
            serviceCategorias.BorrarCategoria(idCategoria);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        

        [ActionName("GuardarCategorias")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarCategorias(CategoriasModel modelo)
        {
            CategoriasService serviceCategorias = new CategoriasService();

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                serviceCategorias.SaveCategorias(modelo);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDocumentosByCategoriaJson(int categoria)
        {
            List<int?> categorias = new List<int?> { categoria };

            var documentos = new DocumentacionService().GetDocumentacionPorCategoria(categorias);

            var data = new
            {
                data = documentos,
                draw = 1,
                recordsFiltered = documentos.Count,
                recordsTotal = documentos.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #region CargaDatatable
        public ActionResult CategoriasJson()
        {
            var categorias = new CategoriasService().GetCategoriasDatatable();

            var data = new
            {
                data = categorias,
                draw = 1,
                recordsFiltered = categorias.Count,
                recordsTotal = categorias.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion  
    }
}