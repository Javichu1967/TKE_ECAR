using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_HIST_CAMBIOS_TARJETA : RepositoryBase<T_G_HIST_CAMBIOS_TARJETA>, IRepositoryT_G_HIST_CAMBIOS_TARJETA
    {
		public RepositoryT_G_HIST_CAMBIOS_TARJETA(ModelEntities context)
            : base(context)
        {

        }
    }
}