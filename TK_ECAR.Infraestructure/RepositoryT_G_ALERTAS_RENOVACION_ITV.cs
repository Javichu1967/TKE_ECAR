using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS_RENOVACION_ITV : RepositoryBase<T_G_ALERTAS_RENOVACION_ITV>, IRepositoryT_G_ALERTAS_RENOVACION_ITV
    {
		public RepositoryT_G_ALERTAS_RENOVACION_ITV(ModelEntities context)
            : base(context)
        {

        }
    }
}