using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_USUARIOS_DIR_TERRITORIAL : RepositoryBase<T_G_USUARIOS_DIR_TERRITORIAL>, IRepositoryT_G_USUARIOS_DIR_TERRITORIAL
    {
		public RepositoryT_G_USUARIOS_DIR_TERRITORIAL(ModelEntities context)
            : base(context)
        {

        }
    }
}