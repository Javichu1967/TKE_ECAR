using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_Delegaciones : RepositoryBase<SAPHR_Delegaciones>, IRepositorySAPHR_Delegaciones
    {
		public RepositorySAPHR_Delegaciones(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}