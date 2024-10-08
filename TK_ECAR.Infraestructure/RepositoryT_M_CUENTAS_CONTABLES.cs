using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_CUENTAS_CONTABLES : RepositoryBase<T_M_CUENTAS_CONTABLES>, IRepositoryT_M_CUENTAS_CONTABLES
    {
		public RepositoryT_M_CUENTAS_CONTABLES(ModelEntities context)
            : base(context)
        {

        }
    }
}