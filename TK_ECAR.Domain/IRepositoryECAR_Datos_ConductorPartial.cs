using System;
using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositoryECAR_Datos_Conductor
    {
        ECAR_Datos_Conductor GetConductorByMatricula(string matricula);
        IQueryable<ECAR_Datos_Conductor> GetConductoresWithFechaCarnetVencida(DateTime fechaHasta);
        ECAR_Datos_Conductor GetConductorByID(int id);
        ECAR_Datos_Conductor GetConductorByNumeroEmpleado(string numEmpleado);

        //IQueryable<Datos_Conductor> GetConductoresWithFechaCarnetVencida(DateTime fechaHasta);
    }
}
