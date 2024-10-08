using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ECAR.PortugalImportacion.Global
{
    public static class GlobalApp
    {
        public enum TipoDeLog { DEBUG, INFO, ERROR };

        public static string GLOBAL_PATH_PROCESS_VIA_VERDE_FILES = ConfigurationManager.AppSettings["PATH_ARCHIVOS_PROCESAR_VIAVERDE"].ToString();
        public static string GLOBAL_PATH_PROCESS_GALP_FILES = ConfigurationManager.AppSettings["PATH_ARCHIVOS_PORCESADOS_GALP"].ToString();
        public static string GLOBAL_PATH_PROCESS_LEASEPLAN_FILES = ConfigurationManager.AppSettings["PATH_ARCHIVOS_PROCESAR_LEASEPLAN"].ToString();


        /// <summary>
        /// Devuelve una lista con todos los archivos de un directorio y sus subdirectorios
        /// </summary>
        /// <param name="directory">Directorio que se quiere leer</param>
        /// <param name="filter">Extension de los archivos que se quiere leer. Si es más de una extensión se separa por comas</param>
        /// <param name="includeSubDirectories">Si se incluyen los subdirectorios</param>
        /// <returns></returns>
        public static List<string> GetAllFilesFromDirectory(string directory, string searchPattern = "*.*", bool includeSubDirectories = false)
        {
            List<string> listFiles = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(directory, searchPattern, (includeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                                                        .Where(s => searchPattern.ToLower().Contains(Path.GetExtension(s).ToLower().Replace(".", ""))))
                {
                    listFiles.Add(f);
                }
            }

            catch (Exception ex)
            {
                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.ERROR, "GetAllFilesFromDirectory -> " + ex.Message);
            }

            return listFiles;
        }


        /// <summary>
        /// Devuelve los archivos a procesar, segun el tipo
        /// </summary>
        /// <param name="searchPattern">tipo de archivos a procesar (por ejemplo "xls"). Si es más de una extensión se separa por comas "xls,xlsx"</param>
        /// <returns></returns>
        public static List<string> GetFilesToProcess(string pathFiles, string searchPattern)
        {
            List<string> files = new List<string>(from item in
                                                            GetAllFilesFromDirectory(
                                                                    pathFiles,
                                                                    searchPattern,
                                                                    false).ToArray()
                                                  let file = new FileInfo(item)
                                                  let fileModification = file.LastWriteTime
                                                  orderby fileModification.ToString("yyyyMMddHHmmss")
                                                  select file.Name).ToList();

            return files;

        }


        public static bool EscribeLogApp(TipoDeLog tipolog, string mensaje)
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            bool valorReturn = true;

            try
            {
                switch (tipolog)
                {
                    case TipoDeLog.ERROR:
                        logger.Error(mensaje);
                        break;
                    case TipoDeLog.INFO:
                        logger.Info(mensaje);
                        break;
                    case TipoDeLog.DEBUG:
                        logger.Debug(mensaje);
                        break;
                }
                //}

            }

            catch (Exception ex)
            {

                valorReturn = false;
            }

            return valorReturn;
        }

    }
}
