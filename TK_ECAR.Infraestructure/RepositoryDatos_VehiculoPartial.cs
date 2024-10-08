using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryDatos_Vehiculo
    {
        
        public List<string> GetMatriculas(string matriculaStartWith, List<string> cecos)
        {
            Datos_VehiculoSpecification spec = new Datos_VehiculoSpecification
            {
                MatriculaStartsWith = matriculaStartWith,

                CCIN = cecos,

                //Baja = false

            };
            return Where(spec).Select(x => x.Matricula).ToList();

        }
        public List<string> GetMatriculasByCecos(List<string> cecos)
        {
            Datos_VehiculoSpecification spec = new Datos_VehiculoSpecification
            {
                CCIN = cecos,

                Baja = false
            };
            return Where(spec).Select(x => x.Matricula).ToList();
        }

        public List<string> GetMatriculasByEmpresas(List<int> empresas)
        {
            Datos_VehiculoSpecification spec = new Datos_VehiculoSpecification
            {
                SociedadIN = empresas.Select(x => (int?)x).ToList(),
                Baja = false
            };
            return Where(spec).Select(x => x.Matricula).ToList();
        }
        public List<string> GetMatriculasByDelegaciones(List<string> delegaciones)
        {
            Datos_VehiculoSpecification spec = new Datos_VehiculoSpecification
            {
                DelegacionIN = delegaciones,
                Baja = false
            };
            return Where(spec).Select(x => x.Matricula).ToList();
        }



        public IQueryable<Datos_Vehiculo> GetAlertasRenting(DateTime fechapreaviso)
        {
            
            return Fetch()
                .Where(x=> x.Baja == false && x.EmpresaLeasing != 2)
                .Where(x => x.Fecha_Alta.HasValue && x.Cuotas.HasValue   )
                .Where(x => SqlFunctions.DateAdd("month", x.Cuotas, x.Fecha_Alta) <= fechapreaviso);
                

        }
        public IQueryable<Datos_Vehiculo> GetAlertasSeguros(DateTime fechapreaviso)
        {
            Datos_VehiculoSpecification spec = new Datos_VehiculoSpecification
            {
                Baja = false,
                Vto_SeguroTo = fechapreaviso
            };

            return Where(spec);


        }

    }
}
