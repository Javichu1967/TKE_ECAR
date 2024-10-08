using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_ECAR.ImportacionFlota.ApplicationServices;

namespace TK_ECAR.ImportacionFlota
{
    class Program
    {
        public static void Main(string[] args)
        {

            var processImportFlota = new ImportFlota();

            processImportFlota.EventProcesingImportFlota += OnGetEventProcesingImportFlota;
            processImportFlota.EventErrorImportFlota += OnGetEventErrorImportFlota;
            processImportFlota.EventFinishedImportFlota += OnGetEventFinishedImportFlota;

            processImportFlota.startImport("nombreArchivoImportarFlota", 8100);
            //Suscribimos el evento.

        }

        #region Eventos importación flota
        private static void OnGetEventProcesingImportFlota(int lineProsessing, int TotalLinesToProcess)
        {
        }

        private static void OnGetEventErrorImportFlota(int lineaProsessing, string msgError)
        {
        }

        private static void OnGetEventFinishedImportFlota(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalLinesProcessedERROR)
        {
        }
        #endregion

    }
}
