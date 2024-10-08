using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_PERFILES : RepositoryBase<T_M_PERFILES>, IRepositoryT_M_PERFILES
    {
		public RepositoryT_M_PERFILES(ModelEntities context)
            : base(context)
        {

        }
    }
}