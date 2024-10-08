﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_ECAR.Utils;

namespace TK_ECAR.Models.Portugal
{
    public class Leasing
    {
        private int? _sociedad;
        public int? SOCIEDAD
        {
            get
            {
                return _sociedad;
            }
            set
            {
                _sociedad = value;
            }
        }

        public string FACTURA { get; set; }
        private DateTime _fechaFactura;
        public DateTime FECHA_FACTURA
        {
            get
            {
                return _fechaFactura;
            }
            set
            {
                _fechaFactura = value;
            }
        }
        private string _matricula;
        public string MATRICULA
        {
            get
            { return _matricula; }

            set
            {
                if (value.IndexOf('-') == -1)
                {
                    _matricula = _matricula.Trim().Trim(' ');
                    if (_sociedad != Constants.CODIGO_EMPRESA_PORTUGAL)
                    {
                        _matricula = $"{_matricula.Substring(0, 4)}-{_matricula.Substring(4)}";
                    }
                    else
                    {
                        _matricula = $"{_matricula.Substring(0, 2)}-{_matricula.Substring(2, 2)}-{_matricula.Substring(4)}";
                    }
                }
                else
                {
                    _matricula = value;
                }
            }
        }

        public string EJERCICIO { get; set; }
        public string TRIMESTRE { get; set; }
        public double IMP_CIRCULACION { get; set; }
        public double IMP_CIRCULACION_IVA { get; set; }
        public double IMP_MATRICULACION { get; set; }
        public double IMP_MATRICULACION_IVA { get; set; }
        public double IMP_ALQUILER { get; set; }
        public double IMP_ALQUILER_IVA { get; set; }
        public double IMP_MANTENIMIENTO { get; set; }
        public double IMP_MANTENIMIENTO_IVA { get; set; }
        public double IMP_NEUMATICOS { get; set; }
        public double IMP_NEUMATICOS_IVA { get; set; }
        public double IMP_ADMINISTRACION { get; set; }
        public double IMP_ADMINISTRACION_IVA { get; set; }
        public double IMP_SEGURO { get; set; }
        public double IMP_SEGURO_IVA { get; set; }
        public double IMP_ASISTEN_CARRETERA { get; set; }
        public double IMP_ASISTEN_CARRETERA_IVA { get; set; }
        public double IMP_INTERESES_PREPAGADOS { get; set; }
        public double IMP_INTERESES_PREPAGADOS_IVA { get; set; }
        public bool CANARIAS { get { return false; } }
        public bool DIRECTIVO { get; set; }
        public DateTime FECHA_SERVICIO { get { return _fechaFactura; } }
        public DateTime FECHA_IMPORTACION { get { return DateTime.Now; } }
        public double IMP_ITV { get; set; }
        public double IMP_ITV_IVA { get; set; }
        public decimal IMPUESTO { get; set; }
        public int EMPRESA_LEASING { get; set; }
        public DateTime FECHA_ALTA { get { return DateTime.Now; } }
        public string CONCEPTO { get; set; }
    }
}
