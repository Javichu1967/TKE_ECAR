using System;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    public partial class RepositoryDatos_ITV
    {

        /// <summary>
        /// Devuelve la fecha de la siguiente ITV del vehículo
        /// </summary>
        /// <param name="codMatricula"></param>
        /// <returns></returns>
        public DateTime? NextDateITV(string matricula)
        {
            Datos_ITVSpecification spec = new Datos_ITVSpecification
            { Matricula = matricula };

            return Where(spec).OrderByDescending(o => o.Vto_ITV).Select(x => x.Vto_ITV).FirstOrDefault();

        }

        public IQueryable<Datos_ITV> GetITVFechaVencida(DateTime fechaVto, IUnitOfWork unitOfWork)
        {
            Datos_ITVSpecification spec = new Datos_ITVSpecification
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
        }

    }
}
