using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS_RENOVACION_CARNET : RepositoryBase<T_G_ALERTAS_RENOVACION_CARNET>, IRepositoryT_G_ALERTAS_RENOVACION_CARNET
    {
		public RepositoryT_G_ALERTAS_RENOVACION_CARNET(ModelEntities context)
            : base(context)
        {

        }
    }
}