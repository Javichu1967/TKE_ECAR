using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_USUARIOS_DELEGACION : RepositoryBase<T_G_USUARIOS_DELEGACION>, IRepositoryT_G_USUARIOS_DELEGACION
    {
		public RepositoryT_G_USUARIOS_DELEGACION(ModelEntities context)
            : base(context)
        {

        }
    }
}