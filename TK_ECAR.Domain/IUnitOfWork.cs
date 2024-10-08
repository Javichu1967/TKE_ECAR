
namespace TK_ECAR.Domain
{
    public partial interface IUnitOfWork
    {
         
        void Commit();
        bool LazyLoadingEnabled { get; set; }
        bool ProxyCreationEnabled { get; set; }
        string ConnectionString { get; set; }
				IRepositoryCompañias_Seguro RepositoryCompañias_Seguro { get; }
           
		IRepositoryDatos_Conductor RepositoryDatos_Conductor { get; }
           
		IRepositoryDatos_ITV RepositoryDatos_ITV { get; }
           
		IRepositoryDatos_LeasePlan RepositoryDatos_LeasePlan { get; }
           
		IRepositoryDatos_Multas RepositoryDatos_Multas { get; }
           
		IRepositoryDatos_SolRed RepositoryDatos_SolRed { get; }
           
		IRepositoryDatos_Vehiculo RepositoryDatos_Vehiculo { get; }
           
		IRepositoryECAR_Datos_Conductor RepositoryECAR_Datos_Conductor { get; }
           
		IRepositoryECAR_Datos_ITV RepositoryECAR_Datos_ITV { get; }
           
		IRepositoryECAR_Datos_Multas RepositoryECAR_Datos_Multas { get; }
           
		IRepositoryECAR_Datos_SolRed RepositoryECAR_Datos_SolRed { get; }
           
		IRepositoryECAR_Datos_Vehiculo RepositoryECAR_Datos_Vehiculo { get; }
           
		IRepositoryECAR_Tipo_Liquidacion RepositoryECAR_Tipo_Liquidacion { get; }
           
		IRepositoryEmpresas_Leasing RepositoryEmpresas_Leasing { get; }
           
		IRepositoryGENERAL_IDIOMAS RepositoryGENERAL_IDIOMAS { get; }
           
		IRepositoryGENERAL_PARAMETROS_APLICACIONES RepositoryGENERAL_PARAMETROS_APLICACIONES { get; }
           
		IRepositoryMERSY_USERS_ZONAS RepositoryMERSY_USERS_ZONAS { get; }
           
		IRepositoryMERSY_ZONAS RepositoryMERSY_ZONAS { get; }
           
		IRepositorySAPHR_CentrosCoste RepositorySAPHR_CentrosCoste { get; }
           
		IRepositorySAPHR_Delegaciones RepositorySAPHR_Delegaciones { get; }
           
		IRepositorySAPHR_DireccionesArea RepositorySAPHR_DireccionesArea { get; }
           
		IRepositorySAPHR_DireccionesTerritoriales RepositorySAPHR_DireccionesTerritoriales { get; }
           
		IRepositorySAPHR_Empresas RepositorySAPHR_Empresas { get; }
           
		IRepositorySAPHR_FiltroSeleccionCentrosCoste RepositorySAPHR_FiltroSeleccionCentrosCoste { get; }
           
		IRepositorySAPHR_UnidadesOrganizativas RepositorySAPHR_UnidadesOrganizativas { get; }
           
		IRepositorySAPHR_UsuariosSAP RepositorySAPHR_UsuariosSAP { get; }
           
		IRepositoryT_G_ALERTAS RepositoryT_G_ALERTAS { get; }
           
		IRepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR RepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR { get; }
           
		IRepositoryT_G_ALERTAS_MULTA RepositoryT_G_ALERTAS_MULTA { get; }
           
		IRepositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR RepositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR { get; }
           
		IRepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES RepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES { get; }
           
		IRepositoryT_G_ALERTAS_RENOVACION_CARNET RepositoryT_G_ALERTAS_RENOVACION_CARNET { get; }
           
		IRepositoryT_G_ALERTAS_RENOVACION_ITV RepositoryT_G_ALERTAS_RENOVACION_ITV { get; }
           
		IRepositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR RepositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR { get; }
           
		IRepositoryT_G_ALERTAS_ROBO RepositoryT_G_ALERTAS_ROBO { get; }
           
		IRepositoryT_G_ALERTAS_SOLICITUD_SOLRED RepositoryT_G_ALERTAS_SOLICITUD_SOLRED { get; }
           
		IRepositoryT_G_DATOS_LEASING RepositoryT_G_DATOS_LEASING { get; }
           
		IRepositoryT_G_DOCUMENTACION RepositoryT_G_DOCUMENTACION { get; }
           
		IRepositoryT_G_DOCUMENTACION_VEHICULO RepositoryT_G_DOCUMENTACION_VEHICULO { get; }
           
		IRepositoryT_G_GESTORES_FLOTA RepositoryT_G_GESTORES_FLOTA { get; }
           
		IRepositoryT_G_HIST_ALERTAS RepositoryT_G_HIST_ALERTAS { get; }
           
		IRepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE RepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE { get; }
           
		IRepositoryT_G_HIST_CAMBIOS_CONDUCTOR RepositoryT_G_HIST_CAMBIOS_CONDUCTOR { get; }
           
		IRepositoryT_G_HIST_CAMBIOS_EMPRESA RepositoryT_G_HIST_CAMBIOS_EMPRESA { get; }
           
		IRepositoryT_G_HIST_CAMBIOS_TARJETA RepositoryT_G_HIST_CAMBIOS_TARJETA { get; }
           
