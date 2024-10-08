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
    public class CategoriasService
    {
        /// <summary>
        /// Devuelve las categorías, según las categorías que se le pasen
        /// </summary>
        /// <param name="Categorias"></param>
        /// <returns></returns>
        public List<CategoriasModel> GetCategoria(List<int?> Categorias)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_M_CATEGORIASSpecification spec = new T_M_CATEGORIASSpecification();
                spec.ID_CATEGORIAIN = Categorias;

                var listaCategorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(spec)
                                       orderby categoria.ORDENACION ascending
                                       select new CategoriasModel
                                       {
                                          ID_Categoria = categoria.ID_CATEGORIA,
                                          Nombre = categoria.NOMBRE,
                                          Ordenacion = categoria.ORDENACION
                                      }).ToList();

                return listaCategorias;
            }
        }

        /// <summary>
        /// Devuelve un documento, según el parámetro que se le pasa
        /// </summary>
        /// <param name="Documento"></param>
        /// <returns></returns>
        public CategoriasModel GetCategoria(int? numCategoria)
        {
            return GetCategoria(new List<int?> { numCategoria }).FirstOrDefault();
        }

        /// <summary>
        /// Devuelve toda la documentación ordenada según el campo ordenación de la categoría.
        /// </summary>
        /// <returns></returns>
        public List<CategoriasModel> AllCategorias()
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification
            {
                BAJA = false
            };

            using (var unitOfWork = new UnitOfWork())
            {
                var listaCategorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                       orderby categoria.ORDENACION ascending
                                       select new CategoriasModel
                                       {
                                           ID_Categoria = categoria.ID_CATEGORIA,
                                           Nombre = categoria.NOMBRE,
                                           Ordenacion = categoria.ORDENACION,
                                           Accion = EnumAccionEntity.SinAccion
                                       }).ToList();

                return listaCategorias;
            }
        }

        public List<CategoriasDataTableModel> GetCategoriasDatatable()
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification
            {
                BAJA = false
            };

            using (var unitOfWork = new UnitOfWork())
            {
                var listaCategorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                       orderby categoria.ORDENACION ascending
                                       select new CategoriasDataTableModel
                                       {
                                           ID_Categoria = categoria.ID_CATEGORIA,
                                           Nombre = categoria.NOMBRE,
                                           Ordenacion = categoria.ORDENACION,
                                           Accion = EnumAccionEntity.SinAccion,
                                           AccionDatatable = ""
                                       }).ToList();

                return listaCategorias;
            }
        }


        public List<string> OrdenacionCategorias(EnumAccionEntity accion)
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification
            {
                BAJA = false
            };
            using (var unitOfWork = new UnitOfWork())
            {
                List<string> listaOrdenacion = new List<string>();

                for (int i = 0; i <= unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria).Count() - 1; i++)
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

        public void SaveCategorias(CategoriasModel modelo)
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification
            {
                BAJA = false
            };

            if (modelo.Accion == EnumAccionEntity.Modificacion)
            {
                T_M_CATEGORIASSpecification spec = new T_M_CATEGORIASSpecification
                {
                    ID_CATEGORIA = modelo.ID_Categoria,
                };

                UnitOfWork unitOW = new UnitOfWork();

                int numOrdenacion = unitOW.RepositoryT_M_CATEGORIAS.Where(spec).FirstOrDefault().ORDENACION;
                if (modelo.Ordenacion != numOrdenacion)
                {
                    if (modelo.Ordenacion <= unitOW.RepositoryT_M_CATEGORIAS.Where(specCategoria).Count())
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
                ReOrdenaCategorias(new UnitOfWork().RepositoryT_M_CATEGORIAS.Where(specCategoria).ToList().Count() + 1, modelo.Ordenacion);
            }

            using (var unitOfWork = new UnitOfWork())
            {
                    var categoria = new T_M_CATEGORIAS
                {
                    NOMBRE = modelo.Nombre,
                    ORDENACION = modelo.Ordenacion,
                    BAJA = false,
                };

                if (modelo.Accion == EnumAccionEntity.Modificacion)
                {
                    categoria.ID_CATEGORIA = modelo.ID_Categoria;
                    unitOfWork.RepositoryT_M_CATEGORIAS.Update(categoria);
                }
                else
                {
                    unitOfWork.RepositoryT_M_CATEGORIAS.Insert(categoria);
                }



                unitOfWork.Commit();
            }
        }


        public void BorrarCategoria(int idCategoria)
        {
            T_M_CATEGORIASSpecification spec = new T_M_CATEGORIASSpecification
            {
                ID_CATEGORIA = idCategoria
            };

            using (var unitOfWork = new UnitOfWork())
            {
                T_M_CATEGORIAS categoria = unitOfWork.RepositoryT_M_CATEGORIAS.Where(spec).FirstOrDefault();

                if (categoria != null)
                {
                    categoria.BAJA = true;
                }

                unitOfWork.RepositoryT_M_CATEGORIAS.Update(categoria);

                unitOfWork.Commit();
            }

            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification
            {
                BAJA = false
            };
            ReOrdenaCategorias(new UnitOfWork().RepositoryT_M_CATEGORIAS.Where(specCategoria).ToList().Count() + 1, 0);
        }


        private void ReOrdenaCategorias(int numOrdenAnterior, int numOrdenNuevo)
        {
            T_M_CATEGORIASSpecification specCategoria = new T_M_CATEGORIASSpecification
            {
                BAJA = false
            };

            using (var unitOfWork = new UnitOfWork())
            {
                List<T_M_CATEGORIAS> categorias = new List<T_M_CATEGORIAS>();

                if (numOrdenNuevo == 0)
                {
                    int numorden = 1;
                    categorias = unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                    .OrderBy(o => o.ORDENACION).ToList();
                    foreach (T_M_CATEGORIAS categoria in categorias)
                    {
                        categoria.ORDENACION = numorden;
                        unitOfWork.RepositoryT_M_CATEGORIAS.Update(categoria);
                        numorden++;
                    }
                }
                else
                {
                    if (numOrdenAnterior < numOrdenNuevo)
                    {
                        categorias = unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                        .Where(o => o.ORDENACION > numOrdenAnterior && o.ORDENACION <= numOrdenNuevo)
                                        .OrderBy(o => o.ORDENACION).ToList();
                        foreach (T_M_CATEGORIAS categoria in categorias)
                        {
                            categoria.ORDENACION = categoria.ORDENACION - 1;
                            unitOfWork.RepositoryT_M_CATEGORIAS.Update(categoria);
                        }
                    }
                    else if (numOrdenAnterior > numOrdenNuevo)
                    {
                        categorias = (from categoria in unitOfWork.RepositoryT_M_CATEGORIAS.Where(specCategoria)
                                      where categoria.ORDENACION >= numOrdenNuevo && categoria.ORDENACION < numOrdenAnterior
                                      select categoria).OrderBy(o => o.ORDENACION).ToList();
                        foreach (T_M_CATEGORIAS categoria in categorias)
                        {
                            categoria.ORDENACION = categoria.ORDENACION + 1;
                            unitOfWork.RepositoryT_M_CATEGORIAS.Update(categoria);
                        }
                    }
                }
                unitOfWork.Commit();
            }
        }
    }
}