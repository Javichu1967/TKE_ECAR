using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_TARJETA_COMBUSTIBLE : RepositoryBase<T_G_TARJETA_COMBUSTIBLE>, IRepositoryT_G_TARJETA_COMBUSTIBLE
    {
		public RepositoryT_G_TARJETA_COMBUSTIBLE(ModelEntities context)
            : base(context)
        {

        }
    }
}