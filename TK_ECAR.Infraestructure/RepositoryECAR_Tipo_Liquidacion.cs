using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryECAR_Tipo_Liquidacion : RepositoryBase<ECAR_Tipo_Liquidacion>, IRepositoryECAR_Tipo_Liquidacion
    {
		public RepositoryECAR_Tipo_Liquidacion(ModelEntities context)
            : base(context)
        {

        }
    }
}