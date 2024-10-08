using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_MARCA_VEHICULOS : RepositoryBase<T_M_MARCA_VEHICULOS>, IRepositoryT_M_MARCA_VEHICULOS
    {
		public RepositoryT_M_MARCA_VEHICULOS(ModelEntities context)
            : base(context)
        {

        }
    }
}