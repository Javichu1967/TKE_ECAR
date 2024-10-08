using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_DOCUMENTACION : RepositoryBase<T_G_DOCUMENTACION>, IRepositoryT_G_DOCUMENTACION
    {
		public RepositoryT_G_DOCUMENTACION(ModelEntities context)
            : base(context)
        {

        }
    }
}