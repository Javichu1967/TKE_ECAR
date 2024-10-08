using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositoryV_CONDUCTORES_USUARIOS_SAP : RepositoryBase<V_CONDUCTORES_USUARIOS_SAP>, IRepositoryV_CONDUCTORES_USUARIOS_SAP
    {
		public RepositoryV_CONDUCTORES_USUARIOS_SAP(ModelEntities context)
            : base(context)
        {

        }
    }
}