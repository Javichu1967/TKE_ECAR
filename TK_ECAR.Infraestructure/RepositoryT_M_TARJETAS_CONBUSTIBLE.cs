using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TARJETAS_CONBUSTIBLE : RepositoryBase<T_M_TARJETAS_CONBUSTIBLE>, IRepositoryT_M_TARJETAS_CONBUSTIBLE
    {
		public RepositoryT_M_TARJETAS_CONBUSTIBLE(ModelEntities context)
            : base(context)
        {

        }
    }
}