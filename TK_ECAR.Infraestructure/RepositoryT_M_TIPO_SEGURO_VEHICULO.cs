using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPO_SEGURO_VEHICULO : RepositoryBase<T_M_TIPO_SEGURO_VEHICULO>, IRepositoryT_M_TIPO_SEGURO_VEHICULO
    {
		public RepositoryT_M_TIPO_SEGURO_VEHICULO(ModelEntities context)
            : base(context)
        {

        }
    }
}