using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPOS_VEHICULO : RepositoryBase<T_M_TIPOS_VEHICULO>, IRepositoryT_M_TIPOS_VEHICULO
    {
		public RepositoryT_M_TIPOS_VEHICULO(ModelEntities context)
            : base(context)
        {

        }
    }
}