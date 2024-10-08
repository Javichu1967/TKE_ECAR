using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryECAR_Datos_ITV : RepositoryBase<ECAR_Datos_ITV>, IRepositoryECAR_Datos_ITV
    {
		public RepositoryECAR_Datos_ITV(ModelEntities context)
            : base(context)
        {

        }
    }
}