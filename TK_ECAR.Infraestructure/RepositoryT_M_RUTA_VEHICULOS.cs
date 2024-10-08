using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_RUTA_VEHICULOS : RepositoryBase<T_M_RUTA_VEHICULOS>, IRepositoryT_M_RUTA_VEHICULOS
    {
		public RepositoryT_M_RUTA_VEHICULOS(ModelEntities context)
            : base(context)
        {

        }
    }
}