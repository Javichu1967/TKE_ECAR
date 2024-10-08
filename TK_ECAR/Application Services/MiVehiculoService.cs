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
    public class MiVehiculoService
    {
        /// <summary>
        /// Devuelve la matrícula del vehículo activo asociado al empleado que se le pasa por parámetro y no está de baja
        /// </summary>
        /// <returns></returns>
        public string GetMatriculaVehiculoActivo(string DNIusuario)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ECAR_Datos_ConductorSpecification specConductor = new ECAR_Datos_ConductorSpecification
                {
                    DNI = DNIusuario
                };

                ECAR_Datos_VehiculoSpecification specVehiculo = new ECAR_Datos_VehiculoSpecification
                {
                    Baja = false
                };

                var matriculaVehiculo = (from conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Where(specConductor)
                                         join
                                         vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Where(specVehiculo) on conductor.Cod_Conductor equals vehiculo.Conductor
                                         select new
                                         {
                                             Matricula = vehiculo.Matricula
                                         }).FirstOrDefault();

                string valorRetorno = string.Empty;

                if (matriculaVehiculo != null)
                {
                    valorRetorno = matriculaVehiculo.Matricula;
                }


                return valorRetorno;
            }
        }
    }
}