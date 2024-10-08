using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryTMP_RENOVACION_ITV : RepositoryBase<TMP_RENOVACION_ITV>, IRepositoryTMP_RENOVACION_ITV
    {
		public RepositoryTMP_RENOVACION_ITV(ModelEntities context)
            : base(context)
        {

        }
    }
}