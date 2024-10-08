using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_CLIENTES : RepositoryBase<T_M_CLIENTES>, IRepositoryT_M_CLIENTES
    {
		public RepositoryT_M_CLIENTES(ModelEntities context)
            : base(context)
        {

        }
    }
}