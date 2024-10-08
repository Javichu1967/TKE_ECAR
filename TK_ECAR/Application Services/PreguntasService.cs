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

namespace TK_ECAR.Application_Services
{
    public class PreguntasService
    {
        /// <summary>
        /// Devuelve las preguntas frecuentes según las categorías que se le pasen
        /// </summary>
        /// <param name="Categorias"></param>
        /// <returns></returns>
        public List<PreguntasDataTableModel> GetPreguntasDatatable(List<int?> Categorias, bool mirarBaja = true)
        {
            T_G_PREGUNTAS_FRECUENTESSpecification spec = new T_G_PREGUNTAS_FRECUENTESSpecification();
            spec.ID_CATEGORIAIN = Categorias;
            return GetPreguntasdataTable(spec);
        }

        /// <summary>
        /// Devuelve todas las preguntas frecuentes
        /// </summary>
        /// <returns></returns>
        public List<PreguntasDataTableModel> GetPreguntasDatatable(List<int> empresas, bool mirarBaja = true)
        {
            T_G_PREGUNTAS_FRECUENTESSpecification spec = new T_G_PREGUNTAS_FRECUENTESSpecification();
            return GetPreguntasdataTable(spec, empresas);
        }


        private List<PreguntasDataTableModel> GetPreguntasdataTable(T_G_PREGUNTAS_FRECUENTESSpecification spec, List<int> empresas = null, bool mirarBaja = true)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification();
            specCategoria.BAJA = false;

            if (empresas != null)
            {
                specCategoria.ID_EMPRESAIN = empresas.Select(x => (int?)x);

            }

            if (mirarBaja)
            {
                spec.BAJA = false;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var listaPreguntas = (from pregunta in unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Where(spec)
                                         join
                                         categoria in unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria) on pregunta.ID_CATEGORIA equals categoria.ID_CATEGORIA
                                         select new PreguntasDataTableModel
                                         {
                                             expnad = "",
                                             idCategoria = pregunta.ID_CATEGORIA,
                                             idPregunta = pregunta.ID_PREGUNTA,
                                             Pregunta = pregunta.PREGUNTA,
                                             Respuesta = pregunta.RESPUESTA,
                                             Categoria = categoria.NOMBRE,
                                             ID_Empresa = categoria.ID_EMPRESA,
                                             DescEmpresa = "",
                                             AccionDatatable = "",
                                             numOrdenCategoria = categoria.ORDENACION,
                                         }).OrderBy(o => o.numOrdenCategoria).ThenBy(o=>o.idPregunta).ToList();


                foreach (PreguntasDataTableModel pregunta in listaPreguntas)
                {
                    pregunta.DescEmpresa = unitOfWork.RepositorySAPHR_Empresas.Fetch().Where(o => o.CodigoEmpresa == pregunta.ID_Empresa).FirstOrDefault().Nombre;
                }

