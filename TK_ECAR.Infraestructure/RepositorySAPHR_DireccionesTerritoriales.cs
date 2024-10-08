using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_DireccionesTerritoriales : RepositoryBase<SAPHR_DireccionesTerritoriales>, IRepositorySAPHR_DireccionesTerritoriales
    {
		public RepositorySAPHR_DireccionesTerritoriales(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}