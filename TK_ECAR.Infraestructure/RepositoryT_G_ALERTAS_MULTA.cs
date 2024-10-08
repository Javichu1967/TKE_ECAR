using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS_MULTA : RepositoryBase<T_G_ALERTAS_MULTA>, IRepositoryT_G_ALERTAS_MULTA
    {
		public RepositoryT_G_ALERTAS_MULTA(ModelEntities context)
            : base(context)
        {

        }
    }
}