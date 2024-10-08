using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryTipo_Seguro : RepositoryBase<Tipo_Seguro>, IRepositoryTipo_Seguro
    {
		public RepositoryTipo_Seguro(CoreaEntities context)
            : base(context)
        {

        }
    }
}