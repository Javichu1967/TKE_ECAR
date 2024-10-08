
using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositorySAPHR_DireccionesTerritoriales
    {

       public IQueryable<SAPHR_DireccionesTerritoriales> GetDirrecionesTerritorialesByEmpresas(IEnumerable<int> empresas)
        {
            var spec = new SAPHR_DireccionesTerritorialesSpecification
            {
                EmpresaIN = empresas != null ? empresas.Select(x => (int?)x) : null,

                Baja = false
            };
            return Where(spec);
                
        }
         

    }
}