		IRepositoryT_G_MENUS RepositoryT_G_MENUS { get; }
           
		IRepositoryT_G_PREGUNTAS_FRECUENTES RepositoryT_G_PREGUNTAS_FRECUENTES { get; }
           
		IRepositoryT_G_TARJETA_COMBUSTIBLE RepositoryT_G_TARJETA_COMBUSTIBLE { get; }
           
		IRepositoryT_G_TELEFONOS_FRECUENTES RepositoryT_G_TELEFONOS_FRECUENTES { get; }
           
		IRepositoryT_G_USUARIOS RepositoryT_G_USUARIOS { get; }
           
		IRepositoryT_G_USUARIOS_CECO RepositoryT_G_USUARIOS_CECO { get; }
           
		IRepositoryT_G_USUARIOS_DELEGACION RepositoryT_G_USUARIOS_DELEGACION { get; }
           
		IRepositoryT_G_USUARIOS_DIR_TERRITORIAL RepositoryT_G_USUARIOS_DIR_TERRITORIAL { get; }
           
		IRepositoryT_G_USUARIOS_EMPRESAS RepositoryT_G_USUARIOS_EMPRESAS { get; }
           
		IRepositoryT_G_VIA_VERDE_EXTRACTOS RepositoryT_G_VIA_VERDE_EXTRACTOS { get; }
           
		IRepositoryT_G_VIA_VERDE_IDENTIFICADORES RepositoryT_G_VIA_VERDE_IDENTIFICADORES { get; }
           
		IRepositoryT_G_VIA_VERDE_TRANSACCIONES RepositoryT_G_VIA_VERDE_TRANSACCIONES { get; }
           
		IRepositoryT_M_ACCIONES RepositoryT_M_ACCIONES { get; }
           
		IRepositoryT_M_CATEGORIA_PREGUNTAS RepositoryT_M_CATEGORIA_PREGUNTAS { get; }
           
		IRepositoryT_M_CATEGORIAS RepositoryT_M_CATEGORIAS { get; }
           
		IRepositoryT_M_CLIENTES RepositoryT_M_CLIENTES { get; }
           
		IRepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE RepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE { get; }
           
		IRepositoryT_M_CUENTAS_CONTABLES RepositoryT_M_CUENTAS_CONTABLES { get; }
           
		IRepositoryT_M_EMPRESAS_VEHICULOS RepositoryT_M_EMPRESAS_VEHICULOS { get; }
           
		IRepositoryT_M_ESTADOS RepositoryT_M_ESTADOS { get; }
           
		IRepositoryT_M_MARCA_VEHICULOS RepositoryT_M_MARCA_VEHICULOS { get; }
           
		IRepositoryT_M_MODELOS_VEHICULO RepositoryT_M_MODELOS_VEHICULO { get; }
           
		IRepositoryT_M_PERFILES RepositoryT_M_PERFILES { get; }
           
		IRepositoryT_M_RUTA_VEHICULOS RepositoryT_M_RUTA_VEHICULOS { get; }
           
		IRepositoryT_M_TARJETAS_CONBUSTIBLE RepositoryT_M_TARJETAS_CONBUSTIBLE { get; }
           
		IRepositoryT_M_TIPO_SEGURO_VEHICULO RepositoryT_M_TIPO_SEGURO_VEHICULO { get; }
           
		IRepositoryT_M_TIPO_TARJETA_COMBUSTIBLE RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE { get; }
           
		IRepositoryT_M_TIPOS_ALERTAS RepositoryT_M_TIPOS_ALERTAS { get; }
           
		IRepositoryT_M_TIPOS_CARBURANTE RepositoryT_M_TIPOS_CARBURANTE { get; }
           
		IRepositoryT_M_TIPOS_CLASIFICACION RepositoryT_M_TIPOS_CLASIFICACION { get; }
           
		IRepositoryT_M_TIPOS_COSTE RepositoryT_M_TIPOS_COSTE { get; }
           
		IRepositoryT_M_TIPOS_SOLICITUD_SOLRED RepositoryT_M_TIPOS_SOLICITUD_SOLRED { get; }
           
		IRepositoryT_M_TIPOS_VEHICULO RepositoryT_M_TIPOS_VEHICULO { get; }
           
		IRepositoryT_M_UBICACIONES RepositoryT_M_UBICACIONES { get; }
           
		IRepositoryT_R_ESTADOS_ACCION RepositoryT_R_ESTADOS_ACCION { get; }
           
		IRepositoryT_R_PERFILES_MENU RepositoryT_R_PERFILES_MENU { get; }
           
		IRepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE RepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE { get; }
           
		IRepositoryTipo_Liquidacion RepositoryTipo_Liquidacion { get; }
           
		IRepositoryTipo_Seguro RepositoryTipo_Seguro { get; }
           
		IRepositoryTipo_Vehiculo RepositoryTipo_Vehiculo { get; }
           
		IRepositoryTMP_RENOVACION_ITV RepositoryTMP_RENOVACION_ITV { get; }
           
		IRepositoryV_ALERTAS_CECOS_DELEGACION RepositoryV_ALERTAS_CECOS_DELEGACION { get; }
           
		IRepositoryV_CONDUCTORES_USUARIOS_SAP RepositoryV_CONDUCTORES_USUARIOS_SAP { get; }
           
		IRepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS RepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS { get; }
           


    }
}
 
  
