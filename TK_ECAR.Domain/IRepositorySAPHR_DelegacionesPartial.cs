using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositorySAPHR_Delegaciones
    {
        IQueryable<SAPHR_Delegaciones> GetDelegacionesByEmpresasOrDT(IEnumerable<int> empresas, IEnumerable<string> dts);

        IQueryable<SAPHR_Delegaciones> GetDelegacionesByUserZona(string logon);

        
    }
}
