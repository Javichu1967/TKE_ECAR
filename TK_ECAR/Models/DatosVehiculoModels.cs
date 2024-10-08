using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TK_ECAR.Framework;
using resources = TK_ECAR.Content.resources.ModelsResources;


namespace TK_ECAR.Models
{

    public class DatosVehiculoModel
    {
        public DatosGeneralesModel DatosGenerales_Vehiculo { get; set; }
        public DatosITVModel DatosITV_Vehiculo { get; set; }
        public DatosContratoModel DatosContrato_Vehiculo { get; set; }
        public ConductoresVehiculoModel DatosConductor_Vehiculo { get; set; }
        public IEnumerable<DatosVehiculoMultaModel> DatosMultas_Vehiculo { get; set; }
        public IEnumerable<DatosVehiculoSOLREDModel> DatosSOLRED_Vehiculo { get; set; }
        public IEnumerable<DatosVehiculoLeasePlanModel> DatosLeasePlan_Vehiculo { get; set; }
        public IEnumerable<DatosVehiculoDocumentacionModel> DatosDocumento_Vehiculo { get; set; }
        public ViaVerdeDatatable DatosVia_VerdeDataTable_Vehiculo { get; set; }
    }



}
