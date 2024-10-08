using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ClassCostes
{
    public sealed class GlobalCostes
    {
        public enum TipoObjetoCoste { Alquiler, Mantenimiento, Neumaticos, Administracion, Seguro, ITV };

        private static Dictionary<TipoObjetoCoste, CuentaContable> conversor_TipoObjetoCoste_CuentaContable = new Dictionary<TipoObjetoCoste, CuentaContable>();

        public GlobalCostes()
        {
            conversor_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Administracion, new CuentaContable("62391100", "Servicio Admon. Flotas"));
            conversor_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Alquiler, new CuentaContable("62104000", "Alquiler Flota Vehículos"));
            conversor_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.ITV, new CuentaContable("62962001", "I.T.V."));
            conversor_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Mantenimiento, new CuentaContable("62201007", "R-C Elementos Tpte."));
            conversor_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Neumaticos, new CuentaContable("62201007", "R-C Elementos Tpte."));
            conversor_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Seguro, new CuentaContable("62502001", "Seguros (imputable)"));
        }

        public string GetNombreCuentaContableAsociada(TipoObjetoCoste tipoCoste)
        {
            CuentaContable valorCuenta = new CuentaContable();
            valorCuenta = conversor_TipoObjetoCoste_CuentaContable[tipoCoste];

            return valorCuenta.NombreCuentaContable;
        }

        public string GetCodigoCuentaContableAsociada(TipoObjetoCoste tipoCoste)
        {
            CuentaContable valorCuenta = new CuentaContable();
            valorCuenta = conversor_TipoObjetoCoste_CuentaContable[tipoCoste];

            return valorCuenta.CodigoCuentaContable;
        }

        class CuentaContable
        {
            public CuentaContable()
            {
            }

            public CuentaContable(string _codigoCuentaContable, string _nombreCuentaContable)
            {
                CodigoCuentaContable = _codigoCuentaContable;
                NombreCuentaContable = _nombreCuentaContable;
            }
            public string CodigoCuentaContable { get; set; }
            public string NombreCuentaContable { get; set; }
        }
    }
}
