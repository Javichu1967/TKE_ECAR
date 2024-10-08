using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR : RepositoryBase<T_G_ALERTAS_CAMBIO_CONDUCTOR>, IRepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR
    {
		public RepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR(ModelEntities context)
            : base(context)
        {

        }
    }
}