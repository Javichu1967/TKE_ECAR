using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE : RepositoryBase<T_R_TIPOSCOSTE_CUENTA_CONTABLE>, IRepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE
    {
		public RepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE(ModelEntities context)
            : base(context)
        {

        }
    }
}