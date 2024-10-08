using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;

namespace TK_ECAR.Infraestructure
{
    enum EnumEstadoAlerta
    {
        [Description("Solicitado")]
        Solicitado = 1,
        [Description("Confirmado")]
        Confirmado = 2,
        [Description("Notificado")]
        Notificado = 3,
        [Description("En Gestión")]
        EnGestion = 4,
        //[Description("Recibido")]
        //Recibido = 5,
        [Description("Atendida")]
        Atendida = 6,
        [Description("Vencimiento Próximo")]
        VencimientoProximo = 7,
        [Description("Vencido")]
        Vencido = 8,
        [Description("Cancelada")]
        Cancelada = 9,
        [Description("Renting rechazado")]
        RentingRechazado = 10,
    }
    public partial class RepositoryT_G_ALERTAS
    {

        /// <summary>
        /// Obtiene las alertas que no se han terminado de tratar.
        /// </summary>
        /// <param name="codigoCecos"></param>
        /// <returns></returns>
        public IQueryable<T_G_ALERTAS> GetPendientes(List<string> codigoCecos)
        {
            T_G_ALERTASSpecification specCecos = new T_G_ALERTASSpecification
            {
                ID_CECOIN =codigoCecos

            };
            T_G_ALERTASSpecification specAtendidas = new T_G_ALERTASSpecification
            {
                ID_ESTADOIN = new List<int?>
                {
                    (int)EnumEstadoAlerta.Atendida,
                    (int)EnumEstadoAlerta.RentingRechazado,
                    (int)EnumEstadoAlerta.Cancelada,
                    //(int)EnumEstadoAlerta.Vencido
                }

            };
            var spec = specCecos.And(specAtendidas.Not());

            return Where(spec);

        }
        //public IQueryable<T_G_ALERTAS> Get(List<string> codigoCecos, List<int> estados, List<int> tipos, List<int> acciones)
        //{
        //    T_G_ALERTASSpecification spec = new T_G_ALERTASSpecification
        //    {
        //        ID_CECOIN = codigoCecos,
        //        ID_ESTADOIN = estados.Cast<int?>(),
        //        ID_ACCIONIN = tipos.Cast<int?>(),
        //        ID_TIPO_ALERTAIN = acciones.Cast<int?>(),

        //    };

        //    return Where(spec);

        //}

        public IQueryable<T_G_ALERTAS> GetRentingRechazado()
        {
            T_G_ALERTASSpecification spec = new T_G_ALERTASSpecification
            {
               ID_ESTADO =(int)EnumEstadoAlerta.RentingRechazado

            };

            return Where(spec) ;

        }

        /// <summary>
        /// Obtiene las alertas automáticas que no se han terminado de tratar.
        /// Tampoco incluye las alertas rechazadas de renting.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T_G_ALERTAS> GetAutomaticasPendientes()
        {
           
            T_G_ALERTASSpecification specAutomaticas = new T_G_ALERTASSpecification
            {
                T_M_TIPOS_ALERTAS = new T_M_TIPOS_ALERTASSpecification
                {
                    B_AUTOMATICA = true
                },

            };
            
            T_G_ALERTASSpecification specAtendidas = new T_G_ALERTASSpecification
            {
                 ID_ESTADOIN = new List<int?> { (int)EnumEstadoAlerta.Atendida, (int)EnumEstadoAlerta.RentingRechazado }

            };

            var spec = specAutomaticas.And(specAtendidas.Not());

            return Where(spec);

        }

        

    }
}
