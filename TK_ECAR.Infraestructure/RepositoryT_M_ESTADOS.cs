using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_ESTADOS : RepositoryBase<T_M_ESTADOS>, IRepositoryT_M_ESTADOS
    {
		public RepositoryT_M_ESTADOS(ModelEntities context)
            : base(context)
        {

        }
    }
}