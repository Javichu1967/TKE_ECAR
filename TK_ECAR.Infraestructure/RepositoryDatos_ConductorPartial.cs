using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryDatos_Conductor
    {
         
        public Datos_Conductor GetConductorByMatricula(string matricula )
        {
            Datos_VehiculoSpecification spec = new Datos_VehiculoSpecification
            {
                Matricula = matricula, 

            };
            
             return  (from conductor in  Fetch()
                         join vehiculos in InternalContext.Set<Datos_Vehiculo>().Where(spec.GetExpression() )
                         on conductor.Cod_Conductor equals vehiculos.Conductor
                         select conductor ).FirstOrDefault();


            
        }

        public IQueryable<Datos_Conductor> GetConductoresWithFechaCarnetVencida(DateTime fechaHasta )
        {
            Datos_ConductorSpecification spec = new Datos_ConductorSpecification
            {
                
                 Fecha_Vencimiento_CarnetTo  = fechaHasta
                 
            };
          

            return Where(spec) ;



        }
    }
}
