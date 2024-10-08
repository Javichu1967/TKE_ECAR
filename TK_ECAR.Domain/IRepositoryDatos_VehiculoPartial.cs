using System;
using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositoryDatos_Vehiculo
    {
 
        List<string> GetMatriculas(string matriculaStartWith, List<string> cecos);
        List<string> GetMatriculasByDelegaciones(List<string> delegaciones);
        List<string> GetMatriculasByEmpresas(List<int> empresas);
        List<string> GetMatriculasByCecos(List<string> cecos);
        IQueryable<Datos_Vehiculo> GetAlertasRenting(DateTime fechapreaviso);

        IQueryable<Datos_Vehiculo> GetAlertasSeguros(DateTime fechapreaviso); 
    }
}
