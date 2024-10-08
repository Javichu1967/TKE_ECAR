using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using System.IO;

namespace TK_ECAR.Application_Services
{
    public class DocumentacionService
    {
        /// <summary>
        /// Devuelve la documentación que tenga las categorías que se le pasan, ordenada según el campo ordenación de la categoría.
        /// </summary>
        /// <param name="Categorias"></param>
        /// <returns></returns>
        public List<DocumentacionModel> GetDocumentacionPorCategoria(List<int?> Categorias)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_DOCUMENTACIONSpecification spec = new T_G_DOCUMENTACIONSpecification();
                spec.ID_CATEGORIAIN = Categorias;
                T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification();
                specCategoria.BAJA = false;

                var listaDocumentos = (from documento in unitOfWork.RepositoryT_G_DOCUMENTACION.Where(spec)
                                      join
                                      categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria) on documento.ID_CATEGORIA equals categoria.ID_CATEGORIA
                                       orderby categoria.ORDENACION ascending, documento.FECHAALTA
                                       select new DocumentacionModel
                                      {
                                          ID_Documento = documento.ID_DOCUMENTO,
                                          ID_Categoria = documento.ID_CATEGORIA,
                                          Descripcion = documento.DESCRIPCION,
                                          Nombre = documento.NOMBRE,
                                          Documento = documento.FICHERO,
                                          Accion = EnumAccionEntity.SinAccion,
                                          TipoArchivo = documento.TIPO_ARCHIVO
                                       }).ToList();

