using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryT_R_ESTADOS_ACCION
    {

         
        public T_R_ESTADOS_ACCION First(int idTipoAlerta)
        {
            return FindOne(x => x.ID_TIPO_ALERTA.Equals(idTipoAlerta) &&
                                 !x.ID_ESTADO_ANTERIOR.HasValue);

             
        }
        
        public T_R_ESTADOS_ACCION Next(int idTipoAlerta, int idEstado)
        {
            //es el  que tiene como estado anterior nuestro estado
            //para el tipo de alerta especificado

           
                return  (from estadoAccion in Fetch()
                         where estadoAccion.ID_TIPO_ALERTA.Equals(idTipoAlerta)                      
                         where estadoAccion.ID_ESTADO_ANTERIOR == idEstado
                         select estadoAccion).FirstOrDefault(); 
        }

        public IQueryable<T_R_ESTADOS_ACCION> AllFirst(int idTipoAlerta)
        {
            return Fetch().Where(x => x.ID_TIPO_ALERTA.Equals(idTipoAlerta) &&
                                 !x.ID_ESTADO_ANTERIOR.HasValue);


        }

        public IQueryable<T_R_ESTADOS_ACCION> GetByAccion(int idTipoAlerta, int idAccion)
        {
            return Fetch().Where(x => x.ID_TIPO_ALERTA.Equals(idTipoAlerta) &&
                                 x.ID_ACCION == idAccion);


        }

    }
}
