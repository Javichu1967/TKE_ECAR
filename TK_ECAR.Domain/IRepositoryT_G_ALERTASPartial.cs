using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface  IRepositoryT_G_ALERTAS
    {
        IQueryable<T_G_ALERTAS> GetPendientes(List<string> codigoCecos);

        //IQueryable<T_G_ALERTAS> Get(List<string> codigoCecos, List<int> estados);
        

        IQueryable<T_G_ALERTAS> GetAutomaticasPendientes();

        IQueryable<T_G_ALERTAS> GetRentingRechazado();
    }
}
