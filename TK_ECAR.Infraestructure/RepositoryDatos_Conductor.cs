using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryDatos_Conductor : RepositoryBase<Datos_Conductor>, IRepositoryDatos_Conductor
    {
		public RepositoryDatos_Conductor(CoreaEntities context)
            : base(context)
        {

        }
    }
}