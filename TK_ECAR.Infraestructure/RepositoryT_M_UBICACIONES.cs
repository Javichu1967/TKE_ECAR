using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_UBICACIONES : RepositoryBase<T_M_UBICACIONES>, IRepositoryT_M_UBICACIONES
    {
		public RepositoryT_M_UBICACIONES(ModelEntities context)
            : base(context)
        {

        }
    }
}