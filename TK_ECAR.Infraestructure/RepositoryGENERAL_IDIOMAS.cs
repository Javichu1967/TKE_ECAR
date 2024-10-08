using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryGENERAL_IDIOMAS : RepositoryBase<GENERAL_IDIOMAS>, IRepositoryGENERAL_IDIOMAS
    {
		public RepositoryGENERAL_IDIOMAS(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}