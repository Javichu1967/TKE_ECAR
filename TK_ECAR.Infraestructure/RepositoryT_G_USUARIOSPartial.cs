
using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryT_G_USUARIOS
    {

       
        public IQueryable<T_G_USUARIOS> GetUser(string logon)
        {
            return Fetch()
                .Where(x => x.B_ACTIVO)
                .Where(x => x.LOGIN.ToUpper().Equals(logon.ToUpper()));
                
        }

        public IQueryable<T_G_USUARIOS> GetAlertaEmail()
        {
            return Fetch()
                .Where(x => x.B_ACTIVO)
                .Where(x=>x.B_RECIBIR_ALERTA_EMAIL == true);

        }

        public IQueryable<T_G_USUARIOS> GetUsuarioByPerfil(int perfil)
        {
            return Fetch()
                .Where(x => x.B_ACTIVO)
                .Where(x => x.ID_PERFIL == perfil)
                .Where(x=>x.B_RECIBIR_ALERTA_EMAIL == true);

        }


    }
}
