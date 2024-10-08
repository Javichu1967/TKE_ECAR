using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;

namespace TK_ECAR.Application_Services
{
    public class GestoresFlotaService
    {
        /// <summary>
        /// Devuelve los Gestores de Flota
        /// </summary>
        /// <returns></returns>
        public List<GestoresFlotaModel> GetAllGestoresFlota()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var listaGestores = (from gestor in unitOfWork.RepositoryT_G_GESTORES_FLOTA.Fetch()
                                     select new GestoresFlotaModel
                                      {
                                        Foto = gestor.FOTO,
                                        NumeroEmpleado = gestor.NUMEROEMPLEADO,
                                        Accion = EnumAccionEntity.SinAccion,
                                        Puesto = gestor.PUESTO,
                                        Telefono1 = gestor.TELEFONO1,
                                        Telefono2 = gestor.TELEFONO2,
                                        FechaAlta = gestor.FECHA_ALTA
                                     }).ToList();

                foreach(GestoresFlotaModel gestor in listaGestores)
                {
                    var usuario = unitOfWork.RepositorySAPHR_UsuariosSAP.Fetch().Where(o => o.NumeroEmpleado == gestor.NumeroEmpleado).FirstOrDefault();

                    if (usuario != null)
                    {
                        gestor.Nombre = usuario.Nombre + " " + usuario.Apellido1 + " " + usuario.Apellido2;
                    }
                    else
                    {
                        gestor.Nombre = "Usuario no encontrado en SAPHR_UsuariosSAP - " + gestor.NumeroEmpleado.ToString();
                    }
                }

                return listaGestores.OrderBy(o=>o.Nombre).ToList();
            }
        }


        public GestoresFlotaModel GetGestorFlota(int? numEmpleado)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                T_G_GESTORES_FLOTASpecification spec = new T_G_GESTORES_FLOTASpecification();
                spec.NUMEROEMPLEADO = numEmpleado;

                var gestor = (from _gestor in unitOfWork.RepositoryT_G_GESTORES_FLOTA.Where(spec)
                              select new GestoresFlotaModel
                              {
                                Foto = _gestor.FOTO,
                                NumeroEmpleado = _gestor.NUMEROEMPLEADO,
                                Accion = EnumAccionEntity.SinAccion,
                                Puesto = _gestor.PUESTO,
                                Telefono1 = _gestor.TELEFONO1,
                                Telefono2 = _gestor.TELEFONO2,
                                FechaAlta = _gestor.FECHA_ALTA
                              }).FirstOrDefault();

                if (gestor != null)
                {
                    var usuario = unitOfWork.RepositorySAPHR_UsuariosSAP.Fetch().Where(o => o.NumeroEmpleado == gestor.NumeroEmpleado).FirstOrDefault();

                    if (usuario != null)
                    {
                        gestor.Nombre = usuario.Nombre + " " + usuario.Apellido1 + " " + usuario.Apellido2;
                    }
                    else
                    {
                        gestor.Nombre = "Usuario no encontrado en SAPHR_UsuariosSAP - " + gestor.NumeroEmpleado.ToString();
                    }
                }

                return gestor;
            }
        }

        #region Mantenimiento
        public void SaveGestorFlota(GestoresFlotaModel modelo)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var gestor = new T_G_GESTORES_FLOTA
                {
                    NUMEROEMPLEADO = modelo.NumeroEmpleado,
                    FOTO = modelo.Foto,
                    FECHA_MODIFICACION = DateTime.Now,
                    PUESTO = modelo.Puesto,
                    TELEFONO1 = modelo.Telefono1,
                    TELEFONO2 = modelo.Telefono2
                };

                BorraArchivoFoto(gestor.NUMEROEMPLEADO);

                if (modelo.Accion == EnumAccionEntity.Modificacion)
                {
                    gestor.FECHA_ALTA = GetGestorFlota(modelo.NumeroEmpleado).FechaAlta;
                    unitOfWork.RepositoryT_G_GESTORES_FLOTA.Update(gestor);
                }
                else
                {
                    gestor.FECHA_ALTA = DateTime.Now;
                    unitOfWork.RepositoryT_G_GESTORES_FLOTA.Insert(gestor);
                }

                unitOfWork.Commit();

                if (modelo.FicheroFoto != null)
                {
                    FileUtilities.UploadFile(modelo.FicheroFoto, Global.PathToUploadFotoGestoresFlota + gestor.NUMEROEMPLEADO.ToString() + "/");
                }
            }
        }


        public void DeleteGestorFlota(int numeroEmpleado)
        {
            T_G_GESTORES_FLOTASpecification spec = new T_G_GESTORES_FLOTASpecification();
            spec.NUMEROEMPLEADO = numeroEmpleado;

            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.RepositoryT_G_GESTORES_FLOTA.Delete(numeroEmpleado);

                unitOfWork.Commit();

            }

            BorraArchivoFoto(numeroEmpleado);

        }
        #endregion


        #region métodos privados
        private void BorraArchivoFoto(int numeroEmpleado)
        {
            if (Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(Global.PathToUploadFotoGestoresFlota + numeroEmpleado.ToString() + "/")))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(Global.PathToUploadFotoGestoresFlota + numeroEmpleado.ToString() + "/"));

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
        }
        #endregion

    }
}