using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositorySAPHR_DireccionesTerritoriales
    {
        IQueryable<SAPHR_DireccionesTerritoriales> GetDirrecionesTerritorialesByEmpresas(IEnumerable<int> empresas);
    }
}
