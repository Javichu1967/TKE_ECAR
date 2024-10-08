using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE : RepositoryBase<T_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE>, IRepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE
    {
		public RepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE(ModelEntities context)
            : base(context)
        {

        }
    }
}