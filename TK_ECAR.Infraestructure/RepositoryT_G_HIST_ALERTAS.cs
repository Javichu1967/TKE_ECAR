using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_HIST_ALERTAS : RepositoryBase<T_G_HIST_ALERTAS>, IRepositoryT_G_HIST_ALERTAS
    {
		public RepositoryT_G_HIST_ALERTAS(ModelEntities context)
            : base(context)
        {

        }
    }
}