                return listaDocumentos;
            }
        }

        public List<DocumentacionModel> GetDocumentacion(List<int?> Documentos)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_DOCUMENTACIONSpecification spec = new T_G_DOCUMENTACIONSpecification();
                spec.ID_DOCUMENTOIN = Documentos;
                T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification();
                specCategoria.BAJA = false;

                var listaDocumentos = (from documento in unitOfWork.RepositoryT_G_DOCUMENTACION.Where(spec)
                                       join
                                       categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria) on documento.ID_CATEGORIA equals categoria.ID_CATEGORIA
                                       select new DocumentacionModel
                                       {
                                           ID_Documento = documento.ID_DOCUMENTO,
                                           ID_Categoria = documento.ID_CATEGORIA,
                                           Descripcion = documento.DESCRIPCION,
                                           Nombre = documento.NOMBRE,
                                           Documento = documento.FICHERO,
                                           Accion = EnumAccionEntity.SinAccion,
                                           TipoArchivo = documento.TIPO_ARCHIVO,
                                           FileUploadDocumentacion_download = documento.FICHERO,
                                       }).ToList();

                return listaDocumentos;
            }
        }

        /// <summary>
        /// Devuelve un documento, según el parámetro que se le pasa
        /// </summary>
        /// <param name="Documento"></param>
        /// <returns></returns>
        public DocumentacionModel GetDocumentacion(int? numDocumento)
        {
            return GetDocumentacion(new List<int?> { numDocumento }).FirstOrDefault();
        }

        /// <summary>
        /// Devuelve toda la documentación ordenada según el campo ordenación de la categoría
        /// </summary>
        /// <returns></returns>
        public List<DocumentacionModel> GetAllDocumentacion()
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification();
            specCategoria.BAJA = false;
            using (var unitOfWork = new UnitOfWork())
            {
                var listaDocumentos = (from categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                       join
                                       documento in unitOfWork.RepositoryT_G_DOCUMENTACION.Fetch() on categoria.ID_CATEGORIA equals documento.ID_CATEGORIA
                                       orderby categoria.ORDENACION ascending
                                       select new DocumentacionModel
                                       {
                                           ID_Documento = documento.ID_DOCUMENTO,
                                           ID_Categoria = documento.ID_CATEGORIA,
                                           Descripcion = documento.DESCRIPCION,
                                           Nombre = documento.NOMBRE,
                                           Documento = documento.FICHERO,
                                           Accion = EnumAccionEntity.SinAccion,
                                           TipoArchivo = documento.TIPO_ARCHIVO,
                                           FileUploadDocumentacion_download = documento.FICHERO,
                                       }).ToList();

                return listaDocumentos;
            }
        }

        /// <summary>
        /// Devuelve toda la documentación ordenada según el campo ordenación de la categoría, para cargar el DataTable.
        /// </summary>
        /// <returns></returns>
        public List<DocumentacionDataTableModel> AllDocumentacionDataTable()
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification();
            specCategoria.BAJA = false;
            using (var unitOfWork = new UnitOfWork())
            {
                var listaDocumentos = (from categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                       join
                                       documento in unitOfWork.RepositoryT_G_DOCUMENTACION.Fetch() on categoria.ID_CATEGORIA equals documento.ID_CATEGORIA
                                       orderby categoria.ORDENACION ascending
                                       select new DocumentacionDataTableModel
                                       {
                                           expnad = "",
                                           Ordenacion = categoria.ORDENACION,
                                           ID_Documento = documento.ID_DOCUMENTO,
                                           ID_Categoria = documento.ID_CATEGORIA,
                                           Descripcion = documento.DESCRIPCION,
                                           Nombre = documento.NOMBRE,
                                           Documento = documento.FICHERO,
                                           DescCategoria = categoria.NOMBRE,
                                           AccionDatatable = "",
                                           Accion = EnumAccionEntity.SinAccion,
                                           TipoArchivo = documento.TIPO_ARCHIVO
                                       }).ToList();

                return listaDocumentos;
            }
        }

        public List<CategoriasModel> AllCategorias()
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification();
            specCategoria.BAJA = false;
            using (var unitOfWork = new UnitOfWork())
            {

                var categorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                    select new CategoriasModel
                                    {
                                        ID_Categoria = categoria.ID_CATEGORIA,
                                        Nombre = categoria.NOMBRE,
                                        Ordenacion = categoria.ORDENACION,
                                        Accion = EnumAccionEntity.SinAccion
                                    }).OrderBy(o => o.Ordenacion).ToList();

                return categorias;
            }
        }


        #region Mantenimiento
        public void SaveDocumentacion(DocumentacionModel modelo)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var documento = new T_G_DOCUMENTACION
                {
                    ID_CATEGORIA = modelo.ID_Categoria,
                    DESCRIPCION = modelo.Descripcion,
                    NOMBRE = modelo.Nombre,
                    FECHAALTA = DateTime.Now
                };
                if (modelo.FileUploadDocumentacion != null)
                {
                    documento.TIPO_ARCHIVO = modelo.FileUploadDocumentacion.ContentType == null ? "UNKNOWN" : modelo.FileUploadDocumentacion.ContentType;
                    if (modelo.FileUploadDocumentacion.FileName != "")
                    {
                        documento.FICHERO = FileUtilities.GetFileName(modelo.FileUploadDocumentacion); //modelo.FileUpload.FileName;
                        BorraDocumentoFisico(Global.PathToUploadDocument + documento.FICHERO);
                    }
                }
                else
                {
                    documento.FICHERO = modelo.Documento;
                }

                if (modelo.Accion == EnumAccionEntity.Modificacion)
                {
                    documento.ID_DOCUMENTO = modelo.ID_Documento;
                    unitOfWork.RepositoryT_G_DOCUMENTACION.Update(documento);
                }
                else
                {
                    unitOfWork.RepositoryT_G_DOCUMENTACION.Insert(documento);
                }

                unitOfWork.Commit();
            }

            //FileUtilities utilityFile = new FileUtilities();

            if (modelo.FileUploadDocumentacion != null)
            {
                FileUtilities.UploadFile(modelo.FileUploadDocumentacion, Global.PathToUploadDocument);
            }

        }

        public void DeleteDocumentacion(int idDocumento)
        {
            T_G_DOCUMENTACIONSpecification spec = new T_G_DOCUMENTACIONSpecification();
            spec.ID_DOCUMENTO = idDocumento;

            string nombreArchivo = new UnitOfWork().RepositoryT_G_DOCUMENTACION.Where(spec).FirstOrDefault().FICHERO;

            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.RepositoryT_G_DOCUMENTACION.Delete(idDocumento);

                unitOfWork.Commit();

            }

            BorraDocumentoFisico(nombreArchivo);

        }
        #endregion


        #region métodos privados
        private void BorraDocumentoFisico(int idDocumento)
        {
            T_G_DOCUMENTACIONSpecification spec = new T_G_DOCUMENTACIONSpecification();
            spec.ID_DOCUMENTO = idDocumento;
            string nombreArchivo = Global.PathToUploadDocument + new UnitOfWork().RepositoryT_G_DOCUMENTACION.Where(spec).FirstOrDefault().FICHERO;
            BorraDocumentoFisico(nombreArchivo);
        }
        private void BorraDocumentoFisico(string Archivo)
        {

            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(Archivo)))
            {
                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(Archivo));
            }

        }
        #endregion


        /// <summary>
        /// Devuelve si el documento ya existe en BBDD para otro ID_DOCUMENTO 
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="nombreDocumento"></param>
        /// <returns></returns>
        public bool ExisteDocumentoToUploadEnBBDD(int idDocumento, string nombreDocumento)
        {
            T_G_DOCUMENTACIONSpecification spec = new T_G_DOCUMENTACIONSpecification();
            spec.FICHERO = nombreDocumento;

            T_G_DOCUMENTACION documento = new UnitOfWork().RepositoryT_G_DOCUMENTACION.Where(spec).Where(o=>o.ID_DOCUMENTO != idDocumento).FirstOrDefault();

            return (documento != null);
        }
    }
}