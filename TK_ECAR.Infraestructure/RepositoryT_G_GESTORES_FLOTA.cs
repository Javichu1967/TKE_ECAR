using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_GESTORES_FLOTA : RepositoryBase<T_G_GESTORES_FLOTA>, IRepositoryT_G_GESTORES_FLOTA
    {
		public RepositoryT_G_GESTORES_FLOTA(ModelEntities context)
            : base(context)
        {

        }
    }
}