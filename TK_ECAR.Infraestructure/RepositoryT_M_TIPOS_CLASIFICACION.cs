using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPOS_CLASIFICACION : RepositoryBase<T_M_TIPOS_CLASIFICACION>, IRepositoryT_M_TIPOS_CLASIFICACION
    {
		public RepositoryT_M_TIPOS_CLASIFICACION(ModelEntities context)
            : base(context)
        {

        }
    }
}