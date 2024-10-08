
using System.ComponentModel;

namespace TK_ECAR.Framework
{

    public enum EnumTipoSolicitudSOLRED
    {
        [Description("Robo/Perdida")]       
        Robo = 1,
        //[Description("Primera vez")]
        //PrimeraVez = 2,
       
        [Description("Deterioro")]
        Deterioro = 4 ,
        [Description("Otros")]
        Otros = 5
    }
    public enum EnumTipoAlerta
    {
        [Description("Multa")]
        Multa = 1,
        [Description("ITV")]
        ITV = 2,
        [Description("Carnet de conducir")]
        Carnet = 3,
        [Description("Seguro")]
        Seguro = 4,
        [Description("Cambio de conductor")]
        CambioConductor = 5,
        [Description("Tarjeta combustible")]
        TarjetaSOLRED =6,
        [Description("Robos")]
        Robos = 7,
        [Description("Otras notificaciones")]
        Otras = 8,
        [Description("Renting")]
        Renting = 9,
    }

    public enum EnumAccionEntity
    {
        [Description("AltaPregunta")]
        Alta = 1,
        [Description("Modificación")]
        Modificacion = 2,
        [Description("Baja Pregunta")]
        Baja = 3,
        [Description("Consulta")]
        Consulta = 4,
        [Description("Sin Acción")]
        SinAccion = 5
    }

    public enum EnumResultadoEntity
    {
        [Description("Grabación correcta")]
        GrabacionCorrecta = 1,
        [Description("Error ya existente")]
        Error_ya_Existente = 2,
        [Description("Error en proceso")]
        Error_en_Proceso = 3
    }

    public enum EnumTipoPerfil
    {
        [Description("Gestor de red")]
        Administrativo = 1,
        [Description("Controller")]
        Controller = 2,
        [Description("Director territorial")]
        DirectorTerritorial = 3,
        [Description("Gestor Flota")]
        SuperUsuario = 4,
        [Description("Delegado de zona")]
        DelegadoZona = 5,
        [Description("Sin definir")]
        SinDefinir = 6
    }
    public enum EnumTipoAccion
    {
      
        [Description("Identificar conductor")]
        IdentificarConductor = 1,
        [Description("Notificar")]
        Notificar = 2,
        [Description("Iniciar gestión")]
        IniciarGestion = 3,
        [Description("Aceptar tramite")]        
        AceptarTramite = 4,
        [Description("Notificar renovación")]
        NotificarRenovacion = 5,
        [Description("Confirmar renting")]
        ConfirmarRenting = 6,
        [Description("Confirmar recepción")]
        ConfirmarRecepcion = 7,
        [Description("Validar solicitud")]
        ValidarSolicitud = 8,
        [Description("Dar respuesta")]
        DarRespuesta = 9,
    }
    public enum EnumEstadoAlerta
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

