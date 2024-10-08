using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_HIST_CAMBIOS_EMPRESA : RepositoryBase<T_G_HIST_CAMBIOS_EMPRESA>, IRepositoryT_G_HIST_CAMBIOS_EMPRESA
    {
		public RepositoryT_G_HIST_CAMBIOS_EMPRESA(ModelEntities context)
            : base(context)
        {

        }
    }
}