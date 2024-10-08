using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryECAR_Datos_Multas : RepositoryBase<ECAR_Datos_Multas>, IRepositoryECAR_Datos_Multas
    {
		public RepositoryECAR_Datos_Multas(ModelEntities context)
            : base(context)
        {

        }
    }
}