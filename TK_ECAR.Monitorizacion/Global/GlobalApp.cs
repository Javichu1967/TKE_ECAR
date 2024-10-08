using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ECAR.Monitorizacion.Global
{
    public static class GlobalApp
    {
        public enum TipoDeLog { DEBUG, INFO, ERROR };

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


        public static string GetMessageError(Exception ex)
        {
            string valorReturn = string.Empty;

            valorReturn = ex.Message;
            if (ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                {
                    if (!string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                    {
                        valorReturn = ex.InnerException.InnerException.Message;
                    }
                }
            }


            return valorReturn;
        }

    }
}
