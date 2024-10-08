using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TK_ECAR.Monitorizacion.ApplicationServices;
using TK_ECAR.Monitorizacion.Global;

namespace TK_ECAR.Monitorizacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            log4net.Config.XmlConfigurator.Configure();
            var monitorizacion = new ProcessMonitorizacion();
            //Application.Run(new Form1());
            try
            {
                //Console.WriteLine("Inicio del proceso generación de alertas...");
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Comienza del proceso generación de alertas... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
                monitorizacion = new ProcessMonitorizacion();
                monitorizacion.Run();
                //Console.WriteLine("Ha finalizado el proceso de generación de alertas...");
            }
            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"PASO [{monitorizacion.Paso}]. {Environment.NewLine} {GlobalApp.GetMessageError(ex)}");
                //ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                //logger.Error(ex);
            }

            finally
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Fijnaliza el proceso generación de alertas... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            }
        }
    }
}
