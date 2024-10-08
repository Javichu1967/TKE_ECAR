using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_USUARIOS_CECO : RepositoryBase<T_G_USUARIOS_CECO>, IRepositoryT_G_USUARIOS_CECO
    {
		public RepositoryT_G_USUARIOS_CECO(ModelEntities context)
            : base(context)
        {

        }
    }
}