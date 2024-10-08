using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryMERSY_ZONAS : RepositoryBase<MERSY_ZONAS>, IRepositoryMERSY_ZONAS
    {
		public RepositoryMERSY_ZONAS(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}