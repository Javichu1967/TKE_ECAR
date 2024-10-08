
using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositorySAPHR_Delegaciones
    {

       public IQueryable<SAPHR_Delegaciones> GetDelegacionesByEmpresasOrDT(IEnumerable<int> empresas, IEnumerable<string> dts)
        {
            //var spec = new SAPHR_DelegacionesSpecification
            //{
            //    EmpresaIN = empresas !=null ? empresas.Select(x=>(int?)x) : null,

            //    IdDTIN = dts,

            //    Baja = false
            //};
            //return Where(spec);


            System.Linq.Expressions.Expression<Func<SAPHR_Delegaciones, bool>> expr =
                   x => (empresas.Contains(x.Empresa)) && 
                        (string.IsNullOrEmpty(x.IdDT) || dts.Contains(x.IdDT)) && (x.Baja.Equals(false));

            return Fetch().Where(expr);

        }

        public IQueryable<SAPHR_Delegaciones> GetDelegacionesByUserZona(string logon)
        {
            SAPHR_DelegacionesSpecification spec = new SAPHR_DelegacionesSpecification
            {
                MERSY_ZONAS = new MERSY_ZONASSpecification
                {
                    MERSY_USERS_ZONAS = new MERSY_USERS_ZONASSpecification
                    {
                        User_Logon = logon,

                    }
                },
                Baja = false

            };
            return Where(spec);

        }

      
    }
}
