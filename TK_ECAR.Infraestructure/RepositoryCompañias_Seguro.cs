using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryCompañias_Seguro : RepositoryBase<Compañias_Seguro>, IRepositoryCompañias_Seguro
    {
		public RepositoryCompañias_Seguro(CoreaEntities context)
            : base(context)
        {

        }
    }
}