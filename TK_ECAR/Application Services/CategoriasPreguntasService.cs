using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;

namespace TK_ECAR.Application_Services
{
    public class CategoriasPreguntasService
    {
        /// <summary>
        /// Devuelve las categorías, según las categorías que se le pasen
        /// </summary>
        /// <param name="Categorias"></param>
        /// <returns></returns>
        public List<CategoriasPreguntasModel> GetCategoriaPregunta(List<int?> Categorias)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_M_CATEGORIA_PREGUNTASSpecification spec = new T_M_CATEGORIA_PREGUNTASSpecification();
                spec.ID_CATEGORIAIN = Categorias;

                var listaCategorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(spec)
                                       orderby categoria.ORDENACION ascending
                                       select new CategoriasPreguntasModel
                                       {
                                          ID_Categoria = categoria.ID_CATEGORIA,
                                          Nombre = categoria.NOMBRE,
                                          Ordenacion = categoria.ORDENACION,
                                          ID_Empresa = categoria.ID_EMPRESA,
                                      }).ToList();

                return listaCategorias;
            }
        }

        /// <summary>
        /// Devuelve un documento, según el parámetro que se le pasa
        /// </summary>
        /// <param name="Documento"></param>
        /// <returns></returns>
        public CategoriasPreguntasModel GetCategoriaPregunta(int? numCategoria)
        {
            return GetCategoriaPregunta(new List<int?> { numCategoria }).FirstOrDefault();
        }

        /// <summary>
        /// Devuelve toda la documentación ordenada según el campo ordenación de la categoría.
        /// </summary>
        /// <returns></returns>
        public List<CategoriasPreguntasModel> AllCategoriasPreguntas(List<int> empresas = null)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification
            {
                BAJA = false
            };

            if (empresas != null)
            {
                specCategoria.ID_EMPRESAIN = empresas.Select(x => (int?)x).ToList();
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var listaCategorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria)
                                       orderby categoria.ORDENACION ascending
                                       select new CategoriasPreguntasModel
                                       {
                                           ID_Categoria = categoria.ID_CATEGORIA,
                                           Nombre = categoria.NOMBRE,
                                           Ordenacion = categoria.ORDENACION,
                                           ID_Empresa = categoria.ID_EMPRESA,
                                           Accion = EnumAccionEntity.SinAccion
                                       }).ToList();

                return listaCategorias;
            }
        }

        public List<CategoriasPreguntasDataTableModel> GetCategoriasPreguntasDatatable(List<int> empresas = null)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification
            {
                BAJA = false
            };
            SAPHR_EmpresasSpecification especEmp = new SAPHR_EmpresasSpecification
            {
                Activo = true
            };

            if (empresas != null)
            {
                specCategoria.ID_EMPRESAIN = empresas.Select(x=> (int?)x).ToList();
                especEmp.CodigoEmpresaIN = empresas.Select(x => (int?)x).ToList();
            }

            using (var unitOfWork = new UnitOfWork())
            {

                var listaCategorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria)
                                       orderby categoria.ORDENACION ascending
                                       select new CategoriasPreguntasDataTableModel
                                       {
                                           ID_Categoria = categoria.ID_CATEGORIA,
                                           Nombre = categoria.NOMBRE,
                                           Ordenacion = categoria.ORDENACION,
                                           ID_Empresa = categoria.ID_EMPRESA,
                                           DescEmpresa = "",
                                           Accion = EnumAccionEntity.SinAccion,
                                           AccionDatatable = ""
                                       }).ToList();

                foreach(CategoriasPreguntasDataTableModel categoria in listaCategorias)
                {
                    categoria.DescEmpresa = unitOfWork.RepositorySAPHR_Empresas.Fetch().Where(o => o.CodigoEmpresa == categoria.ID_Empresa).FirstOrDefault().Nombre;
                }

                return listaCategorias;
            }
        }


        public List<string> OrdenacionCategoriasPreguntas(EnumAccionEntity accion)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification
            {
                BAJA = false
            };

            using (var unitOfWork = new UnitOfWork())
            {
                List<string> listaOrdenacion = new List<string>();

                for (int i = 0; i <= unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria).Count() - 1; i++)
                {
                    listaOrdenacion.Add((i + 1).ToString());
                }
                if (accion == EnumAccionEntity.Alta)
                {
                    listaOrdenacion.Add("Ultimo");
                }

                return listaOrdenacion;
            }
        }

        public void SaveCategoriasPreguntas(CategoriasPreguntasModel modelo)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification
            {
                BAJA = false
            };

            if (modelo.Accion == EnumAccionEntity.Modificacion)
            {
                T_M_CATEGORIA_PREGUNTASSpecification spec = new T_M_CATEGORIA_PREGUNTASSpecification
                {
                    ID_CATEGORIA = modelo.ID_Categoria
                };

                UnitOfWork unitOW = new UnitOfWork();

                int numOrdenacion = unitOW.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(spec).FirstOrDefault().ORDENACION;
                if (modelo.Ordenacion != numOrdenacion)
                {
                    if (modelo.Ordenacion <= unitOW.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria).Count())
                    {
                        ReOrdenaCategorias(numOrdenacion, modelo.Ordenacion);
                    }
                    else
                    {
                        modelo.Ordenacion = numOrdenacion;
                    }
                }
            }
            else
            { 
                ReOrdenaCategorias(new UnitOfWork().RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria).ToList().Count() + 1, modelo.Ordenacion);
            }

            using (var unitOfWork = new UnitOfWork())
            {
                    var categoria = new T_M_CATEGORIA_PREGUNTAS
                {
                    NOMBRE = modelo.Nombre,
                    ORDENACION = modelo.Ordenacion,
                    ID_EMPRESA = modelo.ID_Empresa,
                    BAJA = false,
                };

                if (modelo.Accion == EnumAccionEntity.Modificacion)
                {
                    categoria.ID_CATEGORIA = modelo.ID_Categoria;
                    unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Update(categoria);
                }
                else
                {
                    unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Insert(categoria);
                }



                unitOfWork.Commit();
            }
        }

        private void ReOrdenaCategorias(int numOrdenAnterior, int numOrdenNuevo)
        {
            T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification
            {
                BAJA = false
            };

            using (var unitOfWork = new UnitOfWork())
            {
                List<T_M_CATEGORIA_PREGUNTAS> categorias = new List<T_M_CATEGORIA_PREGUNTAS>();

                if (numOrdenNuevo == 0)
                {
                    int numorden = 1;
                    categorias = unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria)
                                    .OrderBy(o => o.ORDENACION).ToList();
                    foreach (T_M_CATEGORIA_PREGUNTAS categoria in categorias)
                    {
                        categoria.ORDENACION = numorden;
                        unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Update(categoria);
                        numorden++;
                    }
                }
                else
                {
                    if (numOrdenAnterior < numOrdenNuevo)
                    {
                        categorias = unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria)
                                        .Where(o => o.ORDENACION > numOrdenAnterior && o.ORDENACION <= numOrdenNuevo)
                                        .OrderBy(o => o.ORDENACION).ToList();
                        foreach (T_M_CATEGORIA_PREGUNTAS categoria in categorias)
                        {
                            categoria.ORDENACION = categoria.ORDENACION - 1;
                            unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Update(categoria);
                        }
                    }
                    else if (numOrdenAnterior > numOrdenNuevo)
                    {
                        categorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria)
                                      where categoria.ORDENACION >= numOrdenNuevo && categoria.ORDENACION < numOrdenAnterior
                                      select categoria).OrderBy(o => o.ORDENACION).ToList();
                        foreach (T_M_CATEGORIA_PREGUNTAS categoria in categorias)
                        {
                            categoria.ORDENACION = categoria.ORDENACION + 1;
                            unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Update(categoria);
                        }
                    }
                }
                unitOfWork.Commit();
            }
        }

        public void BorrarCategoria(int idCategoria)
        {
            T_M_CATEGORIA_PREGUNTASSpecification spec = new T_M_CATEGORIA_PREGUNTASSpecification
            {
                ID_CATEGORIA = idCategoria
            };

            using (var scope = new TransactionScope())
            {
                using (var unitOfWork = new UnitOfWork())
                {

                    T_M_CATEGORIA_PREGUNTAS categoria = unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Where(spec).FirstOrDefault();

                    if (categoria != null)
                    {
                        categoria.BAJA = true;
                    }

                    unitOfWork.RepositoryT_M_CATEGORIA_PREGUNTAS.Update(categoria);

                    unitOfWork.Commit();

                    T_G_PREGUNTAS_FRECUENTESSpecification specPregunta = new T_G_PREGUNTAS_FRECUENTESSpecification
                    {
                        BAJA = false,
                        ID_CATEGORIA = idCategoria
                    };
                    List<T_G_PREGUNTAS_FRECUENTES> preguntas = unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Where(specPregunta).ToList();
                    foreach(T_G_PREGUNTAS_FRECUENTES pregunta in preguntas)
                    {
                        pregunta.BAJA = true;
                        unitOfWork.RepositoryT_G_PREGUNTAS_FRECUENTES.Update(pregunta);

                        unitOfWork.Commit();
                    }
                }

                scope.Complete();
            }

            using (var unitOfWork = new UnitOfWork())
            {
                T_M_CATEGORIA_PREGUNTASSpecification specCategoria = new T_M_CATEGORIA_PREGUNTASSpecification
                {
                    BAJA = false
                };
                ReOrdenaCategorias(new UnitOfWork().RepositoryT_M_CATEGORIA_PREGUNTAS.Where(specCategoria).ToList().Count() + 1, 0);
            }
        }
    }
}