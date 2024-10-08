using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS : RepositoryBase<T_G_ALERTAS>, IRepositoryT_G_ALERTAS
    {
		public RepositoryT_G_ALERTAS(ModelEntities context)
            : base(context)
        {

        }
    }
}