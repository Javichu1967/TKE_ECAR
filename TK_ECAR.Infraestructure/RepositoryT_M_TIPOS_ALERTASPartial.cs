using System;
using System.Linq;
using System.Linq.Expressions;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryT_M_TIPOS_ALERTAS
    {

        public IQueryable<T_M_TIPOS_ALERTAS> GetAutomaticas( )
        {
            return Fetch()
                .Where(x => x.B_AUTOMATICA); 

        }

        public IQueryable<T_M_TIPOS_ALERTAS> GetAutomaticasWithInclude(params Expression<Func<T_M_TIPOS_ALERTAS, object>>[] includes)
        {
            //Include(x => x.T_R_ESTADOS_ACCION)
            return Include(x => x.T_R_ESTADOS_ACCION)
                .Where(x => x.B_AUTOMATICA);

        }

    }
}
