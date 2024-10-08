using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryDatos_SolRed : RepositoryBase<Datos_SolRed>, IRepositoryDatos_SolRed
    {
		public RepositoryDatos_SolRed(CoreaEntities context)
            : base(context)
        {

        }
    }
}