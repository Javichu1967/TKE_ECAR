using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_UnidadesOrganizativas : RepositoryBase<SAPHR_UnidadesOrganizativas>, IRepositorySAPHR_UnidadesOrganizativas
    {
		public RepositorySAPHR_UnidadesOrganizativas(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}