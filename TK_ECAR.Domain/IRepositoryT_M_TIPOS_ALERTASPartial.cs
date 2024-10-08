using System;
using System.Linq;
using System.Linq.Expressions;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositoryT_M_TIPOS_ALERTAS
    {
        IQueryable<T_M_TIPOS_ALERTAS> GetAutomaticas();

        IQueryable<T_M_TIPOS_ALERTAS> GetAutomaticasWithInclude(params Expression<Func<T_M_TIPOS_ALERTAS, object>>[] includes);
    }
}
