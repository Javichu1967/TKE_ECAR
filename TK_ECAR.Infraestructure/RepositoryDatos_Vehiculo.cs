using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryDatos_Vehiculo : RepositoryBase<Datos_Vehiculo>, IRepositoryDatos_Vehiculo
    {
		public RepositoryDatos_Vehiculo(CoreaEntities context)
            : base(context)
        {

        }
    }
}