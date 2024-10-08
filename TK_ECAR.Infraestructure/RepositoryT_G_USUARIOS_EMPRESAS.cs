using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_USUARIOS_EMPRESAS : RepositoryBase<T_G_USUARIOS_EMPRESAS>, IRepositoryT_G_USUARIOS_EMPRESAS
    {
		public RepositoryT_G_USUARIOS_EMPRESAS(ModelEntities context)
            : base(context)
        {

        }
    }
}