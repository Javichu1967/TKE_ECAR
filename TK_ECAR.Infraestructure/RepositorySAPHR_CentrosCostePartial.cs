
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositorySAPHR_CentrosCoste
    {

       public IQueryable<SAPHR_CentrosCoste> GetCecosByDelegaciones(IEnumerable<string> codigosDelegaciones, IEnumerable<int> codigoEmpresas, IEnumerable<string> codigosDt)
        {



            Expression<Func<SAPHR_CentrosCoste, bool>> expr =
               x => (codigoEmpresas.Contains(x.Empresa)) &&
                     (string.IsNullOrEmpty(x.IdDT) ||
                    codigosDt.Contains(x.IdDT)) &&
                     (string.IsNullOrEmpty(x.IdDelegacion) ||
                    codigosDelegaciones.Contains(x.IdDelegacion)); //&& (x.Baja.Equals(false)); SE QUITA LA BAJA PORQUE HAY CECOS DADOS DE BAJA ASOCIADOS A MATRÍCULAS


            //Expression<Func<SAPHR_CentrosCoste, bool>> expr =
            //    x => (((codigoEmpresas.Contains(x.Empresa) && 
            //          string.IsNullOrEmpty(x.IdDelegacion)) ||
            //         (codigosDelegaciones.Contains(x.IdDelegacion))) && 
            //         x.Baja.Equals(false) ) ;
               
             
            
            return Fetch().Where(expr);
                
        }

        public IQueryable<SAPHR_CentrosCoste> GetCecos(IEnumerable<string> idcecos, bool MirarBaja = true)
        {
            var spec = new SAPHR_CentrosCosteSpecification
            {
                IdCecoIN = idcecos,
            };

            if (MirarBaja)
            {
                spec.Baja = false;
            }


            
            return Where(spec);

        }

    }
}
