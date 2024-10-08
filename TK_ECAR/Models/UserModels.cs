using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TK_ECAR.Framework;

namespace TK_ECAR.Models
{
    public class UserModel
    {
        public string Login { get; set; }
        public int IdEmpresaUsuario { get; set; }

        public int IdPerfil { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Dni { get; set; }

        public string DescripcionPerfil { get; set; }

        public IEnumerable<CecosModel>  CecosModelList { get; set; }

        public IEnumerable<MenuModel> OpcionesMenu { get; set; }
        public IEnumerable<int> Empresas { get; set; }

    }

    public class  CecosModel
    {
        public int CodigoEmpresa { get; set; }

        public string Empresa { get; set; }

        public string IdDT { get; set; }

        public string DireccionTerritorial { get; set; }

        public string IdDelegacion { get; set; }

        public string Delegacion { get; set; }

        public string IdCeco { get; set; }

        public string Ceco { get; set; }

        public string IdCecoFormatted { get; set; }

        public bool Baja { get; set; }

    }

    public class MenuModel
    {
        public int idMenu { get; set; }

        public string Descripcion { get; set; }

        public int? idMenuPadre { get; set; }
    }

    public class UsuarioSapPeticionAcceso
    {
        public string Nombre { get; set; }

        public string Email { get; set; }
    }

}