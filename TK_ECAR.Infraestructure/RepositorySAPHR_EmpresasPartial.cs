
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositorySAPHR_Empresas
    {

        public IQueryable<SAPHR_Empresas> GetEmpresas(IEnumerable<int> idempresas, bool MirarBaja = true)
        {
            var spec = new SAPHR_EmpresasSpecification
            {
                CodigoEmpresaIN = idempresas.Select(x=>(int?)x).ToList(),
            };

            if (MirarBaja)
            {
                spec.Activo = true;
            }



            return Where(spec);

        }


    }
}
