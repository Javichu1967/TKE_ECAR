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
    public class EmpresasVehiculosService
    {
        /// <summary>
        /// Devuelve las empresas aseguradoras y las de renting
        /// </summary>
        /// <param name="mirarBaja"></param>
        /// <returns></returns>
        public List<EmpresasVehiculosDataTableModel> AllEmpVehiculosDataTable(bool mirarBaja = true)
        {
            T_M_EMPRESAS_VEHICULOSSpecification especEmp = new T_M_EMPRESAS_VEHICULOSSpecification();

            if (mirarBaja)
            {
                especEmp.BAJA = false;
            }

            return EmpVehiculos(especEmp);
        }


        /// <summary>
        /// Devuelve las empresas de Renting
        /// </summary>
        /// <param name="mirarBaja"></param>
        /// <returns></returns>
        public List<EmpresasVehiculosDataTableModel> EmpVehiculosRenting(bool mirarBaja = true)
        {
            T_M_EMPRESAS_VEHICULOSSpecification especEmp = new T_M_EMPRESAS_VEHICULOSSpecification
            {
                RENTING = true,
            };

            if (mirarBaja)
            {
                especEmp.BAJA = false;
            }



            return EmpVehiculos(especEmp);
        }

        /// <summary>
        /// Devuelve las empresas aseguradoras
        /// </summary>
        /// <param name="mirarBaja"></param>
        /// <returns></returns>
        public List<EmpresasVehiculosDataTableModel> EmpVehiculosAseguradoras(bool mirarBaja = true)
        {
            T_M_EMPRESAS_VEHICULOSSpecification especEmp = new T_M_EMPRESAS_VEHICULOSSpecification
            {
                ASEGURADORA = true,
            };

            if (mirarBaja)
            {
                especEmp.BAJA = false;
            }



            return EmpVehiculos(especEmp);
        }



        /// <summary>
        /// Devuelve una empresa según su ID
        /// </summary>
        /// <param name="idConductor"></param>
        /// <returns></returns>
        public EmpresasVehiculosModels GetEmpVehiculoByID(int idEmpresa)
        {

            T_M_EMPRESAS_VEHICULOSSpecification especEmp = new T_M_EMPRESAS_VEHICULOSSpecification
            {
                ID_EMPRESA = idEmpresa,
            };

            var emp = EmpVehiculos(especEmp).FirstOrDefault();

            var empVehiculo = new EmpresasVehiculosModels();

            if (emp != null)
            {
                empVehiculo.Aseguradora = emp.Aseguradora;
                empVehiculo.Baja = emp.Baja;
                empVehiculo.CodPostal = emp.CodPostal;
                empVehiculo.Direccion = emp.Direccion;
                empVehiculo.Email = emp.Email;
                empVehiculo.IDEmpresa = emp.IDEmpresa;
                empVehiculo.Nombre = emp.Nombre;
                empVehiculo.PersonaContacto = emp.PersonaContacto;
                empVehiculo.Poblacion = emp.Poblacion;
                empVehiculo.Telefono1 = emp.Telefono1;
                empVehiculo.Telefono2 = emp.Telefono2;
                empVehiculo.Renting = emp.Renting;
                empVehiculo.NIF = emp.NIF;
            }

            return empVehiculo;

        }


        /// <summary>
        ///Devuelve las empresas de renting y aseguradoras, según la especificación
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        private List<EmpresasVehiculosDataTableModel> EmpVehiculos(T_M_EMPRESAS_VEHICULOSSpecification spec)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var listaEmp = (from emp in unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Where(spec)
                                        select new EmpresasVehiculosDataTableModel
                                        {
                                            IDEmpresa = emp.ID_EMPRESA,
                                            Nombre = emp.NOMBRE,
                                            NIF = emp.NUM_DOCUMENTO,
                                            Direccion = emp.DIRECCION,
                                            Poblacion = emp.POBLACION,
                                            CodPostal = emp.CODPOSTAL,
                                            PersonaContacto = emp.PERSONA_CONTACTO,
                                            Telefono1 = emp.TELEFONO1,
                                            Telefono2 = emp.TELEFONO2,
                                            Email = emp.EMAIL,
                                            Aseguradora = emp.ASEGURADORA,
                                            Renting = emp.RENTING,
                                            Baja = emp.BAJA,
                                            Accion = EnumAccionEntity.SinAccion,
                                            AccionDatatable = "",
                                        }).OrderBy(x => x.Nombre).ToList();


                return listaEmp;
            }

        }


        public List<SelectChosen> GetEmpVehiculoByID_Chosen(int idEmpresa)
        {
            List<SelectChosen> empVehiculo = new List<SelectChosen>();

            T_M_EMPRESAS_VEHICULOSSpecification especEmp = new T_M_EMPRESAS_VEHICULOSSpecification
            {
                ID_EMPRESA = idEmpresa,
            };

            empVehiculo = (from unidad in EmpVehiculos(especEmp)
                           select new SelectChosen
                           {
                               PonerValuePorDelanteDeTexto = false,
                               text = unidad.Nombre,
                               value = unidad.IDEmpresa.ToString(),
                           }).OrderBy(x => x.text).ToList();

            return empVehiculo;
        }

        public List<SelectChosen> GetEmpVehiculo_Chosen(string term, int TipoEmpresa, bool mirarBaja = true)
        {
            List<SelectChosen> empVehiculo = new List<SelectChosen>();

            T_M_EMPRESAS_VEHICULOSSpecification especEmp = new T_M_EMPRESAS_VEHICULOSSpecification
            {
                NOMBREContains = term,
            };

            if (TipoEmpresa == (int)EnumTipoEmpresaVehiculo.Seguros)
            {
                especEmp.ASEGURADORA = true;
            }
            if (TipoEmpresa == (int)EnumTipoEmpresaVehiculo.Leasing)
            {
                especEmp.RENTING = true;
            }

            if (mirarBaja)
            { 
                especEmp.BAJA = false;
            }

            empVehiculo = (from unidad in EmpVehiculos(especEmp)
                           select new SelectChosen
                           {
                               PonerValuePorDelanteDeTexto = false,
                               text = unidad.Nombre,
                               value = unidad.IDEmpresa.ToString(),
                           }).OrderBy(x => x.text).ToList();

            return empVehiculo;
        }


        public bool SaveEmpVehiculos(EmpresasVehiculosModels modelo)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var empVehiculo = new T_M_EMPRESAS_VEHICULOS
                    {
                        NOMBRE = modelo.Nombre,
                        NUM_DOCUMENTO = modelo.NIF,
                        DIRECCION = modelo.Direccion,
                        POBLACION = modelo.Poblacion,
                        CODPOSTAL = modelo.CodPostal,
                        PERSONA_CONTACTO = modelo.PersonaContacto,
                        TELEFONO1 = modelo.Telefono1,
                        TELEFONO2 = modelo.Telefono2,
                        EMAIL = modelo.Email,
                        ASEGURADORA = modelo.Aseguradora,
                        RENTING = modelo.Renting,
                        BAJA = modelo.Baja,
                    };

                    if (modelo.Accion == EnumAccionEntity.Alta)
                    {
                        unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Insert(empVehiculo);
                    }
                    else
                    {
                        empVehiculo.ID_EMPRESA = modelo.IDEmpresa;
                        unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Update(empVehiculo);
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

        public bool DeleteEmpVehiculos(int idEmpVehiculo)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var emp = unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Fetch().Where(x => x.ID_EMPRESA == idEmpVehiculo).FirstOrDefault();

                    emp.BAJA = true;

                    unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Update(emp);

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

    }
}