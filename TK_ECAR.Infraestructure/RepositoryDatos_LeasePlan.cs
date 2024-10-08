using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryDatos_LeasePlan : RepositoryBase<Datos_LeasePlan>, IRepositoryDatos_LeasePlan
    {
		public RepositoryDatos_LeasePlan(CoreaEntities context)
            : base(context)
        {

        }
    }
}