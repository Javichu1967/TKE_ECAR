using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_DireccionesArea : RepositoryBase<SAPHR_DireccionesArea>, IRepositorySAPHR_DireccionesArea
    {
		public RepositorySAPHR_DireccionesArea(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}