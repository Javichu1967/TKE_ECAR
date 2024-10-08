using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;
using HelperExtensionsNameSpace;
using Newtonsoft.Json;
using System.IO;
using TK_ECAR.Utils;

namespace TK_ECAR.Controllers
{
    public class GestoresFlotaController : BaseController
    {
        public ActionResult Index()
        {
            var gestores = new GestoresFlotaService().GetAllGestoresFlota();

            return View(gestores);
        }

        public ActionResult NuevoGestorFlota()
        {
            GestoresFlotaModel gestor = new GestoresFlotaModel();
            gestor.Accion = Framework.EnumAccionEntity.Alta;
            return PartialView("_MantenimientoGestorFlota", gestor);
        }

        public ActionResult BorrarGestorFlota(int numEmpleado)
        {

            GestoresFlotaService serviceGestorFlota = new GestoresFlotaService();
            var gestor = serviceGestorFlota.GetGestorFlota(numEmpleado);
            gestor.Accion = Framework.EnumAccionEntity.Baja;

            return PartialView("_BorrarGestorFlota", gestor);
        }

        public ActionResult EditarGestorFlota(int numEmpleado)
        {

            GestoresFlotaService serviceGestorFlota = new GestoresFlotaService();
            var gestor = serviceGestorFlota.GetGestorFlota(numEmpleado);
            gestor.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoGestorFlota", gestor);
        }


        [ActionName("GuardarGestorFlota")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarGestorFlota(GestoresFlotaModel modelo)
        {
            GestoresFlotaService serviceGestorFlota = new GestoresFlotaService();

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                serviceGestorFlota.SaveGestorFlota(modelo);
            }
            else if (modelo.Accion == Framework.EnumAccionEntity.Baja)
            {
                serviceGestorFlota.DeleteGestorFlota(modelo.NumeroEmpleado);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetImgGestorFlota(int numEmpleado)
        {
            var archivoCarnet = string.Empty;

            List<FileInfo> file = new List<FileInfo>();

            FileStream fs = null;

            if (Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(Global.PathToUploadFotoGestoresFlota + numEmpleado.ToString() + "/")))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(Global.PathToUploadFotoGestoresFlota + numEmpleado.ToString() + "/"));

                file = di.GetFiles().ToList();

                if (file.Count() > 0)
                {
                    archivoCarnet = file[0].FullName;
                }
            }

            if (!string.IsNullOrEmpty(archivoCarnet))
            {
                try
                {
                    fs = new FileStream(archivoCarnet, FileMode.Open, FileAccess.Read);
                }

                catch
                {
                    fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/Application/sinImagen.jpg"), FileMode.Open, FileAccess.Read);
                }
            }
            else
            {
                fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/Application/sinImagen.jpg"), FileMode.Open, FileAccess.Read);
            }

            return File(fs, "image/jpeg");
        }


        #region TreeView
        public string GetJsonDataTreeviewGestoresFlota()
        {
            string serializeJsonData = string.Empty;

            var gestores = new GestoresFlotaService().GetAllGestoresFlota();

            int totalGestores = gestores.Count();

            for (int i = 0; i < totalGestores; i++)
            {

                //var documentos = new DocumentacionService().GetDocumentacionPorCategoria(new List<int?> { gestores[i].NumeroEmpleado });

                //int totalDocumentos = documentos.Count();
                //if (totalDocumentos == 0)
                //{
                    ParentWithNoChild single = new ParentWithNoChild();
                    single.text = gestores[i].Nombre;
                    single.id = gestores[i].NumeroEmpleado;
                    serializeJsonData = serializeJsonData.MergeJsonString(JsonConvert.SerializeObject(single));
                //}
                //else if (totalDocumentos >= 1)
                //{
                //    ParentWithChildren parentCategoria = new ParentWithChildren();
                //    Child[] childrenCategoria = new Child[totalDocumentos];

                //    parentCategoria.text = categorias[i].Nombre;
                //    parentCategoria.id = categorias[i].ID_Categoria;
                //    for (int j = 0; j < totalDocumentos; j++)
                //    {
                //        if (!String.IsNullOrEmpty(documentos[j].Nombre))
                //        {
                //            Child child = new Child { text = documentos[j].Nombre, id = documentos[j].ID_Documento };
                //            Child[] childrenDocumento = new Child[1];
                //            Child childDocumento = new Child { text = "Descripción: " + (string.IsNullOrEmpty(documentos[j].Descripcion) ? "(Sin descripción)" : documentos[j].Descripcion) };
                //            childrenDocumento[0] = childDocumento;
                //            child.nodes = childrenDocumento;
                //            childrenCategoria[j] = child;
                //        }
                //    }
                //    parentCategoria.nodes = childrenCategoria;
                //    serializeJsonData = serializeJsonData.MergeJsonString(JsonConvert.SerializeObject(parentCategoria));
                //}
            }
            return serializeJsonData.GetTreeViewJsonFormat();
        }
        #endregion


        
        public ActionResult VerGestorFlota()
        {
            var gestores = new GestoresFlotaService().GetAllGestoresFlota();

            return View(gestores);
        }
    }
}