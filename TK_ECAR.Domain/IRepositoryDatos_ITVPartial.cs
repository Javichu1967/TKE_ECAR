using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositoryDatos_ITV
    {

        IQueryable<Datos_ITV> GetITVFechaVencida(DateTime fechaVto, IUnitOfWork unitOfWork);
    }
}
