using System;
using System.Collections.Generic;
using System.Globalization;
using TK_ECAR.Framework.Utils;
using static TK_ECAR.Framework.Utils.GlobalCostes;

namespace TK_ECAR.Framework.Models
{

    public class FacturaModels
    {
        public string NumeroFactura { get; set; }
        public string CentroCosteCoste { get; set; }
        public string NombreCentroCosteCoste { get; set; }
        public int IDEmpresaFatura { get; set; }
        public string NombreEmpresaFatura { get; set; }
        public string CIFEmpresaFatura { get; set; }
        public int IDEmpresaLeasing { get; set; }
        public string NombreEmpresaLeasing { get; set; }
        public string CIFEmpresaLeasing { get; set; }
        public DateTime FechaFactura { get; set; }
        public string MesFactura
        {
            get
            {
                if (FechaFactura == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    //var mes = FechaFactura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    //var año = FechaFactura.Year.ToString();
                    //return $"{mes}-{año}";
                    return String.Format("{0:y}", FechaFactura);
                }
            }
        }
        public string AñoMesFactura
        {
            get
            {
                if (FechaFactura == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    var mes = FechaFactura.Month.ToString("00");
                    var año = FechaFactura.Year.ToString();
                    return $"{año} {mes}";
                }
            }
        }
        public DateTime FechaVencimientoFactura { get; set; }

        public bool Canarias { get; set; }

        public Dictionary<string, AsientoFactura> AsientosFactura { get; private set; }

        public void AddAsientosFactura(string PorcentajeImpuesto)
        {
            CompruebaAsientosFactura();

            if (!AsientosFactura.ContainsKey(PorcentajeImpuesto))
            {
                AsientosFactura.Add(PorcentajeImpuesto, new AsientoFactura());
            }
        }

