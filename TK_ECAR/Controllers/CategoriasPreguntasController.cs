using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;

namespace TK_ECAR.Controllers
{
    public class CategoriasPreguntasController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NuevaCategoriaPreguntas()
        {
            CategoriasPreguntasModel categorias = new CategoriasPreguntasModel();
            categorias.Accion = Framework.EnumAccionEntity.Alta;
            categorias.Ordenacion = new CategoriasPreguntasService().OrdenacionCategoriasPreguntas(Framework.EnumAccionEntity.Alta).Count();
            return PartialView("_MantenimientoCategoriasPreguntas", categorias);
        }

        public ActionResult EditarCategoriaPreguntas(int idCategoria)
        {

            CategoriasPreguntasService serviceCategorias = new CategoriasPreguntasService();
            var categoria = serviceCategorias.GetCategoriaPregunta((int?)idCategoria);
            categoria.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoCategoriasPreguntas", categoria);
        }

        public ActionResult BorraCategoriaPregunta(int idCategoria)
        {

            CategoriasPreguntasService serviceCategorias = new CategoriasPreguntasService();
            serviceCategorias.BorrarCategoria(idCategoria);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPreguntasByCategoriaJson(int categoriaPregunta)
        {
            List<int> categorias = new List<int> { categoriaPregunta };

            var preguntas = new PreguntasService().GetPreguntasByCategoria(categorias, null);

            var data = new
            {
                data = preguntas,
                draw = 1,
                recordsFiltered = preguntas.Count,
                recordsTotal = preguntas.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }



        [ActionName("GuardarCategoriasPreguntas")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarCategoriaPreguntas(CategoriasPreguntasModel modelo)
        {
            CategoriasPreguntasService serviceCategorias = new CategoriasPreguntasService();

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                serviceCategorias.SaveCategoriasPreguntas(modelo);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        #region CargaDatatable
        public ActionResult CategoriasPreguntasJson()
        {
            var categorias = new CategoriasPreguntasService().GetCategoriasPreguntasDatatable(UserModel.Empresas.ToList());

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