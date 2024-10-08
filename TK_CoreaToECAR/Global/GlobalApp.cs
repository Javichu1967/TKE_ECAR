using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_CoreaToECAR.ApplicationServices
{
    public static class GlobalApp
    {
        public enum TipoDeLog { DEBUG, INFO, ERROR };
        //public static string GLOBAL_PATH_PROCESS_VIA_VERDE_FILES = ConfigurationManager.AppSettings["PATH_ARCHIVOS_PROCESAR_VIAVERDE"].ToString();

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
