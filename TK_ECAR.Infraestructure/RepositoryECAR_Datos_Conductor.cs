using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryECAR_Datos_Conductor : RepositoryBase<ECAR_Datos_Conductor>, IRepositoryECAR_Datos_Conductor
    {
		public RepositoryECAR_Datos_Conductor(ModelEntities context)
            : base(context)
        {

        }
    }
}