using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Infraestructure;

namespace TK_ECAR.Framework.Utils
{
    public sealed class GlobalCostes
    {
        public enum TipoObjetoCoste
        {
            Administracion = 1,
            Alquiler = 2,
            ITV = 3,
            Mantenimiento = 4,
            Neumaticos = 5,
            Seguro = 6
        };

        public const string CENTRO_COSTE_FACTURA = "109110";

        public const int VEHICULO_DIRECCION = 8;

        public const string TEXTO_SIN_DELEGACION = "Sin delegación";

        //private static Dictionary<TipoObjetoCoste, CuentaContable> relacion_TipoObjetoCoste_CuentaContable = new Dictionary<TipoObjetoCoste, CuentaContable>();
        private static Dictionary<int, Dictionary<TipoObjetoCoste, CuentaContable>> conversor_TipoObjetoCoste_CuentaContable = new Dictionary<int, Dictionary<TipoObjetoCoste, CuentaContable>>();

        private static void InicializaConversor_TipoObjetoCoste_CuentaContable()
        {
            if (conversor_TipoObjetoCoste_CuentaContable.Count == 0)
            {
                conversor_TipoObjetoCoste_CuentaContable = GetDictionary_TipoObjetoCoste_CuentaContable();
                //relacion_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Administracion, new CuentaContable("62391100", "Servicio Admon. Flotas"));
                //relacion_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Alquiler, new CuentaContable("62104000", "Alquiler Flota Vehículos"));
                //relacion_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.ITV, new CuentaContable("62962001", "I.T.V."));
                //relacion_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Mantenimiento, new CuentaContable("62201007", "R-C Elementos Tpte."));
                //relacion_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Neumaticos, new CuentaContable("62201007", "R-C Elementos Tpte."));
                //relacion_TipoObjetoCoste_CuentaContable.Add(TipoObjetoCoste.Seguro, new CuentaContable("62502001", "Seguros (imputable)"));
            }
        }

        public static string GetNombreCuentaContableAsociada(int empresaFacturada, TipoObjetoCoste tipoCoste)
        {
            InicializaConversor_TipoObjetoCoste_CuentaContable();

            CuentaContable valorCuenta = new CuentaContable();
            valorCuenta = conversor_TipoObjetoCoste_CuentaContable[empresaFacturada][tipoCoste];

            return valorCuenta.NombreCuentaContable;
        }

        public static string GetCodigoCuentaContableAsociada(int empresaFacturada, TipoObjetoCoste tipoCoste)
        {
            InicializaConversor_TipoObjetoCoste_CuentaContable();

            CuentaContable valorCuenta = new CuentaContable();
            valorCuenta = conversor_TipoObjetoCoste_CuentaContable[empresaFacturada][tipoCoste];

            return valorCuenta.CodigoCuentaContable;
        }

        public static List<TipoObjetoCoste> GetTiposCosteCuentaContableAsociada(int empresa, string cuenta)
        {
            List<TipoObjetoCoste> costes = new List<TipoObjetoCoste>();
            foreach(TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
            {
                if (GetCodigoCuentaContableAsociada(empresa, tCoste) == cuenta)
                {
                    costes.Add(tCoste);
                }
            }

            return costes;
        }

        public class CuentaContable
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

        private static Dictionary<int, Dictionary<TipoObjetoCoste, CuentaContable>> GetDictionary_TipoObjetoCoste_CuentaContable()
        {
            Dictionary<int, Dictionary<TipoObjetoCoste, CuentaContable>> conversor = new Dictionary<int, Dictionary<TipoObjetoCoste, CuentaContable>>();
            Dictionary<TipoObjetoCoste, CuentaContable> relacion_TipoObjetoCoste_CuentaContable = new Dictionary<TipoObjetoCoste, CuentaContable>();
            var cuenta = new CuentaContable();

            using (var unitOfWork = new UnitOfWork())
            {
                int empresaAnt = 0;
                foreach (T_R_TIPOSCOSTE_CUENTA_CONTABLE relacion in unitOfWork.RepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE
                                                                .Fetch().OrderBy(X => X.IDEMPRESA).ThenBy(X => X.ID_TIPOCOSTE))
                {
                    cuenta = new CuentaContable(relacion.CUENTA_CONTABLE, relacion.T_M_CUENTAS_CONTABLES.NOMBRE_CUENTA);

                    if (empresaAnt != relacion.IDEMPRESA)
                    {
                        relacion_TipoObjetoCoste_CuentaContable = new Dictionary<TipoObjetoCoste, CuentaContable>();
                        relacion_TipoObjetoCoste_CuentaContable.Add((TipoObjetoCoste)relacion.ID_TIPOCOSTE, cuenta);
                        conversor.Add(relacion.IDEMPRESA, relacion_TipoObjetoCoste_CuentaContable);
                        empresaAnt = relacion.IDEMPRESA;
                    }
                    else
                    {
                        conversor[relacion.IDEMPRESA].Add((TipoObjetoCoste)relacion.ID_TIPOCOSTE, cuenta);
                    }
                }
            }

            return conversor;
        }

        public static double Truncate(double importe, int digitsToTruncate = 2)
        {
            //double mult = Math.Pow(10.0, digitsToTruncate);
            //double result = Math.Truncate(mult * importe) / mult;

            string formato = $"###,###,##0.{new String('0', digitsToTruncate)}";
            //float rounded = (float)(Math.Round((double)importe, 2));

            return Convert.ToDouble(importe.ToString(formato));

        }

    }
}
