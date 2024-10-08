using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ECAR.ImportacionFlota.Global
{
    public static class GlobalApp
    {
        public const int COL_OBSERVACIONES = 1;
        public const int COL_NOMBRE_DELEGACION = 2;
        public const int COL_CENTRO_COSTE = 3;
        public const int COL_DIRECCION = 4;
        public const int COL_RESPONSABLE = 5;
        public const int COL_MAIL = 6;
        public const int COL_TELEFONO = 7;
        public const int COL_LUGAR_ENTREGA = 8;
        public const int COL_CONCESIONARIO = 9;
        public const int COL_PRIORIDAD_ENTREGA = 10;
        public const int COL_FECHA_RENOVACION = 11;
        public const int COL_FECHA_RECOGIDA = 12;
        public const int COL_FECHA_ALTA = 13;
        public const int COL_CONTRATO = 14;
        public const int COL_CUOTAS = 15;
        public const int COL_KM_TOTALES = 16;
        public const int COL_COSTE_KM_EXC_DEF = 17; //VIENE SEPARADO POR "/"
        public const int COL_ID_TIPO_VEHICULO = 18;
        public const int COL_TIPO_VEHICULO = 19;
        public const int COL_ID_MARCA_VEHICULO = 20;
        public const int COL_ID_MODELO = 21;
        public const int COL_MODELO = 22;
        public const int COL_EXTRAS = 23;
        public const int COL_EQUIPAMIENTO = 24;
        public const int COL_MATRICULA = 25;
        public const int COL_BASTIDOR = 26;
        public const int COL_FECHA_MATRICULACION = 27;
        public const int COL_NUM_EMPLEADO = 28;
        public const int COL_APELLIDOS_CONDUCTOR = 29;
        public const int COL_NOMBRE_CONDUCTOR = 30;
        public const int COL_TELEFONO_CONDUCTOR = 31;
        public const int COL_DNI = 32;
        public const int COL_CALLE_POBLACION = 33;
        public const int COL_POBLACION = 34;
        public const int COL_CP = 35;
        public const int COL_FECHA_NACIMIENTO = 36;
        public const int COL_FECHA_EXPEDICION = 37;
        public const int COL_MATRICULA_SUSTITUYE = 38;
        public const int COL_POLIZA = 39;
        public const int COL_IMPORTE_SEGURO = 40;
        public const int COL_FECHA_VTO_SEGURO = 41;
        public const int COL_ID_TIPO_SEGURO = 42;
        public const int COL_TIPO_SEGURO = 43;
        public const int COL_INCIDENCIA = 44; //ESTA SE AÑADIRÁ PARA PONER UN COMENTARIO SI HAY INCIDENCIA.

        public const int AÑOS_ITV_FURGONETA = 2;
        public const int AÑOS_ITV_RESTO = 4;

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
