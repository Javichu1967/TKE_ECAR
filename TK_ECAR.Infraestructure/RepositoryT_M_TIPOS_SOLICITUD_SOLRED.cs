using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPOS_SOLICITUD_SOLRED : RepositoryBase<T_M_TIPOS_SOLICITUD_SOLRED>, IRepositoryT_M_TIPOS_SOLICITUD_SOLRED
    {
		public RepositoryT_M_TIPOS_SOLICITUD_SOLRED(ModelEntities context)
            : base(context)
        {

        }
    }
}