using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;

namespace TK_ECAR.Application_Services
{
    public class ConductoresService
    {
        public List<ConductorDataTableModel> AllConductoresDataTable(int? codConductor = null)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ECAR_Datos_ConductorSpecification spec = new ECAR_Datos_ConductorSpecification
                {
                    Cod_Conductor = codConductor
                };
                var conductores = (from conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Where(spec)
                                   select new ConductorDataTableModel
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
                                   }).ToList();

                return conductores;

            }
        }

        public ConductorDataTableModel GetConductorByID(int codConductor)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ECAR_Datos_ConductorSpecification spec = new ECAR_Datos_ConductorSpecification
                {
                    Cod_Conductor = codConductor
                };
                var conductores = (from conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Where(spec)
                                   select new ConductorDataTableModel
                                   {
                                       Cod_Conductor = conductor.Cod_Conductor,
                                       Direccion = conductor.Direccion,
                                       Fecha_Vencimiento_Carnet = conductor.Fecha_Vencimiento_Carnet,
                                       Fecha_Nacimiento = conductor.Fecha_Nacimiento,
                                       Cod_Postal = conductor.Cod_Postal,
                                       Poblacion = conductor.Poblacion,
                                       Provincia = conductor.Provincia,
                                       Nombre = (conductor.Nombre ?? string.Empty) + " " + conductor.Apellidos,
                                       DNI = conductor.DNI
                                   }).FirstOrDefault();

                return conductores;

            }
        }


        public List<SelectChosen> GetConductoresChosen(string term)
        {
            ISpecification<ECAR_Datos_Conductor> spec = null;
            spec = new ECAR_Datos_ConductorSpecification
            {
                NombreContains = term,
            };
            ECAR_Datos_ConductorSpecification specApellido = new ECAR_Datos_ConductorSpecification
            {
                ApellidosContains = term,
            };

            spec = spec.Or(specApellido);

            using (var unitOfWork = new UnitOfWork())
            {
                var conductores = (from conductor in unitOfWork.RepositoryECAR_Datos_Conductor.Where(spec)
                                       //where conductor.Nombre.ToUpper().Contains(term.ToUpper()) || conductor.Apellidos.ToUpper().Contains(term.ToUpper())
                                   select new SelectChosen
                                   {
                                       DevolverValueFormateado = false,
                                       PonerValuePorDelanteDeTexto = false,
                                       text = (conductor.Nombre ?? string.Empty) + " " + conductor.Apellidos ?? string.Empty,
                                       value = conductor.Cod_Conductor.ToString(),
                                   }).Take(100).OrderBy(x => x.text).ToList();

                return conductores;

            }
        }


        public ConductorDataTableModel GetConductor(string matricula)
        {
            ConductorDataTableModel conductorModel = null;
            using (var unitOfWork = new UnitOfWork())
            {
                var conductor = unitOfWork.RepositoryECAR_Datos_Conductor.GetConductorByMatricula(matricula);

                if (conductor != null)
                {
                    conductorModel = new ConductorDataTableModel
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
                };

                }
                return conductorModel;
            }
        }

    }
}