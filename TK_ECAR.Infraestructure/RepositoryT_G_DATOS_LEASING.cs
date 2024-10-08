using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_DATOS_LEASING : RepositoryBase<T_G_DATOS_LEASING>, IRepositoryT_G_DATOS_LEASING
    {
		public RepositoryT_G_DATOS_LEASING(ModelEntities context)
            : base(context)
        {

        }
    }
}