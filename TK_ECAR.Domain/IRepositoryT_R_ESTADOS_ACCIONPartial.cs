using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface  IRepositoryT_R_ESTADOS_ACCION
    {
        T_R_ESTADOS_ACCION First(int idTipoAlerta );

        T_R_ESTADOS_ACCION Next(int idTipoAlerta, int idEstado);

        IQueryable<T_R_ESTADOS_ACCION> AllFirst(int idTipoAlerta);

        IQueryable<T_R_ESTADOS_ACCION> GetByAccion(int idTipoAlerta, int idAccion);
    }
}
