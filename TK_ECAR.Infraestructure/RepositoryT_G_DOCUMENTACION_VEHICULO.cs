using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_DOCUMENTACION_VEHICULO : RepositoryBase<T_G_DOCUMENTACION_VEHICULO>, IRepositoryT_G_DOCUMENTACION_VEHICULO
    {
		public RepositoryT_G_DOCUMENTACION_VEHICULO(ModelEntities context)
            : base(context)
        {

        }
    }
}