using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryEmpresas_Leasing : RepositoryBase<Empresas_Leasing>, IRepositoryEmpresas_Leasing
    {
		public RepositoryEmpresas_Leasing(CoreaEntities context)
            : base(context)
        {

        }
    }
}