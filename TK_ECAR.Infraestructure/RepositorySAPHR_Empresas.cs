using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_Empresas : RepositoryBase<SAPHR_Empresas>, IRepositorySAPHR_Empresas
    {
		public RepositorySAPHR_Empresas(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}