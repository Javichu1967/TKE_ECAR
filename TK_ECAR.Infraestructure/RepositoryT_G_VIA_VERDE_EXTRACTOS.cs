using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_VIA_VERDE_EXTRACTOS : RepositoryBase<T_G_VIA_VERDE_EXTRACTOS>, IRepositoryT_G_VIA_VERDE_EXTRACTOS
    {
		public RepositoryT_G_VIA_VERDE_EXTRACTOS(ModelEntities context)
            : base(context)
        {

        }
    }
}