using System;
using System.Collections.Generic;
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
    public class ConductoresECARService
    {
        public List<ConductorECARModel> AllConductoresDataTable_ECAR(List<int> empresas = null)
        {
            ISpecification<V_CONDUCTORES_USUARIOS_SAP> spec = null;

            V_CONDUCTORES_USUARIOS_SAPSpecification especConductores = new V_CONDUCTORES_USUARIOS_SAPSpecification
            {
                ID_EMPRESA_SAPIN = empresas != null ? empresas.Cast<int?>() : null,
            };
            V_CONDUCTORES_USUARIOS_SAPSpecification especConductoresEmpNull = new V_CONDUCTORES_USUARIOS_SAPSpecification
            {
                ID_EMPRESA_SAP = null,
            };

            spec = especConductores.Or(especConductoresEmpNull);

            using (var unitOfWork = new UnitOfWork())
            {
                var listaConductores = (from conductor in unitOfWork.RepositoryV_CONDUCTORES_USUARIOS_SAP.Where(spec)
                                        select new ConductorECARModel
                                        {
                                            Cod_Conductor = conductor.Cod_Conductor,
                                            Nombre = conductor.Nombre != null ? conductor.Nombre : conductor.Nombre_SAP,
                                            Apellidos = conductor.Apellidos != null ? conductor.Apellidos : conductor.Apellidos_SAP,
                                            PersonalInterno = conductor.Personal_Interno == null ? true : (bool)conductor.Personal_Interno,
                                            IDEmpresa = conductor.ID_EMPRESA_SAP,
                                            CECO = conductor.CentroCoste,
                                            Tlfn = conductor.Tlf,
                                            DescEmpresa = conductor.NombreEmpresa,
                                            DescCECO = conductor.NombreCECO,
                                            PendienteDefinir = conductor.PendienteDefinir == null ? true : conductor.PendienteDefinir,
                                        }).ToList();



                return listaConductores;
            }

        }

        //public List<ConductoresOnlyDataTableModel> AllConductoresOnlyDataTable_ECAR(string search)
        //{
        //    ISpecification<ECAR_Datos_Conductor> spec = null;
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        spec = new ECAR_Datos_ConductorSpecification
        //        {
        //            CodCecoContains = search,
        //        };

        //        ECAR_Datos_ConductorSpecification spec1 = new ECAR_Datos_ConductorSpecification();
        //        spec1.NombreContains = search;
        //        spec = spec.Or(spec1);

        //        ECAR_Datos_ConductorSpecification spec2 = new ECAR_Datos_ConductorSpecification();
        //        spec2.ApellidosContains = search;
        //        spec = spec.Or(spec2);
        //    }
        //    else
        //    {
        //        spec = new ECAR_Datos_ConductorSpecification();
        //    }

        //    using (var unitOfWork = new UnitOfWork())
        //    {
        //        var conductores = (from conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Where(spec)
        //                           select new ConductoresOnlyDataTableModel
        //                           {
        //                               Cod_Conductor = conductor.Cod_Conductor,
        //                               CECO = conductor.CodCeco,
        //                               NombreCompleto = (conductor.Nombre ?? string.Empty) + " " + conductor.Apellidos,
        //                               AccionDatatable = "",
        //                           }).ToList();

        //        return conductores;

        //    }
        //}

        public ConductorECARModel GetConductorByID_ECAR(int idConductor)
        {

            V_CONDUCTORES_USUARIOS_SAPSpecification especConductores = new V_CONDUCTORES_USUARIOS_SAPSpecification
            {
                Cod_Conductor = idConductor,
            };

            return GetConductoresECAR(especConductores).FirstOrDefault();

        }

        public ConductorECARModel GetConductorByNumEmpleado_ECAR(int? numEmpleado)
        {
            V_CONDUCTORES_USUARIOS_SAPSpecification especConductores = new V_CONDUCTORES_USUARIOS_SAPSpecification
            {
                Num_Empleado_SAP = numEmpleado,
            };

            return GetConductoresECAR(especConductores).FirstOrDefault();
        }


        private List<ConductorECARModel> GetConductoresECAR(ISpecification<V_CONDUCTORES_USUARIOS_SAP> especConductores)
        {

            using (var unitOfWork = new UnitOfWork())
            {
                var listaConductores = (from conductor in unitOfWork.RepositoryV_CONDUCTORES_USUARIOS_SAP.Where(especConductores)
                                        select new ConductorECARModel
                                        {
                                            Cod_Conductor = conductor.Cod_Conductor,
                                            Direccion = conductor.Direccion != null ? conductor.Direccion : conductor.Direccion_SAP,
                                            Fecha_Carnet = conductor.Fecha_Carnet,
                                            Fecha_Vencimiento_Carnet = conductor.Fecha_Vencimiento_Carnet,
                                            Fecha_Nacimiento = conductor.Fecha_Nacimiento != null ? conductor.Fecha_Nacimiento : conductor.Fecha_Nacimiento_SAP,
                                            Cod_Postal = conductor.Cod_Postal != null ? conductor.Cod_Postal : conductor.Cod_Postal_SAP,
                                            Poblacion = conductor.Poblacion != null ? conductor.Poblacion : conductor.Poblacion_SAP,
                                            Provincia = conductor.Provincia != null ? conductor.Provincia : conductor.Provincia_SAP,
                                            Nombre = conductor.Nombre != null ? conductor.Nombre : conductor.Nombre_SAP,
                                            Apellidos = conductor.Apellidos != null ? conductor.Apellidos : conductor.Apellidos_SAP,
                                            IdTipoDocumentoIdentificacion = conductor.IDTipoDocIdentificacion == null ? (int)EnumDocumentoIdentificacion.tipoDocI_DNI : conductor.IDTipoDocIdentificacion,
                                            TipoDocumentoIdentificacion = conductor.IDTipoDocIdentificacion == null ? (int)EnumDocumentoIdentificacion.tipoDocI_DNI : conductor.IDTipoDocIdentificacion,
                                            PersonalInterno = (bool)conductor.Personal_Interno,
                                            IDEmpresa = conductor.ID_EMPRESA_SAP,
                                            Tlfn = conductor.Tlf,
                                            Movil = conductor.Movil,
                                            NumEmpleado = conductor.Num_Empleado != null ? conductor.Num_Empleado : conductor.Num_Empleado_SAP,
                                            CECO = conductor.CentroCoste != null ? conductor.CentroCoste : conductor.CentroCoste_SAP,
                                            NumeroCarnetConducir = conductor.NumeroCarnetConducir,
                                            DNI = conductor.DNI_CONDUCTOR != null ? conductor.DNI_CONDUCTOR : conductor.DNI_SAP,
                                            Email = conductor.Email != null ? conductor.Email : conductor.Email_SAP,
                                            Fecha_Alta = conductor.FechaAlta,
                                            DescEmpresa = conductor.NombreEmpresa,
                                            PendienteDefinir = conductor.PendienteDefinir,
                                        }).OrderBy(x=>x.Nombre).ThenBy(x=>x.Apellidos).ToList();



                return listaConductores;
            }
        }

        public ConductorECARModel GetConductorSAPByID(int? id)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var usuariosSAP = (from uSAP in unitOfWork.RepositorySAPHR_UsuariosSAP.Fetch()
                                   where uSAP.NumeroEmpleado == id
                                   select new ConductorECARModel
                                   {
                                       Direccion = uSAP.Domicilio,
                                       Fecha_Nacimiento = uSAP.FecNacimiento,
                                       Cod_Postal = uSAP.CodPostal,
                                       Poblacion = uSAP.Poblacion,
                                       Provincia = uSAP.Provincia,
                                       Nombre = uSAP.Nombre,
                                       Apellidos = uSAP.Apellido1 + " " + uSAP.Apellido2,
                                       IdTipoDocumentoIdentificacion = (int)EnumDocumentoIdentificacion.tipoDocI_DNI,
                                       TipoDocumentoIdentificacion = (int)EnumDocumentoIdentificacion.tipoDocI_DNI,
                                       DNI = uSAP.Dni,
                                       PersonalInterno = true,
                                       IDEmpresa = uSAP.CodigoEmpresa,
                                       NumEmpleado = uSAP.NumeroEmpleado,
                                       CECO = uSAP.IdCeco,
                                       Email = uSAP.Email,
                                   }).FirstOrDefault();

                return usuariosSAP;

            }
        }

        public List<SelectChosen> GetConductorSAPByCecoChosen(string term, List<string> cecos)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                ISpecification<SAPHR_UsuariosSAP> specGeneral = null;

                ISpecification<SAPHR_UsuariosSAP> spec = new SAPHR_UsuariosSAPSpecification()
                {
                    Baja = false,
                    IdCecoIN = cecos,
                };

                var specNombre = new SAPHR_UsuariosSAPSpecification
                {
                    NombreContains = term
                };
                specGeneral = specNombre;
                var specApellido1 = new SAPHR_UsuariosSAPSpecification
                {
                    Apellido1Contains = term
                };
                specGeneral = specGeneral.Or(specApellido1);
                var specApellido2 = new SAPHR_UsuariosSAPSpecification
                {
                    Apellido2Contains = term
                };
                specGeneral = specGeneral.Or(specApellido2);

                spec = spec.And(specGeneral);

                var usuariosSAP = (from uSAP in unitOfWork.RepositorySAPHR_UsuariosSAP.Where(spec)
                                   where uSAP.IdUniOrganizativa != "50003566" && uSAP.IdDivision != "FE01" //Jubilados
                                   select new SelectChosen
                                   {
                                       PonerValuePorDelanteDeTexto = false,
                                       DevolverValueFormateado = false,
                                       text = uSAP.Nombre + " " + uSAP.Apellido1 + " " + uSAP.Apellido2,
                                       value = uSAP.NumeroEmpleado.ToString(),
                                   }).Take(100).OrderBy(o => o.value).ThenBy(x => x.text).Distinct().ToList();

                return usuariosSAP;

            }
        }


        public ConductorECARModel GetConductorByMatricula(string matricula)
        {
            ConductorECARModel conductorModel = new ConductorECARModel();
            using (var unitOfWork = new UnitOfWork())
            {
                var conductor = unitOfWork.RepositoryECAR_Datos_Conductor.GetConductorByMatricula(matricula);

                if (conductor != null)
                {
                    conductorModel = new ConductorECARModel
                    {
                        Cod_Conductor = conductor.Cod_Conductor,
                        Direccion = conductor.Direccion,
                        Fecha_Vencimiento_Carnet = conductor.Fecha_Vencimiento_Carnet,
                        Fecha_Nacimiento = conductor.Fecha_Nacimiento,
                        Cod_Postal = conductor.Cod_Postal,
                        Poblacion = conductor.Poblacion,
                        Provincia = conductor.Provincia,
                        Nombre = (conductor.Nombre ?? string.Empty) + " " + conductor.Apellidos,
                        DNI = conductor.DNI,
                        NumeroCarnetConducir = conductor.NumeroCarnetConducir,
                        TipoDocumentoIdentificacion = conductor.IDTipoDocIdentificacion == null ? (int)EnumDocumentoIdentificacion.tipoDocI_DNI : conductor.IDTipoDocIdentificacion,
                        IdTipoDocumentoIdentificacion = conductor.IDTipoDocIdentificacion == null ? (int)EnumDocumentoIdentificacion.tipoDocI_DNI : conductor.IDTipoDocIdentificacion,
                    };

                }
                return conductorModel;
            }
        }


        public bool SaveConductorECAR(ConductorECARModel modelo)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var conductor = new ECAR_Datos_Conductor();

                    conductor.Personal_Interno = modelo.PersonalInterno;
                    conductor.Nombre = modelo.Nombre;
                    conductor.Direccion = modelo.Direccion;
                    conductor.Fecha_Nacimiento = modelo.Fecha_Nacimiento;
                    conductor.Fecha_Carnet = modelo.Fecha_Carnet;
                    conductor.Fecha_Vencimiento_Carnet = modelo.Fecha_Vencimiento_Carnet;
                    conductor.Cod_Postal = modelo.Cod_Postal;
                    conductor.Poblacion = modelo.Poblacion;
                    conductor.Provincia = modelo.Provincia;
                    conductor.Apellidos = modelo.Apellidos;
                    conductor.IDTipoDocIdentificacion = modelo.TipoDocumentoIdentificacion;
                    conductor.NumeroCarnetConducir = modelo.NumeroCarnetConducir;
                    conductor.DNI = modelo.DNI;
                    conductor.CodCeco = modelo.CECO == null ? null : modelo.CECO.TrimStart('0');
                    conductor.E_Mail = modelo.Email;
                    conductor.Tlf = modelo.Tlfn;
                    conductor.Movil = modelo.Movil;
                    conductor.Baja = false;
                    conductor.Num_Empleado = modelo.NumEmpleado.ToString();
                    if (Convert.ToBoolean(modelo.PendienteDefinir))
                    {
                        if (!modelo.PersonalInterno)
                        {
                            conductor.PendienteDefinir = false;
                        }
                        else
                        {
                            if (modelo.NumEmpleado != null)
                            {
                                conductor.PendienteDefinir = false;
                            }
                        }
                    }

                    if (modelo.Accion == EnumAccionEntity.Alta)
                    {
                        conductor.FechaAlta = DateTime.Now;
                        unitOfWork.RepositoryECAR_Datos_Conductor.Insert(conductor);
                    }
                    else
                    {
                        conductor.Cod_Conductor = modelo.Cod_Conductor;
                        unitOfWork.RepositoryECAR_Datos_Conductor.Update(conductor);
                    }

                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, ex.Message);
                return false;
            }

            return true;
        }

        public bool DeleteConductorECAR(int idConductor)
        {
            try
            {
                //using (var unitOfWork = new UnitOfWork())
                //{
                //    var conductor = unitOfWork.RepositoryDatos_Conductor_ECAR.Fetch().Where(x=>x.Cod_Conductor == idConductor).FirstOrDefault();

                //    conductor.Baja = true;

                //    unitOfWork.RepositoryDatos_Conductor_ECAR.Update(conductor);

                //    unitOfWork.Commit();
                //}
            }
            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, ex.Message);
                return false;
            }

            return true;
        }
    }
}