        public double TotalFactura
        {
            get
            {
                double returnTotal = 0.0;
                //CompruebaAsientosFactura();
                //foreach (KeyValuePair<string, AsientoFactura> asiento in AsientosFactura)  
                //{
                    returnTotal += (GlobalCostes.Truncate(Base, 2) + GlobalCostes.Truncate(Impuesto, 2));
                //}

                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        public double Base
        {
            get
            {
                var returnTotal = 0.0;
                CompruebaAsientosFactura();
                foreach (KeyValuePair<string, AsientoFactura> asiento in AsientosFactura)
                {
                    Dictionary<string, string> conceptosTratados = new Dictionary<string, string>();
                    foreach (TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
                    {
                        var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(IDEmpresaFatura, tCoste);
                        if (asiento.Value.DesgloseFactura.ContainsKey(keyConcepto) && !conceptosTratados.ContainsKey(keyConcepto))
                        {
                            returnTotal += GlobalCostes.Truncate(asiento.Value.DesgloseFactura[keyConcepto].Importe, 2);
                        }
                        if (!conceptosTratados.ContainsKey(keyConcepto))
                        {
                            conceptosTratados.Add(keyConcepto, keyConcepto);
                        }
                    }
                }
                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        public double Impuesto
        {
            get
            {
                double returnTotal = 0.0;
                CompruebaAsientosFactura();
                foreach (KeyValuePair<string, AsientoFactura> asiento in AsientosFactura)
                {
                    returnTotal += GlobalCostes.Truncate(asiento.Value.TotalImporteImpuesto, 2);
                }

                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        private void CompruebaAsientosFactura()
        {
            if (AsientosFactura == null)
            {
                AsientosFactura = new Dictionary<string, AsientoFactura>();
            }

        }

    }

    //Por cada % de impuesto distinto.
    public class AsientoFactura
    {
        private int empresaFacturadaAsiento;
        public double PorcentajeDeImpuesto { get; set; }

        public Dictionary<string, ConceptoFactura> DesgloseFactura { get; private set; }

        public string NombreCuentaContable { get; private set; }
        public string CodigoCuentaContable { get; private set; }

        //Método para añadir el concepto de facturacíón.
        public void AddConceptoFacturacion(int empresaFacturada, TipoObjetoCoste TipoCoste, double? Importe, double importeImpuesto, double importeImpuestoNoDeducible, double? importeImpuestoDeducible)
        {
            CompruebaDesgloseFactura();
            empresaFacturadaAsiento = empresaFacturada;

            if (Importe == null)
            {
                Importe = 0.0;
            }
            if (importeImpuestoDeducible == null)
            {
                importeImpuestoDeducible = 0.0;
            }
            var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(empresaFacturada, TipoCoste);
            CodigoCuentaContable = keyConcepto;
            NombreCuentaContable = GlobalCostes.GetNombreCuentaContableAsociada(empresaFacturada, TipoCoste);
            if (!DesgloseFactura.ContainsKey(keyConcepto))
            {
                DesgloseFactura.Add(keyConcepto,
                    new ConceptoFactura
                    {
                        CuentaContable = keyConcepto,
                        NombreCuentaContable = GlobalCostes.GetNombreCuentaContableAsociada(empresaFacturada, TipoCoste),
                    }
                    );
            }
            DesgloseFactura[keyConcepto].Importe += GlobalCostes.Truncate((double)Importe);
            DesgloseFactura[keyConcepto].ImporteImpuesto += GlobalCostes.Truncate(importeImpuesto);
            DesgloseFactura[keyConcepto].ImporteImpuestoNoDeducible += GlobalCostes.Truncate(importeImpuestoNoDeducible);
            DesgloseFactura[keyConcepto].ImporteImpuestoDeducible += GlobalCostes.Truncate((double)importeImpuestoDeducible);
        }

        public double TotalGasto
        {
            get
            {
                var returnTotal = 0.0;
                CompruebaDesgloseFactura();
                Dictionary<string, string> conceptosTratados = new Dictionary<string, string>(); 
                foreach (TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
                {
                    var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(empresaFacturadaAsiento, tCoste);
                    if (DesgloseFactura.ContainsKey(keyConcepto) && !conceptosTratados.ContainsKey(keyConcepto))
                    {
                        //Console.WriteLine(DesgloseFactura[keyConcepto].Importe);
                        returnTotal += GlobalCostes.Truncate(DesgloseFactura[keyConcepto].Importe, 2); //+ DesgloseFactura[keyConcepto].ImporteImpuestoDeducible;
                    }
                    if (!conceptosTratados.ContainsKey(keyConcepto))
                    {
                        conceptosTratados.Add(keyConcepto, keyConcepto);
                    }
                }
                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        public double TotalImporteImpuesto
        {
            get
            {
                var returnTotal = 0.0;
                CompruebaDesgloseFactura();
                Dictionary<string, string> conceptosTratados = new Dictionary<string, string>();
                foreach (TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
                {
                    var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(empresaFacturadaAsiento, tCoste);
                    if (DesgloseFactura.ContainsKey(keyConcepto) && !conceptosTratados.ContainsKey(keyConcepto))
                    {
                        returnTotal += GlobalCostes.Truncate(DesgloseFactura[keyConcepto].ImporteImpuesto, 2);
                    }
                    if (!conceptosTratados.ContainsKey(keyConcepto))
                    {
                        conceptosTratados.Add(keyConcepto, keyConcepto);
                    }
                }
                return GlobalCostes.Truncate(returnTotal, 2); // Math.Ceiling(returnTotal * 100) / 100f;
            }
        }

        public double TotalImporteImpuestoDeducible
        {
            get
            {
                var returnTotal = 0.0;
                CompruebaDesgloseFactura();
                Dictionary<string, string> conceptosTratados = new Dictionary<string, string>();
                foreach (TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
                {
                    var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(empresaFacturadaAsiento, tCoste);
                    if (DesgloseFactura.ContainsKey(keyConcepto) && !conceptosTratados.ContainsKey(keyConcepto))
                    {
                        returnTotal += GlobalCostes.Truncate(DesgloseFactura[keyConcepto].ImporteImpuestoDeducible, 2); //Math.Round(DesgloseFactura[keyConcepto].ImporteImpuestoDeducible, 2); //Math.Ceiling(DesgloseFactura[keyConcepto].ImporteImpuestoDeducible * 100) / 100.0; 
                    }
                    if (!conceptosTratados.ContainsKey(keyConcepto))
                    {
                        conceptosTratados.Add(keyConcepto, keyConcepto);
                    }
                }
                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        public double TotalImporteImpuestoNoDeducible
        {
            get
            {
                var returnTotal = 0.0;
                CompruebaDesgloseFactura();
                Dictionary<string, string> conceptosTratados = new Dictionary<string, string>();
                foreach (TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
                {
                    var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(empresaFacturadaAsiento, tCoste);
                    if (DesgloseFactura.ContainsKey(keyConcepto) && !conceptosTratados.ContainsKey(keyConcepto))
                    {
                        returnTotal += GlobalCostes.Truncate(DesgloseFactura[keyConcepto].ImporteImpuestoNoDeducible, 2); // Math.Ceiling(DesgloseFactura[keyConcepto].ImporteImpuestoNoDeducible * 100) / 100.0; //DesgloseFactura[keyConcepto].ImporteImpuestoNoDeducible;
                    }
                    if (!conceptosTratados.ContainsKey(keyConcepto))
                    {
                        conceptosTratados.Add(keyConcepto, keyConcepto);
                    }
                }
                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        public double TotalGastoMasTotalImporteImpuestoNoDeducible
        {
            get
            {
                var returnTotal = 0.0;
                returnTotal += (GlobalCostes.Truncate(TotalGasto, 2) + GlobalCostes.Truncate(TotalImporteImpuestoNoDeducible, 2));
                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        public double TotalConceptosDeducibles
        {
            get
            {
                var returnTotal = 0.0;
                CompruebaDesgloseFactura();
                Dictionary<string, string> conceptosTratados = new Dictionary<string, string>();
                foreach (TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
                {
                    var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(empresaFacturadaAsiento, tCoste);
                    if (DesgloseFactura.ContainsKey(keyConcepto) && !conceptosTratados.ContainsKey(keyConcepto))
                    {
                        returnTotal += GlobalCostes.Truncate(DesgloseFactura[keyConcepto].Importe, 2) + GlobalCostes.Truncate(DesgloseFactura[keyConcepto].ImporteImpuestoDeducible, 2);
                    }
                    if (!conceptosTratados.ContainsKey(keyConcepto))
                    {
                        conceptosTratados.Add(keyConcepto, keyConcepto);
                    }
                }
                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }

        public double TotalConceptos
        {
            get
            {
                var returnTotal = 0.0;
                CompruebaDesgloseFactura();
                Dictionary<string, string> conceptosTratados = new Dictionary<string, string>();
                foreach (TipoObjetoCoste tCoste in Enum.GetValues(typeof(TipoObjetoCoste)))
                {
                    var keyConcepto = GlobalCostes.GetCodigoCuentaContableAsociada(empresaFacturadaAsiento, tCoste);
                    if (DesgloseFactura.ContainsKey(keyConcepto) && !conceptosTratados.ContainsKey(keyConcepto))
                    {
                        returnTotal += GlobalCostes.Truncate(DesgloseFactura[keyConcepto].Total, 2);
                    }
                    if (!conceptosTratados.ContainsKey(keyConcepto))
                    {
                        conceptosTratados.Add(keyConcepto, keyConcepto);
                    }
                }
                return GlobalCostes.Truncate(returnTotal, 2);
            }
        }



        private void CompruebaDesgloseFactura()
        {
            if (DesgloseFactura == null)
            {
                DesgloseFactura = new Dictionary<string, ConceptoFactura>();
            }

        }
    }


    public class ConceptoFactura
    {
        //public TipoObjetoCoste TipoCoste { get; set; }
        public string CuentaContable { get; set; }

        public string NombreCuentaContable { get; set; }

        private double _importe;
        public double Importe //{ get; set; }
        {
            get
            {
                return GlobalCostes.Truncate(_importe);
            }
            set { _importe = value; }
        }
        private double _importeImpuesto;
        public double ImporteImpuesto //{ get; set; }
        {
            get
            {
                return GlobalCostes.Truncate(_importeImpuesto);
            }
            set { _importeImpuesto = value; }
        }

        //Coches de dirección. El 50% del ImporteImpuestoDeducible.
        private double _importeImpuestoNoDeducible;
        public double ImporteImpuestoNoDeducible //{ get; set; }
        {
            get
            {
                return GlobalCostes.Truncate(_importeImpuestoNoDeducible);
            }
            set { _importeImpuestoNoDeducible = value; }
        }
        private double _importeImpuestoDeducible;
        public double ImporteImpuestoDeducible //{ get; set; }
        {
            get
            {
                if (ImporteImpuestoNoDeducible > 0.0000001)
                {
                    return _importeImpuestoDeducible;
                }
                else
                {
                    return GlobalCostes.Truncate(_importeImpuestoDeducible);
                }
            }
            set { _importeImpuestoDeducible = value; }
        }


        public double TotalDeducible
        {
            get
            {
                return GlobalCostes.Truncate(Importe, 2) + GlobalCostes.Truncate(ImporteImpuestoDeducible, 2);
            }
        }

        public double TotalGastoMasImporteImpuestoNoDeducible
        {
            get
            {
                return GlobalCostes.Truncate(Importe, 2) + GlobalCostes.Truncate(ImporteImpuestoNoDeducible, 2);
            }
        }

        public double Total
        {
            get
            {
                return GlobalCostes.Truncate(Importe, 2) + GlobalCostes.Truncate(ImporteImpuestoDeducible, 2) + GlobalCostes.Truncate(ImporteImpuestoNoDeducible, 2);
            }
        }
    }

    public class FacturaDataTableModels
    {
        public string EmpresaFactura { get; set; }
        public string EmpresaLeasing { get; set; }
        public string NumFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public double BaseFactura { get; set; }
        public string BaseFactura_FORMATEADA
        {
            get
            {
                return $"{ConvertExtensions.NullableToFormattedString((decimal?)BaseFactura, "###,###,##0.00")}€";
            }
        }
        public double ImpuestoFactura { get; set; }
        public string ImpuestoFactura_FORMATEADA
        {
            get
            {
                return $"{ConvertExtensions.NullableToFormattedString((decimal?)ImpuestoFactura, "###,###,##0.00")}€";
            }
        }
        public double TotalFactura { get; set; }
        public string TotalFactura_FORMATEADA
        {
            get
            {
                return $"{ConvertExtensions.NullableToFormattedString((decimal?)TotalFactura, "###,###,##0.00")}€";
            }
        }
    }

    //public class ObjetoCosteFacturaModels
    //{
    //    public string Matricula { get; set; }
    //    public bool CocheDireccion { get; set; }
    //    public TipoObjetoCoste TipoCoste { get; set; }
    //    public string NombreTipoCoste { get; set; }
    //    public string CentroCosteCoste { get; set; }
    //    public string NombreCentroCosteCoste { get; set; }
    //    public int IDEmpresaFatura { get; set; }
    //    public string NombreEmpresaFatura { get; set; }
    //    public int IDEmpresaLeasing { get; set; }
    //    public string NombreEmpresaLeasing { get; set; }
    //    public string NumeroFactura { get; set; }
    //    public DateTime FechaFactura { get; set; }
    //    public DateTime FechaVencimientoFactura { get; set; }

    //    public string NombreCuentaContableAsociada
    //    {
    //        get { return new GlobalCostes().GetNombreCuentaContableAsociada(TipoCoste); }
    //    }
    //    public string CodigoCuentaContableAsociada
    //    {
    //        get { return new GlobalCostes().GetCodigoCuentaContableAsociada(TipoCoste); }
    //    }

    //    private float _importe;
    //    public double Importe
    //    {
    //        get { return _importe; }
    //        set { _importe = value; }
    //    }

    //    private float _porcentajeImpuesto;
    //    public double PorcentajeImpuesto
    //    {
    //        get { return _porcentajeImpuesto; }
    //        set { _porcentajeImpuesto = value; }
    //    }

    //    private float _importeConImpuesto;
    //    public double ImporteConImpuesto
    //    {
    //        get
    //        {
    //            if (_importeConImpuesto == 0)
    //            {
    //                if (_porcentajeImpuesto == 0)
    //                {
    //                    return _importe;
    //                }
    //                else
    //                {
    //                    if (_importe == 0)
    //                    {
    //                        return _importe;
    //                    }
    //                    else
    //                    {
    //                        var _multiplicaPor = 0.0;
    //                        if (_porcentajeImpuesto < 1)
    //                        {
    //                            _multiplicaPor = 1 + _porcentajeImpuesto;
    //                        }
    //                        else
    //                        {
    //                            _multiplicaPor = 1 + (_porcentajeImpuesto / 100);
    //                        }
    //                        return _importe * _multiplicaPor;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                return _importeConImpuesto;
    //            }
    //        }

    //        set
    //        {
    //        }
    //    }
    //    public double Descuento { get; set; }
    //    public double Incremento { get; set; }
    //}


    //public class FacturacionModels
    //{
    //    public List<ObjetoCosteFacturaModels> ListaCostes { get; set; }
    //}

}
