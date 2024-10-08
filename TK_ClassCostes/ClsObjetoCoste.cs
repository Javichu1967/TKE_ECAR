using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TK_ClassCostes.GlobalCostes;

namespace TK_ClassCostes
{
    public class ClsObjetoCoste
    {
        public TipoObjetoCoste TipoCoste { get; set; }
        public string NombreTipoCoste { get; set; }
        //public int IDEmpresaFatura { get; set; }
        //public string NombreEmpresaFatura { get; set; }
        public string NombreCuentaContableAsociada
        {
            get{return new GlobalCostes().GetNombreCuentaContableAsociada(TipoCoste);}
        }
        public string CodigoCuentaContableAsociada
        {
            get { return new GlobalCostes().GetCodigoCuentaContableAsociada(TipoCoste); }
        }

        private double _importe;
        public double Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public double _porcentajeImpuesto;
        public double PorcentajeImpuesto
        {
            get { return _porcentajeImpuesto; }
            set { _porcentajeImpuesto = value; }
        }

        private double _importeConImpuesto;
        public double ImporteConImpuesto
        {
            get
            {
                if (_importeConImpuesto == 0)
                {
                    if (_porcentajeImpuesto == 0)
                    {
                        return _importe;
                    }
                    else
                    {
                        if (_importe == 0)
                        {
                            return _importe;
                        }
                        else
                        {
                            var _multiplicaPor = 0.0;
                            if (_porcentajeImpuesto < 1)
                            {
                                _multiplicaPor = 1 + _porcentajeImpuesto;
                            }
                            else
                            {
                                _multiplicaPor = 1 + (_porcentajeImpuesto/100);
                            }
                            return _importe * _multiplicaPor;
                        }
                    }
                }
                else
                {
                    return _importeConImpuesto;
                }
            }

            set
            {
            }
        }
        public double Descuento { get; set; }
        public double Incremento { get; set; }
    }
}
