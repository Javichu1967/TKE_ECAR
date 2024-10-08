using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{   
    public partial interface IRepositorySAPHR_Empresas : IRepositoryBase<SAPHR_Empresas>
    {
        IQueryable<SAPHR_Empresas> GetEmpresas(IEnumerable<int> idempresas, bool MirarBaja);

    }
}