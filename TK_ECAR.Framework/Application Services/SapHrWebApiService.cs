using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TK_ECAR.Models;

namespace TK_ECAR.Framework.Application_Services
{
    public class SapHrWebApiService
    {
        public class FuncionModel
        {
            public int IdFuncion { get; set; }
            public string Funcion { get; set; }
        }

        /// <summary>
        ///  Obtiene el usuario de SAP y datos relacionados a partir de los parámetros utilizados como filtro,en este caso por nombres de usuario(logon). 
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <param name="login"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public FuncionModel obtenerFuncionPersonal(string nombreUsuario, string login, string fecha)
        {

            //Añadimos los parámetros de búsqueda
            StringBuilder webUrl = new StringBuilder(System.Configuration.ConfigurationManager.AppSettings["urlApiSAPHR"]);
            webUrl.Append("?");
            webUrl.Append("tipoRespuesta=1");
            webUrl.Append("&modoConsulta=0");
            webUrl.Append("&nombresUsuario=" + nombreUsuario);
            webUrl.Append("&login=" + login);
            webUrl.Append("&fecha=" + fecha);

            string jsonObject = string.Empty;
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            {
                httpClient.BaseAddress = new Uri(webUrl.ToString());
                httpClient.DefaultRequestHeaders.Accept.Clear();

                // Agrega el header Accept: application/json para recibir la data como json  
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                System.Threading.Tasks.Task<string> response = httpClient.GetStringAsync(webUrl.ToString());

                // Esperamos a que llegue la respuesta a la invocación
                while (!response.IsCompleted && !response.IsFaulted && !response.IsCanceled)
                { continue; }

                string objJson = response.Result;
                List<FuncionesSAPHRModel> listado = JsonConvert.DeserializeObject<List<FuncionesSAPHRModel>>(objJson.ToString());

                return listado.Select(x => new FuncionModel
                {
                    IdFuncion = x.IdFuncion,
                    Funcion = x.Funcion,
                }).FirstOrDefault();
            }
        }

        public string obtenerCorreoUsuario(string idUsuario)
        {
            //Añadimos los parámetros de búsqueda
            StringBuilder webUrl = new StringBuilder(System.Configuration.ConfigurationManager.AppSettings["urlApiSAPHR"]);
            webUrl.Append("?");
            webUrl.Append("tipoRespuesta=1");
            webUrl.Append("&modoConsulta=0");
            webUrl.Append("&nombresUsuario=" + idUsuario);
            webUrl.Append("&login=" + idUsuario);
            webUrl.Append("&fecha=" + DateTime.Now.ToString("yyyyMMdd"));

            string jsonObject = string.Empty;
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            {
                httpClient.BaseAddress = new Uri(webUrl.ToString());
                httpClient.DefaultRequestHeaders.Accept.Clear();

                // Agrega el header Accept: application/json para recibir la data como json  
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                System.Threading.Tasks.Task<string> response = httpClient.GetStringAsync(webUrl.ToString());

                // Esperamos a que llegue la respuesta a la invocación
                while (!response.IsCompleted && !response.IsFaulted && !response.IsCanceled)
                { continue; }

                string objJson = response.Result;
                List<FuncionesSAPHRModel> listado = JsonConvert.DeserializeObject<List<FuncionesSAPHRModel>>(objJson.ToString());

                return listado.Select(x => x.Email).FirstOrDefault();
            }
        }
    }
}