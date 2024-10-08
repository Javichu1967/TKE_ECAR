using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_M_TIPOS_CARBURANTE : RepositoryBase<T_M_TIPOS_CARBURANTE>, IRepositoryT_M_TIPOS_CARBURANTE
    {
		public RepositoryT_M_TIPOS_CARBURANTE(ModelEntities context)
            : base(context)
        {

        }
    }
}