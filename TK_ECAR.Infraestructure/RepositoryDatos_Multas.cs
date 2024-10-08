using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryDatos_Multas : RepositoryBase<Datos_Multas>, IRepositoryDatos_Multas
    {
		public RepositoryDatos_Multas(CoreaEntities context)
            : base(context)
        {

        }
    }
}