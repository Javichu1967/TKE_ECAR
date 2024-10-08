using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryT_G_ALERTAS_ROBO : RepositoryBase<T_G_ALERTAS_ROBO>, IRepositoryT_G_ALERTAS_ROBO
    {
		public RepositoryT_G_ALERTAS_ROBO(ModelEntities context)
            : base(context)
        {

        }
    }
}