using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_FiltroSeleccionCentrosCoste : RepositoryBase<SAPHR_FiltroSeleccionCentrosCoste>, IRepositorySAPHR_FiltroSeleccionCentrosCoste
    {
		public RepositorySAPHR_FiltroSeleccionCentrosCoste(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}