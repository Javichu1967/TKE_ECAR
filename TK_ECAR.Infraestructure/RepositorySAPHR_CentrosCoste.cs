using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_CentrosCoste : RepositoryBase<SAPHR_CentrosCoste>, IRepositorySAPHR_CentrosCoste
    {
		public RepositorySAPHR_CentrosCoste(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}