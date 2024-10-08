using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_HIST_CAMBIOS_CONDUCTOR : RepositoryBase<T_G_HIST_CAMBIOS_CONDUCTOR>, IRepositoryT_G_HIST_CAMBIOS_CONDUCTOR
    {
		public RepositoryT_G_HIST_CAMBIOS_CONDUCTOR(ModelEntities context)
            : base(context)
        {

        }
    }
}