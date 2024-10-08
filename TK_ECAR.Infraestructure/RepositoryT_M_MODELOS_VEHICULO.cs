using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_MODELOS_VEHICULO : RepositoryBase<T_M_MODELOS_VEHICULO>, IRepositoryT_M_MODELOS_VEHICULO
    {
		public RepositoryT_M_MODELOS_VEHICULO(ModelEntities context)
            : base(context)
        {

        }
    }
}