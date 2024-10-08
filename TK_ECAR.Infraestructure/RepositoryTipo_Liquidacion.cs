using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryTipo_Liquidacion : RepositoryBase<Tipo_Liquidacion>, IRepositoryTipo_Liquidacion
    {
		public RepositoryTipo_Liquidacion(CoreaEntities context)
            : base(context)
        {

        }
    }
}