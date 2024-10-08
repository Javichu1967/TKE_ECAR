using System;
using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositoryECAR_Datos_Vehiculo
    {
 
        List<string> GetMatriculas(string term, List<string> cecos, bool matriculasDeBaja);
        List<string> GetMatriculasByDelegaciones(List<string> delegaciones);
        List<string> GetMatriculasByEmpresas(List<int> empresas);
        List<string> GetMatriculasByCecos(List<string> cecos);
        IQueryable<ECAR_Datos_Vehiculo> GetAlertasRenting(DateTime fechapreaviso);

        IQueryable<ECAR_Datos_Vehiculo> GetAlertasSeguros(DateTime fechapreaviso); 
    }
}
