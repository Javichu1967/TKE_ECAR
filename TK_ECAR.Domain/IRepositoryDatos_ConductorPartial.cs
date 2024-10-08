using System;
using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositoryDatos_Conductor
    {

        Datos_Conductor GetConductorByMatricula(string matricula);

        IQueryable<Datos_Conductor> GetConductoresWithFechaCarnetVencida(DateTime fechaHasta);
    }
}
