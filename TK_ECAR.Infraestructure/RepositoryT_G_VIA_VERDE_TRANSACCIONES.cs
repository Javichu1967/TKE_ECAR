using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_VIA_VERDE_TRANSACCIONES : RepositoryBase<T_G_VIA_VERDE_TRANSACCIONES>, IRepositoryT_G_VIA_VERDE_TRANSACCIONES
    {
		public RepositoryT_G_VIA_VERDE_TRANSACCIONES(ModelEntities context)
            : base(context)
        {

        }
    }
}