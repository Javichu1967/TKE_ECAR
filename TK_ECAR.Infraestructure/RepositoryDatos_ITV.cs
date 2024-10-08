using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryDatos_ITV : RepositoryBase<Datos_ITV>, IRepositoryDatos_ITV
    {
		public RepositoryDatos_ITV(CoreaEntities context)
            : base(context)
        {

        }
    }
}