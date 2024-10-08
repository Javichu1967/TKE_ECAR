using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPOS_COSTE : RepositoryBase<T_M_TIPOS_COSTE>, IRepositoryT_M_TIPOS_COSTE
    {
		public RepositoryT_M_TIPOS_COSTE(ModelEntities context)
            : base(context)
        {

        }
    }
}