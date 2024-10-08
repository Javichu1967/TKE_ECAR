using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryECAR_Datos_Conductor
    {

        public ECAR_Datos_Conductor GetConductorByMatricula(string matricula)
        {
            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                Matricula = matricula,
            };

            return (from conductor in Fetch()
                    join vehiculos in InternalContext.Set<ECAR_Datos_Vehiculo>().Where(spec.GetExpression())
                    on conductor.Cod_Conductor equals vehiculos.Conductor
                    select conductor).FirstOrDefault();
        }

        public IQueryable<ECAR_Datos_Conductor> GetConductoresWithFechaCarnetVencida(DateTime fechaHasta)
        {
            ECAR_Datos_ConductorSpecification spec = new ECAR_Datos_ConductorSpecification
            {
                Fecha_Vencimiento_CarnetTo = fechaHasta
            };

            return Where(spec);
        }

        public ECAR_Datos_Conductor GetConductorByID(int id)
        {
            ECAR_Datos_ConductorSpecification spec = new ECAR_Datos_ConductorSpecification
            {
                Cod_Conductor = id, 
            };
            
             return  (from conductor in Fetch().Where(spec.GetExpression())
                         select conductor ).FirstOrDefault();
        }

        public ECAR_Datos_Conductor GetConductorByNumeroEmpleado(string numEmpleado)
        {
            ECAR_Datos_ConductorSpecification spec = new ECAR_Datos_ConductorSpecification
            {
                Num_Empleado = numEmpleado,
            };

            return (from conductor in Fetch().Where(spec.GetExpression())
                    select conductor).FirstOrDefault();
        }

    }
}
