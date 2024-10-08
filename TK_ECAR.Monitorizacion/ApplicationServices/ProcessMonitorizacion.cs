using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_ECAR.Domain;
using TK_ECAR.Infraestructure;
using TK_ECAR.Framework;
using log4net;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Monitorizacion.Global;

namespace TK_ECAR.Monitorizacion.ApplicationServices
{
   
    public class ProcessMonitorizacion
    {
        public enum EnumEmpresasLeasing
        {
            HBFAUTORENTING =1,
            ENPROPIEDAD=2,
            LEASING=3,
            LEASEPLAN=4,
            ALDAutomotive=5,
            ALQUILER=6,
            ALPHABET=7,
            VOLKSWAGENRENTING =8
        }
        public void InicializaMonitorizacion()
        {
            Paso = "Inicializando";

            using (var unitOfWork = new UnitOfWork())
            {
                tiposAlertas =  unitOfWork.RepositoryT_M_TIPOS_ALERTAS.GetAutomaticasWithInclude(x=>x.T_R_ESTADOS_ACCION).ToList();               

                var alertaAutomaticasPendientes = unitOfWork.RepositoryT_G_ALERTAS.GetAutomaticasPendientes();

                var alertasRentingRechazadas = unitOfWork.RepositoryT_G_ALERTAS.GetRentingRechazado(); 

                alertaAutomaticas = alertaAutomaticasPendientes.Union(alertasRentingRechazadas).ToList();

                vehiculos = unitOfWork.RepositoryDatos_Vehiculo.Fetch().Where(x => x.Baja == false).ToList();
                //vehiculos = unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(x => x.Baja == false).ToList();  //SUSTITUIR CUANDO SE QUITE COREA
            }
        }

        List<T_M_TIPOS_ALERTAS> tiposAlertas;
        
        List<T_G_ALERTAS> alertaAutomaticas;

        List<int> estadoInciales = new List<int> { (int)EnumEstadoAlerta.VencimientoProximo, (int)EnumEstadoAlerta.Vencido };

        DateTime hoy = DateTime.Now.Date;

        List<Datos_Vehiculo> vehiculos;
        //List<ECAR_Datos_Vehiculo> vehiculos; //SUSTITUIR CUANDO SE QUITE COREA

        const string userAlertas = "MONITORIZACION";

        public string Paso = string.Empty;

        #region Métodos públicos


        [LogException]
        public void Run()
        {
            if (vehiculos == null)
            {
                InicializaMonitorizacion();
            }

            //Console.WriteLine("Inicio de generación de alertas renting...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Generación de alertas renting... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            RunAlertasRenting();
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Finaliza la generación de alertas renting... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            //Console.WriteLine("Inicio de generación de alertas carnet...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Generación de alertas carnet... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            RunAlertasCarnet();
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Finaliza la generación de alertas carnet... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            //Console.WriteLine("Inicio de generación de alertas itv...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Generación de alertas itv... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            RunAlertasITV();
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Finaliza la generación de alertas itv... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            //Console.WriteLine("Inicio de generación de alertas seguro...");
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Generación de alertas seguro... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
            RunAlertasSeguros();
            GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Finaliza la generación de alertas seguro... {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}");
        }

        public void RunAlertasRenting()
        {
            Paso = "RunAlertasRenting";
           
            using (var unitOfWork = new UnitOfWork())
            {
                var cont = 0;

                var fechaPreaviso = getFechaPreaviso((int)EnumTipoAlerta.Renting);
                
                var vehiculosRenting  = getVehiculosRenting(fechaPreaviso);

                foreach (var vehiculo in vehiculosRenting)
                {
                    var fechafinalizaRenting = vehiculo.Fecha_Alta.Value.AddMonths(vehiculo.Cuotas.Value); 

                    cont = cont + saveAlerta(unitOfWork, (int)EnumTipoAlerta.Renting, fechafinalizaRenting.Date, vehiculo.Matricula);
                     
                }

                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Alertas renting... {cont.ToString()}");

                //hay que hacer un tratamiento especial:
                //la alerta sin le han cambiado la fecha en la app de Corea  nosotros nos
                //encargaremos de poner la alerta en estado de atendida para ECar 
                //sabemos cuales son si existen en ECar y no está atendida y no nos la hemos traido de Corea
                var matriculas = vehiculosRenting.Select(x => x.Matricula);

                var alertasRenting = getAlertasECarNotContain((int)EnumTipoAlerta.Renting, matriculas);

                alertasRenting.ForEach(alerta =>
                {
                    changeEstadoAlerta(unitOfWork, alerta, (int)EnumEstadoAlerta.Atendida);
                });
                ////////
                unitOfWork.Commit();
            }
            
        }

