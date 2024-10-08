using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_CoreaToECAR.ApplicationServices;

namespace TK_CoreaToECAR
{
    class Program
    {
        static void Main(string[] args)
        {
            ImportFromCOREA.ImportaDatosVehiculoFromCorea();
            ImportFromCOREA.ImportaDatosLeasePLanFromCorea();
            ImportFromCOREA.ImportaDatosSOLREDFromCorea();
        }
    }
}
