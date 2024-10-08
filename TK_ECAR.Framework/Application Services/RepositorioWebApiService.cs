using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TK_ECAR.Models;
using TK_ECAR.Framework.Utils;

namespace TK_ECAR.Framework
{
    public class RepositorioWebApiService
    {
        public static string EnviarEmail(MonitorizacionCorreoModel modelCorreo)
        {
            HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

            client.BaseAddress = new Uri($"{System.Configuration.ConfigurationManager.AppSettings["urlRepositorioWebApi"]}/api/MonitorizacionCorreo/GetEnviarEmail");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            System.Threading.Tasks.Task<HttpResponseMessage> response = client.PostAsJsonAsync($"{System.Configuration.ConfigurationManager.AppSettings["urlRepositorioWebApi"]}/api/MonitorizacionCorreo/GetEnviarEmail", modelCorreo);

            string objJson = response.Result.Content.ReadAsStringAsync().Result.ToString().Trim('"');

            string desencriptado = new Encriptar().Desencripta(objJson);

            return desencriptado;
        }

        public static string GuardarLogCorreo(MonitorizacionCorreoModel modelCorreo)
        {
            HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

            client.BaseAddress = new Uri($"{System.Configuration.ConfigurationManager.AppSettings["urlRepositorioWebApi"]}/api/MonitorizacionCorreo/GetGuardarLogCorreo");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            System.Threading.Tasks.Task<HttpResponseMessage> response = client.PostAsJsonAsync($"{System.Configuration.ConfigurationManager.AppSettings["urlRepositorioWebApi"]}/api/MonitorizacionCorreo/GetGuardarLogCorreo", modelCorreo);

            string objJson = response.Result.Content.ReadAsStringAsync().Result.ToString().Trim('"');

            string desencriptado = new Encriptar().Desencripta(objJson);

            return desencriptado;
        }
    }
}