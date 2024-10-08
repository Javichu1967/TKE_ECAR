using System.Linq;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositoryT_G_USUARIOS
    {
        IQueryable<T_G_USUARIOS> GetUser(string logon);

        IQueryable<T_G_USUARIOS> GetAlertaEmail();

        IQueryable<T_G_USUARIOS> GetUsuarioByPerfil(int perfil);
    }
}
