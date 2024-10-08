using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Application_Services;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using HelperExtensionsNameSpace;
using Newtonsoft.Json;

namespace TK_ECAR.Controllers
{
    public class DocumentosController : BaseController
    {
        public ActionResult Index()
        {
            var documentos = new DocumentacionService().AllDocumentacionDataTable();

            return View(documentos);
        }

        public ActionResult VerDocumentacion()
        {
            return View();
        }

        public ActionResult NuevoDocumento()
        {
            DocumentacionModel documentacion = new DocumentacionModel();
            documentacion.Accion = Framework.EnumAccionEntity.Alta;
            return PartialView("_MantenimientoDocumentacion", documentacion);
        }

        public ActionResult BorrarDocumento(int idDocumentacion)
        {

            DocumentacionService serviceDocumentacion = new DocumentacionService();
            var documento = serviceDocumentacion.GetDocumentacion(idDocumentacion);
            documento.Accion = Framework.EnumAccionEntity.Baja;

            return PartialView("_BorrarDocumentacion", documento);
        }

        public ActionResult EditarDocumento(int idDocumentacion)
        {

            DocumentacionService serviceDocumentacion = new DocumentacionService();
            var documento = serviceDocumentacion.GetDocumentacion(idDocumentacion);
            documento.Accion = Framework.EnumAccionEntity.Modificacion;

            return PartialView("_MantenimientoDocumentacion", documento);
        }


        [ActionName("GuardarDocumentacion")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarDocumentacion(DocumentacionModel modelo)
        {
            DocumentacionService serviceDocumentacion = new DocumentacionService();

            if (modelo.Accion == Framework.EnumAccionEntity.Alta || modelo.Accion == Framework.EnumAccionEntity.Modificacion)
            {
                serviceDocumentacion.SaveDocumentacion(modelo);
            }
            else if (modelo.Accion == Framework.EnumAccionEntity.Baja)
            {
                serviceDocumentacion.DeleteDocumentacion(modelo.ID_Documento);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        #region TreeView
        public string GetJsonDataTreeviewDocumentacion()
        {
            string serializeJsonData = string.Empty;

            var categorias = new DocumentacionService().AllCategorias();

            int totalCategorias = categorias.Count();

            for (int i = 0; i < totalCategorias; i++)
            {

                var documentos = new DocumentacionService().GetDocumentacionPorCategoria(new List<int?> { categorias[i].ID_Categoria });

                int totalDocumentos = documentos.Count();
                if (totalDocumentos == 0)
                {
                    ParentWithNoChild single = new ParentWithNoChild();
                    single.text = categorias[i].Nombre;
                    single.id = categorias[i].ID_Categoria;
                    serializeJsonData = serializeJsonData.MergeJsonString(JsonConvert.SerializeObject(single));
                }
                else if (totalDocumentos >= 1)
                {
                    ParentWithChildren parentCategoria = new ParentWithChildren();
                    Child[] childrenCategoria = new Child[totalDocumentos];

                    parentCategoria.text = categorias[i].Nombre;
                    parentCategoria.id = categorias[i].ID_Categoria;
                    for (int j = 0; j < totalDocumentos; j++)
                    {
                        if (!String.IsNullOrEmpty(documentos[j].Nombre))
                        {
                            Child child = new Child { text = documentos[j].Nombre, id = documentos[j].ID_Documento };
                            Child[] childrenDocumento = new Child[1];
                            Child childDocumento = new Child { text = "Descripción: " + (string.IsNullOrEmpty(documentos[j].Descripcion) ? "(Sin descripción)" : documentos[j].Descripcion) };
                            childrenDocumento[0] = childDocumento;
                            child.nodes = childrenDocumento;
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


        public ActionResult CompruebaArchivo(string nombreArchivo, int idDocumento, Framework.EnumAccionEntity accion)
        {
            bool valorReturn = false;
            DocumentacionService serviceDocumentacion = new DocumentacionService();

            if (accion == Framework.EnumAccionEntity.Alta)
            {
                valorReturn = FileUtilities.ExisteDocumentoToUploadEnDisco(Global.PathToUploadDocument + nombreArchivo);
            }
            else
            {
                valorReturn = serviceDocumentacion.ExisteDocumentoToUploadEnBBDD(idDocumento, nombreArchivo);
            }

            //System.IO.File.Exists(Global.PathToUploadDocument + nombreArchivo)
            
            return Json(valorReturn, JsonRequestBehavior.AllowGet);
        }
        

        public FileResult VerDocumento(int idDocumento)
        {
            var documentos = new DocumentacionService().GetDocumentacion(idDocumento);

            //byte[] fileBytes = System.IO.File.ReadAllBytes(@documentos.Ruta);

            var documento = Global.PathToUploadDocument + documentos.Documento;
            return File(System.Web.HttpContext.Current.Server.MapPath(documento), System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(System.Web.HttpContext.Current.Server.MapPath(documento)));
        }

    }
}