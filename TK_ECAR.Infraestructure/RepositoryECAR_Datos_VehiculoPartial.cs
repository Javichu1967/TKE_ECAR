using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryECAR_Datos_Vehiculo
    {
        
        public List<string> GetMatriculas(string term, List<string> cecos, bool matriculasDeBaja)
        {
            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                MatriculaContains = term,

                CCIN = cecos,
            };

            if (matriculasDeBaja)
            {
                spec.Baja = true;
            }
            else
            {
                spec.Baja = false;
            }

            return Where(spec).Select(x => x.Matricula).Take(100).ToList();

        }
        public List<string> GetMatriculasByCecos(List<string> cecos)
        {
            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                CCIN = cecos,

                Baja = false
            };
            return Where(spec).Select(x => x.Matricula).ToList();
        }

        public List<string> GetMatriculasByEmpresas(List<int> empresas)
        {
            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                SociedadIN = empresas.Select(x => (int?)x).ToList(),
                Baja = false
            };
            return Where(spec).Select(x => x.Matricula).ToList();
        }
        public List<string> GetMatriculasByDelegaciones(List<string> delegaciones)
        {
            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                DelegacionIN = delegaciones,
                Baja = false
            };
            return Where(spec).Select(x => x.Matricula).ToList();
        }



        public IQueryable<ECAR_Datos_Vehiculo> GetAlertasRenting(DateTime fechapreaviso)
        {
            
            return Fetch()
                .Where(x=> x.Baja == false && x.EmpresaLeasing != 2)
                .Where(x => x.Fecha_Alta.HasValue && x.Cuotas.HasValue   )
                .Where(x => SqlFunctions.DateAdd("month", x.Cuotas, x.Fecha_Alta) <= fechapreaviso);
                

        }
        public IQueryable<ECAR_Datos_Vehiculo> GetAlertasSeguros(DateTime fechapreaviso)
        {
            ECAR_Datos_VehiculoSpecification spec = new ECAR_Datos_VehiculoSpecification
            {
                Baja = false,
                Vto_SeguroTo = fechapreaviso
            };

            return Where(spec);


        }

    }
}
