using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;
using HelperExtensionsNameSpace;
using Newtonsoft.Json;

namespace TK_ECAR.Controllers
{
    public class PreguntasFrecuentesController : BaseController
    {
        public ActionResult Index()
        {
            List<int> empresas = new UsersService().GetEmpresasUsuario().ToList();
            var preguntas = new PreguntasService().GetPreguntasDatatable(empresas);

            return View(preguntas);
            //return View();
        }

        public ActionResult VerPreguntasFrecuentes()
        {
            return View();
        }
        


        public ActionResult NuevaPregunta()
        {
            PreguntasModel pregunta = new PreguntasModel();
            pregunta.Accion = Framework.EnumAccionEntity.Alta;
            return PartialView("_MantenimientoPregunta", pregunta);
        }

        public ActionResult BorrarPregunta(int idPregunta)
        {

            PreguntasService servicePreguntas = new PreguntasService();
            var pregunta = servicePreguntas.GetPregunta(idPregunta);
            pregunta.Accion = Framework.EnumAccionEntity.Baja;

            return PartialView("_BorrarPregunta", pregunta);
        }

        public ActionResult EditarPregunta(int idPregunta)
        {

            PreguntasService servicePreguntas = new PreguntasService();
            var pregunta = servicePreguntas.GetPregunta(idPregunta);
            pregunta.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoPregunta", pregunta);
        }


        [ActionName("GuardarPregunta")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarPregunta(PreguntasModel modelo)
        {
            PreguntasService servicePreguntas = new PreguntasService();

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                servicePreguntas.SavePregunta(modelo);
            }
            else if (modelo.Accion == Framework.EnumAccionEntity.Baja)
            {
                servicePreguntas.DeletePregunta(modelo.idPregunta);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        
        #region CargaDatatable
        public ActionResult PreguntasJson()
        {
            List<int> empresas = new UsersService().GetEmpresasUsuario().ToList();
            var preguntas = new PreguntasService().GetPreguntasDatatable(empresas);

            var data = new
            {
                data = preguntas,
                draw = 1,
                recordsFiltered = preguntas.Count,
                recordsTotal = preguntas.Count
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TreeView
        [HttpPost]
        public string GetJsonDataTreeviewPreguntas()
        {
            string serializeJsonData = string.Empty;

            List<int> empresas = new UsersService().GetEmpresasUsuario().ToList();
            var categorias = new PreguntasService().AllTipoCategoriaPreguntas(empresas);
            

            int totalCategorias = categorias.Count();


            for (int i = 0; i < totalCategorias; i++)
            {

                var preguntas = new PreguntasService().GetPreguntasDatatable(new List<int?> { categorias[i].idCategoria });

                int totalPreguntas = preguntas.Count();
                if (totalPreguntas == 0)
                {
                    ParentWithNoChild single = new ParentWithNoChild();
                    single.text = categorias[i].Descripcion;
                    single.id = categorias[i].idCategoria;
                    serializeJsonData = serializeJsonData.MergeJsonString(JsonConvert.SerializeObject(single));
                    //Response.Write("<br/></br> serializeJsonData: " + serializeJsonData);
                }
                else if (totalPreguntas >= 1)
                {
                    ParentWithChildren parentCategoria = new ParentWithChildren();
                    Child[] childrenCategoria = new Child[totalPreguntas];

                    parentCategoria.text = categorias[i].Descripcion;
                    parentCategoria.id = categorias[i].idCategoria;
                    for (int j = 0; j < totalPreguntas; j++)
                    {
                        if (!String.IsNullOrEmpty(preguntas[j].Pregunta))
                        {
                            Child child = new Child { text = preguntas[j].Pregunta, id = preguntas[j].idPregunta };
                            Child[] childrenPregunta = new Child[1];
                            Child childPregunta = new Child { text = preguntas[j].Respuesta };
                            childrenPregunta[0] = childPregunta;
                            child.nodes = childrenPregunta;
                            childrenCategoria[j] = child;
                        }
                    }
                    parentCategoria.nodes = childrenCategoria;
                    serializeJsonData = serializeJsonData.MergeJsonString(JsonConvert.SerializeObject(parentCategoria));
                }
            }
            return serializeJsonData.GetTreeViewJsonFormat();
        }
        #endregion

    }
}