using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryECAR_Datos_SolRed : RepositoryBase<ECAR_Datos_SolRed>, IRepositoryECAR_Datos_SolRed
    {
		public RepositoryECAR_Datos_SolRed(ModelEntities context)
            : base(context)
        {

        }
    }
}