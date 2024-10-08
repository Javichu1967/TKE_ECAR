using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_R_PERFILES_MENU : RepositoryBase<T_R_PERFILES_MENU>, IRepositoryT_R_PERFILES_MENU
    {
		public RepositoryT_R_PERFILES_MENU(ModelEntities context)
            : base(context)
        {

        }
    }
}