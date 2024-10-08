
using System;
using System.Collections.Generic;
using System.Linq;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositorySAPHR_UsuariosSAP
    {

       public bool ExistUserInSAP(string logon)
        {
            return Fetch()
                .Where(x => x.Logon.ToUpper().Equals(logon.ToUpper()))
                .Where(x=>!x.Baja)
                .Any();
        }


        /// <summary>
        /// Devuelve de los logon recibidos los existentes en SAP en MAYÚSCULAS 
        /// </summary>
        /// <param name="logins"></param>
        /// <returns></returns>
        public List<string> ReturnLoginExistInSAP(IEnumerable<string> logins)
        {
             

            var loginsToUpper = logins.Select(x => x.ToUpper()).ToList();
             
            return Fetch()
                .Where(x => loginsToUpper.Contains( x.Logon.ToUpper() ))
                .Where(x => !x.Baja)
                .Select(x=>x.Logon.ToUpper())
                .ToList();
        }
    }
}
