using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositorySAPHR_CentrosCoste
    {
        IQueryable<SAPHR_CentrosCoste> GetCecosByDelegaciones(IEnumerable<string> delegaciones, IEnumerable<int> codigoEmpresas, IEnumerable<string> codigosDt);

        IQueryable<SAPHR_CentrosCoste> GetCecos(IEnumerable<string> idcecos, bool MirarBaja);

    }
}
