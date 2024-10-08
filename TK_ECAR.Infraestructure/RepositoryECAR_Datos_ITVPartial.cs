using System;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryECAR_Datos_ITV
    {

        /// <summary>
        /// Devuelve la fecha de la siguiente ITV del vehículo
        /// </summary>
        /// <param name="codMatricula"></param>
        /// <returns></returns>
        public DateTime? NextDateITV(string matricula)
        {
            ECAR_Datos_ITVSpecification spec = new ECAR_Datos_ITVSpecification
            { Matricula = matricula };

            return Where(spec).OrderByDescending(o => o.Vto_ITV).Select(x => x.Vto_ITV).FirstOrDefault();
        }

        public IQueryable<ECAR_Datos_ITV> GetITVFechaVencida(DateTime fechaVto, IUnitOfWork unitOfWork)
        {
            ECAR_Datos_ITVSpecification spec = new ECAR_Datos_ITVSpecification
            {
                Vto_ITV = fechaVto,
            };

            return (from datosITV in Where(spec)
                    join vehiculo in InternalContext.Set<Datos_Vehiculo>()
                    on datosITV.Matricula equals vehiculo.Matricula
                    where vehiculo.Baja == false
                    orderby datosITV.Vto_ITV descending
                    select datosITV)
                    .GroupBy(x => x.Matricula)

                    .Select(x => x.FirstOrDefault());

            //return (from datosITV in unitOfWork.RepositoryECAR_Datos_ITV.Fetch().Where(o => o.Vto_ITV <= fechaVto && (o.Tarifa == null || o.Tarifa == 0))
            //        join vehiculo in InternalContext.Set<Datos_Vehiculo>()
            //        on datosITV.Matricula equals vehiculo.Matricula
            //        where vehiculo.Baja == false
            //        orderby datosITV.Vto_ITV descending
            //        select datosITV)
            //        .GroupBy(x => x.Matricula)

            //        .Select(x => x.FirstOrDefault());


            //Coger aquellas matrículas cuya fecha mayor de vencimiento ITV esté por debajo 
            //  de la fecha fecVto pasada por parámetro (fecha de vencimiento calculada con 
            //  los días de preaviso).
            //var fechasVto = (from datosITV in unitOfWork.RepositoryDatos_ITV.Fetch()
            //                 group datosITV by datosITV.Matricula into fechasProximaVTO
            //                 select new
            //                 {
            //                     Id = fechasProximaVTO.Max(x => x.Id),
            //                     Falta = fechasProximaVTO.Max(x => x.Falta),
            //                     Importe = fechasProximaVTO.Max(x => x.Importe) ?? 0,
            //                     Impuesto_Circulacion = fechasProximaVTO.Max(x => x.Impuesto_Circulacion) ?? 0,
            //                     Matricula = fechasProximaVTO.Max(x => x.Matricula),
            //                     Otros = fechasProximaVTO.Max(x => x.Otros),
            //                     Pr_Conservacion = fechasProximaVTO.Max(x => x.Pr_Conservacion) ?? 0,
            //                     Tarifa = fechasProximaVTO.Max(x => x.Tarifa) ?? 0,
            //                     Tasa = fechasProximaVTO.Max(x => x.Tasa) ?? 0,
            //                     Ultima_ITV = fechasProximaVTO.Max(x => x.Ultima_ITV),
            //                     Vto_ITV = fechasProximaVTO.Max(x => x.Vto_ITV),
            //                 }).Where(o => o.Vto_ITV <= fechaVto).ToList();

            //var result = (from datosITV in fechasVto
            //              join vehiculo in InternalContext.Set<Datos_Vehiculo>()
            //              on datosITV.Matricula equals vehiculo.Matricula
            //              where vehiculo.Baja == false
            //              select datosITV);

        }
    }
}
