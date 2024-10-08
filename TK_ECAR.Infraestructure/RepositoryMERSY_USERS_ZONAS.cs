using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryMERSY_USERS_ZONAS : RepositoryBase<MERSY_USERS_ZONAS>, IRepositoryMERSY_USERS_ZONAS
    {
		public RepositoryMERSY_USERS_ZONAS(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}