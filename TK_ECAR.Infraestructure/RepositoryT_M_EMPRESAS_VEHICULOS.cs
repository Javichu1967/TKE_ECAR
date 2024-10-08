using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_EMPRESAS_VEHICULOS : RepositoryBase<T_M_EMPRESAS_VEHICULOS>, IRepositoryT_M_EMPRESAS_VEHICULOS
    {
		public RepositoryT_M_EMPRESAS_VEHICULOS(ModelEntities context)
            : base(context)
        {

        }
    }
}