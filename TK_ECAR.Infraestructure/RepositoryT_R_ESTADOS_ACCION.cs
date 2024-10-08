using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_R_ESTADOS_ACCION : RepositoryBase<T_R_ESTADOS_ACCION>, IRepositoryT_R_ESTADOS_ACCION
    {
		public RepositoryT_R_ESTADOS_ACCION(ModelEntities context)
            : base(context)
        {

        }
    }
}