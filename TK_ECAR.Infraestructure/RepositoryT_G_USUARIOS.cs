using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_USUARIOS : RepositoryBase<T_G_USUARIOS>, IRepositoryT_G_USUARIOS
    {
		public RepositoryT_G_USUARIOS(ModelEntities context)
            : base(context)
        {

        }
    }
}