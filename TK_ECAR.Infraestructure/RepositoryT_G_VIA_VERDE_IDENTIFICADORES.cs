using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_VIA_VERDE_IDENTIFICADORES : RepositoryBase<T_G_VIA_VERDE_IDENTIFICADORES>, IRepositoryT_G_VIA_VERDE_IDENTIFICADORES
    {
		public RepositoryT_G_VIA_VERDE_IDENTIFICADORES(ModelEntities context)
            : base(context)
        {

        }
    }
}