                return listaPreguntas;
            }
        }

        //public PreguntasModel GetRespuesta(int idPregunta)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {
        //        T_G_PREGUNTAS_FRECUENTESSpecification spec = new T_G_PREGUNTAS_FRECUENTESSpecification();
        //        spec.ID_PREGUNTA = idPregunta;

        //        var respuesta = (from pregunta in unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Where(spec)
        //                                join
        //                                categoria in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Fetch() on pregunta.ID_CATEGORIA equals categoria.ID_TIPO_ALERTA
        //                                select new PreguntasModel
        //                                {
        //                                    expnad = "",
        //                                    idCategoria = pregunta.ID_CATEGORIA,
        //                                    idPregunta = pregunta.ID_PREGUNTA,
        //                                    Pregunta = pregunta.PREGUNTA,
        //                                    Respuesta = pregunta.RESPUESTA,
        //                                    Categoria = categoria.DESCRIPCION,
        //                                    Accion = ""
        //                                }).FirstOrDefault();

        //        return respuesta;
        //    }
        //}


        public List<CategoriaPregunta> AllTipoAlertas()
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var tiposAlertas = (from tipoAlerta in unitOfWork.RepositoryT_M_TIPOS_ALERTAS.Fetch()
                               select new CategoriaPregunta
                               {
                                   idCategoria = tipoAlerta.ID_TIPO_ALERTA,
                                   Descripcion = tipoAlerta.DESCRIPCION
                               }).OrderBy(o=>o.Descripcion).ToList();



                return tiposAlertas;
            }
        }

        public List<CategoriaPregunta> AllTipoCategoriaPreguntas(List<int> empresas)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification();
            specCategoria.BAJA = false;
            specCategoria.ID_EMPRESAIN = empresas.Select(x => (int?)x);

            using (var unitOfWork = new UnitOfWork())
            {

                var categorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria)
                                    select new CategoriaPregunta
                                    {
                                        idCategoria = categoria.ID_CATEGORIA,
                                        Descripcion = categoria.NOMBRE,
                                        numOrdenCategoria = categoria.ORDENACION,
                                    }).OrderBy(o => o.numOrdenCategoria).ToList();



                return categorias;
            }
        }


        public List<PreguntasModel> GetPreguntasByCategoria(List<int> categorias, List<int> empresas = null)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification();
            specCategoria.BAJA = false;
            if (empresas != null)
            {
                specCategoria.ID_EMPRESAIN = empresas.Select(x => (int?)x);
            }

            specCategoria.ID_CATEGORIAIN = categorias.Select(x => (int?)x);

            T_G_PREGUNTAS_FRECUENTESSpecification spec = new T_G_PREGUNTAS_FRECUENTESSpecification();
            spec.BAJA = false;


            using (var unitOfWork = new UnitOfWork())
            {
                var preguntas = (from categoria in unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria)
                                join pregunta in unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Where(spec) on categoria.ID_CATEGORIA equals pregunta.ID_CATEGORIA
                                select new PreguntasModel
                                {
                                    idCategoria = pregunta.ID_CATEGORIA,
                                    idPregunta = pregunta.ID_PREGUNTA,
                                    Pregunta = pregunta.PREGUNTA,
                                    Respuesta = pregunta.RESPUESTA,
                                    Accion = EnumAccionEntity.SinAccion
                                }).OrderBy(o => o.idCategoria).ThenBy(x => x.Pregunta).ToList();

                return preguntas;
            }
        }



        public PreguntasModel GetPregunta(int idPregunta)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_PREGUNTAS_FRECUENTESSpecification spec = new T_G_PREGUNTAS_FRECUENTESSpecification();
                spec.ID_PREGUNTA = idPregunta;

                var pregunta = (from _pregunta in unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Where(spec)
                                select new PreguntasModel
                                {
                                    idCategoria = _pregunta.ID_CATEGORIA,
                                    idPregunta = _pregunta.ID_PREGUNTA,
                                    Pregunta = _pregunta.PREGUNTA,
                                    Respuesta = _pregunta.RESPUESTA,
                                    Accion = EnumAccionEntity.SinAccion
                                }).FirstOrDefault();

                return pregunta;
            }
        }


        public void SavePregunta(PreguntasModel modelo)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var pregunta = new T_G_PREGUNTAS_FRECUENTES
                {
                    ID_CATEGORIA = modelo.idCategoria,
                    PREGUNTA = modelo.Pregunta,
                    RESPUESTA = modelo.Respuesta
                };

                if (modelo.Accion == EnumAccionEntity.Modificacion)
                {
                    pregunta.ID_PREGUNTA = modelo.idPregunta;
                    unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Update(pregunta);
                }
                else
                {
                    unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Insert(pregunta);
                }



                unitOfWork.Commit();
            }
        }

        public void DeletePregunta(int idPregunta)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_PREGUNTAS_FRECUENTES pregunta = unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Fetch().Where(x => x.ID_PREGUNTA == idPregunta).FirstOrDefault();

                pregunta.BAJA = true;

                unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Update(pregunta);

                unitOfWork.Commit();
            }
        }
    }
}