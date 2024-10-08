using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_CATEGORIAS : RepositoryBase<T_M_CATEGORIAS>, IRepositoryT_M_CATEGORIAS
    {
		public RepositoryT_M_CATEGORIAS(ModelEntities context)
            : base(context)
        {

        }
    }
}