        public void RunAlertasITV()
        {
            var cont = 0;

            Paso = "RunAlertasITV";

            using (var unitOfWork = new UnitOfWork())
            {
                
                var fechaPreaviso = getFechaPreaviso((int)EnumTipoAlerta.ITV);

                var datosITV = unitOfWork.RepositoryDatos_ITV.GetITVFechaVencida(fechaPreaviso, unitOfWork).ToList();
                //var datosITV = unitOfWork.RepositoryECAR_Datos_ITV.GetITVFechaVencida(fechaPreaviso, unitOfWork).ToList();//SUSTITUIR CUANDO SE QUITE COREA

                foreach (var itv in datosITV)
                {                   
                     
                    cont = cont + saveAlerta(unitOfWork, (int)EnumTipoAlerta.ITV, itv.Vto_ITV.Value.Date, itv.Matricula);

                }

                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Alertas itv... {cont.ToString()}");

                //hay que hacer un tratamiento especial:
                //la alerta sin le han cambiado la fecha en la app de Corea  nosotros nos
                //encargaremos de poner la alerta en estado de atendida para ECar 
                //sabemos cuales son si existen en ECar y no está atendida y no nos la hemos traido de Corea
                var matriculas = datosITV.Select(x => x.Matricula);

                var alertasITV = getAlertasECarNotContain((int)EnumTipoAlerta.ITV, matriculas);

                alertasITV.ForEach(alerta =>
                {
                    changeEstadoAlerta(unitOfWork, alerta, (int)EnumEstadoAlerta.Atendida);
                });

                ////////
                unitOfWork.Commit();
            }
            

        }

        public void RunAlertasCarnet()
        {
            Paso = "RunAlertasCarnet";

            using (var unitOfWork = new UnitOfWork())
            {
                var cont = 0;

                var fechaPreaviso = getFechaPreaviso((int)EnumTipoAlerta.Carnet);

                var datosConductor = unitOfWork.RepositoryDatos_Conductor.GetConductoresWithFechaCarnetVencida(fechaPreaviso).ToList();
                //var datosConductor = unitOfWork.RepositoryECAR_Datos_Conductor.GetConductoresWithFechaCarnetVencida(fechaPreaviso).ToList(); //SUSTITUIR CUANDO SE QUITE COREA

                var datosConductorAlerta = (from conductor in datosConductor
                                    join datosVehiculos in vehiculos
                                    on conductor.Cod_Conductor equals datosVehiculos.Conductor
                                    select new
                                    {
                                        conductor.Fecha_Vencimiento_Carnet,
                                        datosVehiculos.Matricula

                                    }).ToList();

                foreach (var datoConductorAlerta in datosConductorAlerta)
                { 
                    cont = cont + saveAlerta(unitOfWork, (int)EnumTipoAlerta.Carnet, datoConductorAlerta.Fecha_Vencimiento_Carnet.Value.Date, datoConductorAlerta.Matricula);

                }

                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Alertas carnet... {cont.ToString()}");

                //hay que hacer un tratamiento especial:
                //la alerta sin le han cambiado la fecha en la app de Corea  nosotros nos
                //encargaremos de poner la alerta en estado de atendida para ECar 
                //sabemos cuales son si existen en ECar y no está atendida y no nos la hemos traido de Corea
                var matriculas = datosConductorAlerta.Select(x => x.Matricula);

                var alertasCarnet = getAlertasECarNotContain((int)EnumTipoAlerta.Carnet, matriculas);

                alertasCarnet.ForEach(alerta =>
                {
                    changeEstadoAlerta(unitOfWork, alerta, (int)EnumEstadoAlerta.Atendida);
                });
                ////////

                unitOfWork.Commit();
            }
            
        }

        public void RunAlertasSeguros()
        {
            var cont = 0;

            Paso = "RunAlertasSeguros";

            using (var unitOfWork = new UnitOfWork())
            {
                var fechaPreaviso = getFechaPreaviso((int)EnumTipoAlerta.Seguro);

                var vehiculos = unitOfWork.RepositoryDatos_Vehiculo.GetAlertasSeguros(fechaPreaviso).ToList();
                //var vehiculos = unitOfWork.RepositoryECAR_Datos_Vehiculo.GetAlertasSeguros(fechaPreaviso).ToList();//SUSTITUIR CUANDO SE QUITE COREA

                foreach (var vehiculo in vehiculos)
                { 
                    cont = cont + saveAlerta(unitOfWork, (int)EnumTipoAlerta.Seguro, vehiculo.Vto_Seguro.Value.Date, vehiculo.Matricula);

                }

                GlobalApp.EscribeLogApp(GlobalApp.TipoDeLog.INFO, $"Alertas seguro... {cont}");

                //hay que hacer un tratamiento especial:
                //la alerta sin le han cambiado la fecha en la app de Corea  nosotros nos
                //encargaremos de poner la alerta en estado de atendida para ECar 
                //sabemos cuales son si existen en ECar y no está atendida y no nos la hemos traido de Corea
                var matriculas = vehiculos.Select(x => x.Matricula);

                var alertasSeguros = getAlertasECarNotContain((int)EnumTipoAlerta.Seguro, matriculas);

                alertasSeguros.ForEach(alerta =>
                {
                    changeEstadoAlerta(unitOfWork, alerta, (int)EnumEstadoAlerta.Atendida);
                });

                ////////


                unitOfWork.Commit();
            }
             
        }

        #endregion


