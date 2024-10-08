using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPOS_ALERTAS : RepositoryBase<T_M_TIPOS_ALERTAS>, IRepositoryT_M_TIPOS_ALERTAS
    {
		public RepositoryT_M_TIPOS_ALERTAS(ModelEntities context)
            : base(context)
        {

        }
    }
}