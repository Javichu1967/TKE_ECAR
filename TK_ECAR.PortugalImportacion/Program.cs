using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TK_ECAR.PortugalImportacion.ApplicationServices;
using TK_ECAR.PortugalImportacion.Global;
using TK_ECAR.PortugalImportacion.Models;

namespace TK_ECAR.PortugalImportacion
{
    static class Program
    {
        private static string Paso = "";
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {

                ProcessViaVerde();

                ProcessGALP();

                ProcessLEASEPLAN();

            }
            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<Proceso importación Via Verde...> {Paso}, {ex.Message}");
            }
            finally
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Finaliza del proceso importación ...> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            }
        }

        #region ViaVerde
        private static void ProcessViaVerde()
        {
            Console.WriteLine("Inicio del proceso importación Via Verde...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Inicio del proceso importación Via Verde...> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");

            List<string> filesViaVerdeToProcess = GlobalApp.GetFilesToProcess(GlobalApp.GLOBAL_PATH_PROCESS_VIA_VERDE_FILES, "*.xml");

            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Nº de archivos a importar Via Verde > {filesViaVerdeToProcess.Count()}");

            int cont = 0;
            foreach (string viaVerdeFile in filesViaVerdeToProcess)
            {
                try
                {
                    Paso = $"ProcessViaVerde {viaVerdeFile}";
                    cont++;
                    Console.WriteLine($"<Inicio Archivo {cont} de {filesViaVerdeToProcess.Count()} ... {viaVerdeFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Inicio Archivo {cont} de {filesViaVerdeToProcess.Count()} ... {viaVerdeFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                    var importarPortugal = new ImportacionPortugal();

                    EXTRACTO DatosImportar = importarPortugal.SerializeViaVerde(GlobalApp.GLOBAL_PATH_PROCESS_VIA_VERDE_FILES + viaVerdeFile);
                    importarPortugal.Actualiza_E_CAR(DatosImportar);
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Fin Archivo {cont} de {filesViaVerdeToProcess.Count()} ... {viaVerdeFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                }
                catch (Exception ex)
                {
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ProcessViaVerde...> {Paso}, {ex.Message}");
                }
            }
            Console.WriteLine("Ha finalizado el proceso de importación Via Verde...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Finalizado el proceso de importación Via Verde... {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");
        }
        #endregion

        #region GALP
        private static void ProcessGALP()
        {
            Console.WriteLine("Inicio del proceso importación GALP...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Inicio del proceso importación GALP...> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");

            List<string> filesGALPToProcess = GlobalApp.GetFilesToProcess(GlobalApp.GLOBAL_PATH_PROCESS_GALP_FILES, "*.xlsx");

            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Nº de archivos a importar GALP > {filesGALPToProcess.Count()}");

            int cont = 0;
            foreach (string GALPFile in filesGALPToProcess)
            {
                try
                {
                    Paso = $"ProcessGALP {GALPFile}";
                    cont++;
                    Console.WriteLine($"<Inicio Archivo {cont} de {filesGALPToProcess.Count()} ... {GALPFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Inicio Archivo {cont} de {filesGALPToProcess.Count()} ... {GALPFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                    var importarPortugal = new ImportacionPortugal();

                    importarPortugal.ImportaDatosGALP(GlobalApp.GLOBAL_PATH_PROCESS_GALP_FILES + GALPFile);
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Fin Archivo {cont} de {filesGALPToProcess.Count()} ... {GALPFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                }
                catch (Exception ex)
                {
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ProcessGALP...> {Paso}, {ex.Message}");
                }
            }
            Console.WriteLine("Ha finalizado el proceso de importación GALP...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Finalizado el proceso de importación GALP... {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");
        }
        #endregion

        #region LEASEPLAN
        private static void ProcessLEASEPLAN()
        {
            Console.WriteLine("Inicio del proceso importación LEASEPLAN...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Inicio del proceso importación LEASEPLAN...> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");

            List<string> filesLEASEPLANToProcess = GlobalApp.GetFilesToProcess(GlobalApp.GLOBAL_PATH_PROCESS_LEASEPLAN_FILES, "*.txt");

            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Nº de archivos a importar LEASEPLAN > {filesLEASEPLANToProcess.Count()}");

            int cont = 0;
            foreach (string LEASEPLANFile in filesLEASEPLANToProcess)
            {
                try
                {
                    Paso = $"ProcessLEASEPLAN {LEASEPLANFile}";
                    cont++;
                    Console.WriteLine($"<Inicio Archivo {cont} de {filesLEASEPLANToProcess.Count()} ... {LEASEPLANFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Inicio Archivo {cont} de {filesLEASEPLANToProcess.Count()} ... {LEASEPLANFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                    var importarPortugal = new ImportacionPortugal();

                    importarPortugal.ImportaDatosLEASEPLAN(GlobalApp.GLOBAL_PATH_PROCESS_LEASEPLAN_FILES + LEASEPLANFile);
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Fin Archivo {cont} de {filesLEASEPLANToProcess.Count()} ... {LEASEPLANFile}> {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
                }
                catch (Exception ex)
                {
                    GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, $"<ProcessLEASEPLAN...> {Paso}, {ex.Message}");
                }
            }
            Console.WriteLine("Ha finalizado el proceso de importación LEASEPLAN...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"<Finalizado el proceso de importación LEASEPLAN... {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"********************************************************************************************");
        }
        #endregion
    }
}