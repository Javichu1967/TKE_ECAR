using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryGENERAL_PARAMETROS_APLICACIONES : RepositoryBase<GENERAL_PARAMETROS_APLICACIONES>, IRepositoryGENERAL_PARAMETROS_APLICACIONES
    {
		public RepositoryGENERAL_PARAMETROS_APLICACIONES(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}