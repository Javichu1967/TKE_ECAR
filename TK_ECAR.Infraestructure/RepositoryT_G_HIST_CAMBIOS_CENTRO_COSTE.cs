using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE : RepositoryBase<T_G_HIST_CAMBIOS_CENTRO_COSTE>, IRepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE
    {
		public RepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE(ModelEntities context)
            : base(context)
        {

        }
    }
}