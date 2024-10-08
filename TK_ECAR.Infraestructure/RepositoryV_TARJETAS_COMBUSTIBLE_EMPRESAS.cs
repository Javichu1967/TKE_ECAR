using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS : RepositoryBase<V_TARJETAS_COMBUSTIBLE_EMPRESAS>, IRepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS
    {
		public RepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS(ModelEntities context)
            : base(context)
        {

        }
    }
}