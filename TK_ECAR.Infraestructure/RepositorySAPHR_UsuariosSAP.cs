using TK_ECAR.Domain;
 
    
namespace TK_ECAR.Infraestructure
{   
    public partial class RepositorySAPHR_UsuariosSAP : RepositoryBase<SAPHR_UsuariosSAP>, IRepositorySAPHR_UsuariosSAP
    {
		public RepositorySAPHR_UsuariosSAP(TKRepositorioEntities context)
            : base(context)
        {

        }
    }
}