        #region Métodos privados


        private int saveAlerta(UnitOfWork unitOfWork, int idTipoAlerta, DateTime fechaVencimiento, string matricula)
        {
            var alerta = getAlertaECar(idTipoAlerta, matricula);

            int idestado = (fechaVencimiento < hoy) ? (int)EnumEstadoAlerta.Vencido : (int)EnumEstadoAlerta.VencimientoProximo;

            if (alerta == null)
            {
                var idaccion = getAccion(idTipoAlerta, idestado);

                var mivehiculo = vehiculos.Single(x => x.Matricula.Equals(matricula));

                //creamos la alerta
                var alertanueva = new T_G_ALERTAS
                {
                    ID_TIPO_ALERTA = idTipoAlerta,
                    MATRICULA = matricula,
                    ID_ACCION = idaccion,
                    ID_ESTADO = idestado,
                    ID_CECO = mivehiculo.CC,
                    MODELO = mivehiculo.Modelo,
                    USUARIO_CREACION = userAlertas,                    
                    FECHA_VENCIMIENTO = fechaVencimiento
                };

                unitOfWork.RepositoryT_G_ALERTAS.Insert(alertanueva);

                return 1;
            }
            else if (estadoInciales.Contains(alerta.ID_ESTADO) && alerta.ID_ESTADO != idestado)
            {
                //si  la alerta todavía se encuentra en sus estados iniciales
                //pero ahora le corresponde otro estado se actualiza
                //es decir ha pasado de vencimiento prox. a vencido.
               
                changeEstadoAlerta(unitOfWork, alerta, idestado);

                return 1;
            }
            else
            {
                return 0;
            }
        }

        private void changeEstadoAlerta(UnitOfWork unitOfWork, T_G_ALERTAS alerta, int idestado)
        {

            alerta.ID_ESTADO = idestado;

            alerta.USUARIO_MODIFICACION = userAlertas;

            if (idestado==(int)EnumEstadoAlerta.Atendida)
            {
                alerta.ID_ACCION = null;
            }

            unitOfWork.RepositoryT_G_ALERTAS.Update(alerta);
        }


        private List<int?> getEmpresasRenting()
        {
            T_M_EMPRESAS_VEHICULOSSpecification spec = new T_M_EMPRESAS_VEHICULOSSpecification
            {
                RENTING = true
            };

            using (var unitOfWork = new UnitOfWork())
            {
                return unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Where(spec).Select(x => (int?)x.ID_EMPRESA).ToList();
            }
        }


        private List<Datos_Vehiculo> getVehiculosRenting(DateTime fechaPreaviso)
        {

            var empresasExcluir = new List<int?>
           {
               (int)EnumEmpresasLeasing.HBFAUTORENTING,
               (int)EnumEmpresasLeasing.ENPROPIEDAD,
               (int)EnumEmpresasLeasing.LEASING,
               (int)EnumEmpresasLeasing.ALQUILER,
               (int)EnumEmpresasLeasing.ALPHABET,
           };

            //var empresasExcluir = getEmpresasRenting(); //SUSTITUIR CUANDO SE QUITE COREA

           return vehiculos
                    .Where(x => !empresasExcluir.Contains(x.EmpresaLeasing) && x.Fecha_Alta.HasValue && x.Cuotas.HasValue)
                    .Where(x => x.Fecha_Alta.Value.AddMonths(x.Cuotas.Value) <= fechaPreaviso)
                    .ToList();
        }
        private DateTime getFechaPreaviso(int idtipoalerta)
        {
            var diasPreaviso = getParamDiasPreaviso(idtipoalerta);

            return hoy.AddDays(diasPreaviso);
        }

        private int getParamDiasPreaviso(int idtipoalerta)
        {
           return  tiposAlertas.Where(x => x.ID_TIPO_ALERTA == idtipoalerta)
                .Select(x => x.DIAS_PREAVISO)
                .FirstOrDefault() ?? 0;
        }

       
        private T_G_ALERTAS getAlertaECar(int idtipoalerta, string matricula)
        {
             return alertaAutomaticas
                       .Where(x => x.ID_TIPO_ALERTA.Equals(idtipoalerta))
                       .SingleOrDefault(x => x.MATRICULA.Equals(matricula));
        }

        private int? getAccion(int idtipoalerta, int idestado)
        {
            var tipoAlerta = tiposAlertas.Single(x => x.ID_TIPO_ALERTA == idtipoalerta);
            return tipoAlerta.T_R_ESTADOS_ACCION                  
                 .Where(x=>!x.ID_ESTADO_ANTERIOR.HasValue )
                 .Select(x=>x.ID_ACCION)
                 .FirstOrDefault();
        }

        private List<T_G_ALERTAS> getAlertasECarNotContain(int idtipoalerta, IEnumerable<string> matriculas)
        {
            return alertaAutomaticas
                      .Where(x => x.ID_TIPO_ALERTA.Equals(idtipoalerta)) 
                      .Where(x=>!matriculas.Contains(x.MATRICULA))                    
                      .ToList();
        }


        #endregion
    }
}
