using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{
    public class ClientesModels
    {
        public string ID_CLIENTE { get; set; }
        public string NIF { get; set; }
        public string NOMBRE { get; set; }
        public string DIRECCION { get; set; }
        public string LOCALIDAD { get; set; }
        public string CODIGO_POSTAL { get; set; }
    }

}
