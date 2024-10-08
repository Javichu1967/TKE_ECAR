using System;
using System.Collections.Generic;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Infraestructure;

namespace TK_ECAR.Application_Services
{
    public class LeasingService
    {

        public List<T_G_DATOS_LEASING> GetLeasing(DateTime fechaFactura, List<string> lCentrosCoste,
                                            List<int?> empresasFacturadas, List<int?> empresasLeasing)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                //T_G_DATOS_LEASINGSpecification spec = new T_G_DATOS_LEASINGSpecification();
                var firstDayOfMonth = new DateTime(fechaFactura.Year, fechaFactura.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                //return (from datoLeasing in unitOfWork.RepositoryT_G_DATOS_LEASING.Fetch()
                //        where empresasFacturadas.Contains(datoLeasing.Sociedad) &&
                //        empresasLeasing.Contains(datoLeasing.EmpresaLeasing)
                //        where datoLeasing.Fecha_Factura >= firstDayOfMonth
                //        where datoLeasing.Fecha_Factura <= lastDayOfMonth
                //        where lCentrosCoste.Contains(datoLeasing.ECAR_Datos_Vehiculo.CC) || datoLeasing.ECAR_Datos_Vehiculo.CC == null || datoLeasing.ECAR_Datos_Vehiculo.CC == ""
                //        select datoLeasing).OrderBy(x => x.Fecha_Factura).ThenBy(x => x.Num_Factura).ToList();
                //return (from datoLeasing in unitOfWork.RepositoryT_G_DATOS_LEASING.Fetch()
                //        where empresasLeasing.Contains(datoLeasing.EmpresaLeasing)
                //        where datoLeasing.Fecha_Factura >= firstDayOfMonth
                //        where datoLeasing.Fecha_Factura <= lastDayOfMonth
                //        select datoLeasing).OrderBy(x=>x.Fecha_Factura).ThenBy(x=>x.Num_Factura).ToList();
                return (from datoLeasing in unitOfWork.RepositoryT_G_DATOS_LEASING.Include(x => x.ECAR_Datos_Vehiculo)
                        where empresasLeasing.Contains(datoLeasing.EmpresaLeasing)
                        where datoLeasing.Fecha_Factura >= firstDayOfMonth
                        where datoLeasing.Fecha_Factura <= lastDayOfMonth
                        select datoLeasing).OrderBy(x => x.Fecha_Factura).ThenBy(x => x.Num_Factura).ToList();
            }
        }
    }
}