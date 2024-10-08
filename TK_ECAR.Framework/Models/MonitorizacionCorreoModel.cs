using System;
using System.Collections.Generic;

namespace TK_ECAR.Models
{
    public class MonitorizacionCorreoModel
    {
        public int IdLog { get; set; }

        public string CodigoAplicacion { get; set; }

        public string NombreAplicacion { get; set; }

        public string EmailFrom { get; set; }

        public string EmailTo { get; set; }

        public string AsuntoEmail { get; set; }

        public string CuerpoEmail { get; set; }

        public DateTime FechaEnvioCorreo { get; set; }

        public string LoginUsuario { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public string UrlApp { get; set; }

        public string Path { get; set; } //Ruta física del archivo adjuntado (si lo tuviera)

        public List<string> EmailsCC { get; set; } //Lista de emails con copia

        public List<string> EmailsCCO { get; set; } //Lista de emails con copia oculta

        public string Prioridad { get; set; } //Prioridad del email
    }
}