using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryV_ALERTAS_CECOS_DELEGACION : RepositoryBase<V_ALERTAS_CECOS_DELEGACION>, IRepositoryV_ALERTAS_CECOS_DELEGACION
    {
		public RepositoryV_ALERTAS_CECOS_DELEGACION(ModelEntities context)
            : base(context)
        {

        }
    }
}