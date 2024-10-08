using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS_SOLICITUD_SOLRED : RepositoryBase<T_G_ALERTAS_SOLICITUD_SOLRED>, IRepositoryT_G_ALERTAS_SOLICITUD_SOLRED
    {
		public RepositoryT_G_ALERTAS_SOLICITUD_SOLRED(ModelEntities context)
            : base(context)
        {

        }
    }
}