    public enum EnumOpcionesMenu
    {
        [Description("Gestion")]
        Gestion = 1,
        [Description("Alertas")]
        Alertas = 2,
        [Description("Mi Vehículo")]
        MiVehiculo = 3,
        [Description("Mi Flota")]
        MiFlota = 4,
        [Description("Preguntas Frecuentes")]
        PreguntasFrecuentes = 5,
        [Description("Documentación")]
        Documentacion = 6,
        [Description("Aplicaciones Externas")]
        AplicacionesExternas = 7,
        [Description("Teléfonos frecuentes")]
        TelefonoFrecuentes = 8,
        [Description("Mto. preguntas frecuentes")]
        MtoPreguntasFrecuentes = 9,
        [Description("Mto. documentación")]
        MtoDocumentacion = 10,
        [Description("Mto. categorías documentos")]
        MtoCategorias = 11,
        [Description("Mto. teléfonos frecuentes")]
        MtoTelefonosFrecuentes = 12,
        [Description("Informes de consumos")]
        InformesConsumos = 13,
        [Description("Plataforma de presupuestos")]
        PlataformaPresupuestos = 14,
        [Description("Mto. Días Preaviso Tipos de Alerta")]
        MtoDiasPreavisoTipoAlerta = 15,
        [Description("Mto. Gestores Flota")]
        MtoGestoresFlota = 16,
        [Description("Gestores Flota")]
        GestoresFlota = 17,
        [Description("Mto. categorías preguntas frec.")]
        MtoCategoriasPreguntas = 18,
        [Description("Mto. conductores")]
        MtoConductores = 19,
        [Description("Mto. carburante")]
        MtoCarburante = 20,
        [Description("Mto. marcas vehículo")]
        MtoMarcasVehiculo = 21,
        [Description("Mto. modelos vehículo")]
        MtoModelosVehiculo = 22,
        [Description("Mto. rutas vehículo")]
        MtoRutasVehiculo = 23,
        [Description("Mto. tipos seguro vehículo")]
        MtoTiposSeguroVehiculo = 24,
        [Description("Mto. tipos tarjetas combustible")]
        MtoTiposTarjetasCombustible = 25,
        [Description("Mto. ubicaciones")]
        MtoUbicaciones= 26,
        [Description("Mto. tipos vehículo")]
        MtoTiposVehiculo = 27,
        [Description("Mto. empresas renting/aseguradoras")]
        MtoEmpVehiculos = 28,
        [Description("Mto. tarjetas combustible")]
        MtoTarjetasCombustible = 29,
        [Description("Mto. tipos liquidación seguro")]
        MtoTipoLiquidacionSeguro = 30,
        [Description("Importar flota")]
        ImportarFlota = 31,
        [Description("Importar Via Verde")]
        ImportarViaVerde = 32,
        [Description("Importar Consumo Combustible")]
        ImportarConsumoCombustible = 33,
        [Description("Importar LEASING")]
        ImportarLEASING = 34,
        [Description("Facturación")]
        Facturacion = 35,
        [Description("BorrarImportación")]
        BorrarImportacion = 36,
        [Description("Informe flota alta/baja fecha contrato")]
        InfAltaBajaFechaContrato = 37,
    }

    public enum EnumAltasBajasAmbas
    {
        [Description("Altas")]
        Altas = 1,
        [Description("Bajas")]
        Bajas = 2,
        [Description("Todas")]
        Todas = 3
    }

    public enum GenericCompareOperator
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

    public enum EnumMtoTiposGenerales
    {
        Marcas = 1,
        Modelos = 2,
        Vehiculos = 3,
        Ubicaciones = 4,
        Carburante = 5,
        Seguro = 6,
        TarjetaCombustible = 7,
        Ruta = 8,
        TipoLiquidacionSeguro = 9,
    }

    public enum EnumTipoEmpresaVehiculo
    {
        Leasing = 1,
        Seguros = 2,
    }

    public enum EnumTipoLineaImportacion
    {
        [Description("Normal")]
        Normal = 1,
        [Description("MatriculaVacia")]
        MatriculaVacia = 2,
        [Description("NoHayConductor")]
        NoHayConductor = 3,
        [Description("ConductorNuevo")]
        ConductorNuevo = 4,
        [Description("SinFechaMatriculacion")]
        SinFechaMatriculacion = 5,
        [Description("SinTipoDeVehiculo")]
        SinTipoDeVehiculo = 6,
        [Description("VehiculoExistente")]
        VehiculoExistente = 7,
        [Description("ErrorVehiculo")]
        Error = 8,
        [Description("Resumen")]
        Resumen = 9,
        [Description("IncidenciaGenerica")]
        Incidencia = 10,
        [Description("MatriculaInexistente")]
        MatriculaInexistente = 11,
        [Description("ConductorNoEncontrado")]
        ConductorNoEncontrado = 12,
        [Description("ConceptoLeasinNoContemplado")]
        ConceptoLeasinNoContemplado = 13,
    }

    public enum EnumTipoImportacion
    {
        ImportFlota = 1,
        ImportViaVerde = 2,
        ImportGALP = 3,
        ImportLEASEPLAN = 4,
    }

    public enum EnumTipoBorradoImportacion
    {
        BorrarImportacionFlota = 1,
        BorrarImportacionFacuracion = 2,
        BorrarImportacionViaVerde = 3,
        BorrarImportacionCombustible = 4,
    }

}