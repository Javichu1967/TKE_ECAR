using System.Collections.Generic;

namespace TK_ECAR.Domain
{
    public  partial interface IRepositorySAPHR_UsuariosSAP
    {
        bool ExistUserInSAP(string logon);

        List<string> ReturnLoginExistInSAP(IEnumerable<string> logins);
    }
}
