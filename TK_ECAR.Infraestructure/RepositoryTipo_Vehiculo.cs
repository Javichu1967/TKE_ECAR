using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryTipo_Vehiculo : RepositoryBase<Tipo_Vehiculo>, IRepositoryTipo_Vehiculo
    {
		public RepositoryTipo_Vehiculo(CoreaEntities context)
            : base(context)
        {

        }
    }
}