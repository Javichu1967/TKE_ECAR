using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryECAR_Datos_Vehiculo : RepositoryBase<ECAR_Datos_Vehiculo>, IRepositoryECAR_Datos_Vehiculo
    {
		public RepositoryECAR_Datos_Vehiculo(ModelEntities context)
            : base(context)
        {

        }
    }
}