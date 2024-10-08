using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_TELEFONOS_FRECUENTES : RepositoryBase<T_G_TELEFONOS_FRECUENTES>, IRepositoryT_G_TELEFONOS_FRECUENTES
    {
		public RepositoryT_G_TELEFONOS_FRECUENTES(ModelEntities context)
            : base(context)
        {

        }
    }
}