using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE : RepositoryBase<T_M_TIPO_TARJETA_COMBUSTIBLE>, IRepositoryT_M_TIPO_TARJETA_COMBUSTIBLE
    {
		public RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE(ModelEntities context)
            : base(context)
        {

        }
    }
}