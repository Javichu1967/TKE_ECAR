using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_ACCIONES : RepositoryBase<T_M_ACCIONES>, IRepositoryT_M_ACCIONES
    {
		public RepositoryT_M_ACCIONES(ModelEntities context)
            : base(context)
        {

        }
    }
}