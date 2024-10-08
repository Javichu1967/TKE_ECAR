using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES : RepositoryBase<T_G_ALERTAS_OTRAS_NOTIFICACIONES>, IRepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES
    {
		public RepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES(ModelEntities context)
            : base(context)
        {

        }
    }
}