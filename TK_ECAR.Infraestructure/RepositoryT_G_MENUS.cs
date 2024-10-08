using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_MENUS : RepositoryBase<T_G_MENUS>, IRepositoryT_G_MENUS
    {
		public RepositoryT_G_MENUS(ModelEntities context)
            : base(context)
        {

        }
    }
}