using System;
using TK_ECAR.Domain;

namespace TK_ECAR.Infraestructure
{
    public partial class UnitOfWork :  IUnitOfWork, IDisposable
    {

				ModelEntities _context_ModelEntities = null;
        private  ModelEntities context_ModelEntities
        {
            get
            {
                if (_context_ModelEntities == null)
                    _context_ModelEntities = new ModelEntities();

                return _context_ModelEntities;
            }
        }

		 #region Repositories
	
			private IRepositoryT_G_ALERTAS repositoryT_G_ALERTAS;
			  
			public IRepositoryT_G_ALERTAS RepositoryT_G_ALERTAS
			{
				get
				{

					if (repositoryT_G_ALERTAS == null)
					{
						repositoryT_G_ALERTAS = new RepositoryT_G_ALERTAS(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS;
				}
			}

		
			private IRepositoryT_G_ALERTAS_RENOVACION_CARNET repositoryT_G_ALERTAS_RENOVACION_CARNET;
			  
			public IRepositoryT_G_ALERTAS_RENOVACION_CARNET RepositoryT_G_ALERTAS_RENOVACION_CARNET
			{
				get
				{

					if (repositoryT_G_ALERTAS_RENOVACION_CARNET == null)
					{
						repositoryT_G_ALERTAS_RENOVACION_CARNET = new RepositoryT_G_ALERTAS_RENOVACION_CARNET(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_RENOVACION_CARNET;
				}
			}

		
			private IRepositoryT_G_ALERTAS_RENOVACION_ITV repositoryT_G_ALERTAS_RENOVACION_ITV;
			  
			public IRepositoryT_G_ALERTAS_RENOVACION_ITV RepositoryT_G_ALERTAS_RENOVACION_ITV
			{
				get
				{

					if (repositoryT_G_ALERTAS_RENOVACION_ITV == null)
					{
						repositoryT_G_ALERTAS_RENOVACION_ITV = new RepositoryT_G_ALERTAS_RENOVACION_ITV(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_RENOVACION_ITV;
				}
			}

		
			private IRepositoryT_G_ALERTAS_ROBO repositoryT_G_ALERTAS_ROBO;
			  
			public IRepositoryT_G_ALERTAS_ROBO RepositoryT_G_ALERTAS_ROBO
			{
				get
				{

					if (repositoryT_G_ALERTAS_ROBO == null)
					{
						repositoryT_G_ALERTAS_ROBO = new RepositoryT_G_ALERTAS_ROBO(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_ROBO;
				}
			}

		
			private IRepositoryT_G_ALERTAS_SOLICITUD_SOLRED repositoryT_G_ALERTAS_SOLICITUD_SOLRED;
			  
			public IRepositoryT_G_ALERTAS_SOLICITUD_SOLRED RepositoryT_G_ALERTAS_SOLICITUD_SOLRED
			{
				get
				{

					if (repositoryT_G_ALERTAS_SOLICITUD_SOLRED == null)
					{
						repositoryT_G_ALERTAS_SOLICITUD_SOLRED = new RepositoryT_G_ALERTAS_SOLICITUD_SOLRED(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_SOLICITUD_SOLRED;
				}
			}

		
			private IRepositoryT_G_DOCUMENTACION repositoryT_G_DOCUMENTACION;
			  
			public IRepositoryT_G_DOCUMENTACION RepositoryT_G_DOCUMENTACION
			{
				get
				{

					if (repositoryT_G_DOCUMENTACION == null)
					{
						repositoryT_G_DOCUMENTACION = new RepositoryT_G_DOCUMENTACION(context_ModelEntities);
					}
					return repositoryT_G_DOCUMENTACION;
				}
			}

		
			private IRepositoryT_G_DOCUMENTACION_VEHICULO repositoryT_G_DOCUMENTACION_VEHICULO;
			  
			public IRepositoryT_G_DOCUMENTACION_VEHICULO RepositoryT_G_DOCUMENTACION_VEHICULO
			{
				get
				{

					if (repositoryT_G_DOCUMENTACION_VEHICULO == null)
					{
						repositoryT_G_DOCUMENTACION_VEHICULO = new RepositoryT_G_DOCUMENTACION_VEHICULO(context_ModelEntities);
					}
					return repositoryT_G_DOCUMENTACION_VEHICULO;
				}
			}

		
			private IRepositoryT_G_HIST_ALERTAS repositoryT_G_HIST_ALERTAS;
			  
			public IRepositoryT_G_HIST_ALERTAS RepositoryT_G_HIST_ALERTAS
			{
				get
				{

					if (repositoryT_G_HIST_ALERTAS == null)
					{
						repositoryT_G_HIST_ALERTAS = new RepositoryT_G_HIST_ALERTAS(context_ModelEntities);
					}
					return repositoryT_G_HIST_ALERTAS;
				}
			}

		
			private IRepositoryT_G_HIST_CAMBIOS_CONDUCTOR repositoryT_G_HIST_CAMBIOS_CONDUCTOR;
			  
			public IRepositoryT_G_HIST_CAMBIOS_CONDUCTOR RepositoryT_G_HIST_CAMBIOS_CONDUCTOR
			{
				get
				{

					if (repositoryT_G_HIST_CAMBIOS_CONDUCTOR == null)
					{
						repositoryT_G_HIST_CAMBIOS_CONDUCTOR = new RepositoryT_G_HIST_CAMBIOS_CONDUCTOR(context_ModelEntities);
					}
					return repositoryT_G_HIST_CAMBIOS_CONDUCTOR;
				}
			}

		
			private IRepositoryT_G_MENUS repositoryT_G_MENUS;
			  
			public IRepositoryT_G_MENUS RepositoryT_G_MENUS
			{
				get
				{

					if (repositoryT_G_MENUS == null)
					{
						repositoryT_G_MENUS = new RepositoryT_G_MENUS(context_ModelEntities);
					}
					return repositoryT_G_MENUS;
				}
			}

		
			private IRepositoryT_G_USUARIOS repositoryT_G_USUARIOS;
			  
			public IRepositoryT_G_USUARIOS RepositoryT_G_USUARIOS
			{
				get
				{

					if (repositoryT_G_USUARIOS == null)
					{
						repositoryT_G_USUARIOS = new RepositoryT_G_USUARIOS(context_ModelEntities);
					}
					return repositoryT_G_USUARIOS;
				}
			}

		
			private IRepositoryT_G_USUARIOS_CECO repositoryT_G_USUARIOS_CECO;
			  
			public IRepositoryT_G_USUARIOS_CECO RepositoryT_G_USUARIOS_CECO
			{
				get
				{

					if (repositoryT_G_USUARIOS_CECO == null)
					{
						repositoryT_G_USUARIOS_CECO = new RepositoryT_G_USUARIOS_CECO(context_ModelEntities);
					}
					return repositoryT_G_USUARIOS_CECO;
				}
			}

		
			private IRepositoryT_G_USUARIOS_DELEGACION repositoryT_G_USUARIOS_DELEGACION;
			  
			public IRepositoryT_G_USUARIOS_DELEGACION RepositoryT_G_USUARIOS_DELEGACION
			{
				get
				{

					if (repositoryT_G_USUARIOS_DELEGACION == null)
					{
						repositoryT_G_USUARIOS_DELEGACION = new RepositoryT_G_USUARIOS_DELEGACION(context_ModelEntities);
					}
					return repositoryT_G_USUARIOS_DELEGACION;
				}
			}

		
			private IRepositoryT_G_USUARIOS_DIR_TERRITORIAL repositoryT_G_USUARIOS_DIR_TERRITORIAL;
			  
			public IRepositoryT_G_USUARIOS_DIR_TERRITORIAL RepositoryT_G_USUARIOS_DIR_TERRITORIAL
			{
				get
				{

					if (repositoryT_G_USUARIOS_DIR_TERRITORIAL == null)
					{
						repositoryT_G_USUARIOS_DIR_TERRITORIAL = new RepositoryT_G_USUARIOS_DIR_TERRITORIAL(context_ModelEntities);
					}
					return repositoryT_G_USUARIOS_DIR_TERRITORIAL;
				}
			}

		
			private IRepositoryT_G_USUARIOS_EMPRESAS repositoryT_G_USUARIOS_EMPRESAS;
			  
			public IRepositoryT_G_USUARIOS_EMPRESAS RepositoryT_G_USUARIOS_EMPRESAS
			{
				get
				{

					if (repositoryT_G_USUARIOS_EMPRESAS == null)
					{
						repositoryT_G_USUARIOS_EMPRESAS = new RepositoryT_G_USUARIOS_EMPRESAS(context_ModelEntities);
					}
					return repositoryT_G_USUARIOS_EMPRESAS;
				}
			}

		
			private IRepositoryT_M_ACCIONES repositoryT_M_ACCIONES;
			  
			public IRepositoryT_M_ACCIONES RepositoryT_M_ACCIONES
			{
				get
				{

					if (repositoryT_M_ACCIONES == null)
					{
						repositoryT_M_ACCIONES = new RepositoryT_M_ACCIONES(context_ModelEntities);
					}
					return repositoryT_M_ACCIONES;
				}
			}

		
			private IRepositoryT_M_CATEGORIAS repositoryT_M_CATEGORIAS;
			  
			public IRepositoryT_M_CATEGORIAS RepositoryT_M_CATEGORIAS
			{
				get
				{

					if (repositoryT_M_CATEGORIAS == null)
					{
						repositoryT_M_CATEGORIAS = new RepositoryT_M_CATEGORIAS(context_ModelEntities);
					}
					return repositoryT_M_CATEGORIAS;
				}
			}

		
			private IRepositoryT_M_ESTADOS repositoryT_M_ESTADOS;
			  
			public IRepositoryT_M_ESTADOS RepositoryT_M_ESTADOS
			{
				get
				{

					if (repositoryT_M_ESTADOS == null)
					{
						repositoryT_M_ESTADOS = new RepositoryT_M_ESTADOS(context_ModelEntities);
					}
					return repositoryT_M_ESTADOS;
				}
			}

		
			private IRepositoryT_M_PERFILES repositoryT_M_PERFILES;
			  
			public IRepositoryT_M_PERFILES RepositoryT_M_PERFILES
			{
				get
				{

					if (repositoryT_M_PERFILES == null)
					{
						repositoryT_M_PERFILES = new RepositoryT_M_PERFILES(context_ModelEntities);
					}
					return repositoryT_M_PERFILES;
				}
			}

		
			private IRepositoryT_M_TIPOS_ALERTAS repositoryT_M_TIPOS_ALERTAS;
			  
			public IRepositoryT_M_TIPOS_ALERTAS RepositoryT_M_TIPOS_ALERTAS
			{
				get
				{

					if (repositoryT_M_TIPOS_ALERTAS == null)
					{
						repositoryT_M_TIPOS_ALERTAS = new RepositoryT_M_TIPOS_ALERTAS(context_ModelEntities);
					}
					return repositoryT_M_TIPOS_ALERTAS;
				}
			}

		
			private IRepositoryT_M_TIPOS_SOLICITUD_SOLRED repositoryT_M_TIPOS_SOLICITUD_SOLRED;
			  
			public IRepositoryT_M_TIPOS_SOLICITUD_SOLRED RepositoryT_M_TIPOS_SOLICITUD_SOLRED
			{
				get
				{

					if (repositoryT_M_TIPOS_SOLICITUD_SOLRED == null)
					{
						repositoryT_M_TIPOS_SOLICITUD_SOLRED = new RepositoryT_M_TIPOS_SOLICITUD_SOLRED(context_ModelEntities);
					}
					return repositoryT_M_TIPOS_SOLICITUD_SOLRED;
				}
			}

		
			private IRepositoryT_R_PERFILES_MENU repositoryT_R_PERFILES_MENU;
			  
			public IRepositoryT_R_PERFILES_MENU RepositoryT_R_PERFILES_MENU
			{
				get
				{

					if (repositoryT_R_PERFILES_MENU == null)
					{
						repositoryT_R_PERFILES_MENU = new RepositoryT_R_PERFILES_MENU(context_ModelEntities);
					}
					return repositoryT_R_PERFILES_MENU;
				}
			}

		
			private IRepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES repositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES;
			  
			public IRepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES RepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES
			{
				get
				{

					if (repositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES == null)
					{
						repositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES = new RepositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_OTRAS_NOTIFICACIONES;
				}
			}

		
			private IRepositoryT_M_TIPOS_CLASIFICACION repositoryT_M_TIPOS_CLASIFICACION;
			  
			public IRepositoryT_M_TIPOS_CLASIFICACION RepositoryT_M_TIPOS_CLASIFICACION
			{
				get
				{

					if (repositoryT_M_TIPOS_CLASIFICACION == null)
					{
						repositoryT_M_TIPOS_CLASIFICACION = new RepositoryT_M_TIPOS_CLASIFICACION(context_ModelEntities);
					}
					return repositoryT_M_TIPOS_CLASIFICACION;
				}
			}

		
			private IRepositoryT_G_GESTORES_FLOTA repositoryT_G_GESTORES_FLOTA;
			  
			public IRepositoryT_G_GESTORES_FLOTA RepositoryT_G_GESTORES_FLOTA
			{
				get
				{

					if (repositoryT_G_GESTORES_FLOTA == null)
					{
						repositoryT_G_GESTORES_FLOTA = new RepositoryT_G_GESTORES_FLOTA(context_ModelEntities);
					}
					return repositoryT_G_GESTORES_FLOTA;
				}
			}

		
			private IRepositoryT_R_ESTADOS_ACCION repositoryT_R_ESTADOS_ACCION;
			  
			public IRepositoryT_R_ESTADOS_ACCION RepositoryT_R_ESTADOS_ACCION
			{
				get
				{

					if (repositoryT_R_ESTADOS_ACCION == null)
					{
						repositoryT_R_ESTADOS_ACCION = new RepositoryT_R_ESTADOS_ACCION(context_ModelEntities);
					}
					return repositoryT_R_ESTADOS_ACCION;
				}
			}

		
			private IRepositoryT_G_VIA_VERDE_EXTRACTOS repositoryT_G_VIA_VERDE_EXTRACTOS;
			  
			public IRepositoryT_G_VIA_VERDE_EXTRACTOS RepositoryT_G_VIA_VERDE_EXTRACTOS
			{
				get
				{

					if (repositoryT_G_VIA_VERDE_EXTRACTOS == null)
					{
						repositoryT_G_VIA_VERDE_EXTRACTOS = new RepositoryT_G_VIA_VERDE_EXTRACTOS(context_ModelEntities);
					}
					return repositoryT_G_VIA_VERDE_EXTRACTOS;
				}
			}

		
			private IRepositoryT_G_VIA_VERDE_IDENTIFICADORES repositoryT_G_VIA_VERDE_IDENTIFICADORES;
			  
			public IRepositoryT_G_VIA_VERDE_IDENTIFICADORES RepositoryT_G_VIA_VERDE_IDENTIFICADORES
			{
				get
				{

					if (repositoryT_G_VIA_VERDE_IDENTIFICADORES == null)
					{
						repositoryT_G_VIA_VERDE_IDENTIFICADORES = new RepositoryT_G_VIA_VERDE_IDENTIFICADORES(context_ModelEntities);
					}
					return repositoryT_G_VIA_VERDE_IDENTIFICADORES;
				}
			}

		
			private IRepositoryT_M_CLIENTES repositoryT_M_CLIENTES;
			  
			public IRepositoryT_M_CLIENTES RepositoryT_M_CLIENTES
			{
				get
				{

					if (repositoryT_M_CLIENTES == null)
					{
						repositoryT_M_CLIENTES = new RepositoryT_M_CLIENTES(context_ModelEntities);
					}
					return repositoryT_M_CLIENTES;
				}
			}

		
			private IRepositoryT_G_VIA_VERDE_TRANSACCIONES repositoryT_G_VIA_VERDE_TRANSACCIONES;
			  
			public IRepositoryT_G_VIA_VERDE_TRANSACCIONES RepositoryT_G_VIA_VERDE_TRANSACCIONES
			{
				get
				{

					if (repositoryT_G_VIA_VERDE_TRANSACCIONES == null)
					{
						repositoryT_G_VIA_VERDE_TRANSACCIONES = new RepositoryT_G_VIA_VERDE_TRANSACCIONES(context_ModelEntities);
					}
					return repositoryT_G_VIA_VERDE_TRANSACCIONES;
				}
			}

		
			private IRepositoryT_G_TELEFONOS_FRECUENTES repositoryT_G_TELEFONOS_FRECUENTES;
			  
			public IRepositoryT_G_TELEFONOS_FRECUENTES RepositoryT_G_TELEFONOS_FRECUENTES
			{
				get
				{

					if (repositoryT_G_TELEFONOS_FRECUENTES == null)
					{
						repositoryT_G_TELEFONOS_FRECUENTES = new RepositoryT_G_TELEFONOS_FRECUENTES(context_ModelEntities);
					}
					return repositoryT_G_TELEFONOS_FRECUENTES;
				}
			}

		
			private IRepositoryT_M_CATEGORIA_PREGUNTAS repositoryT_M_CATEGORIA_PREGUNTAS;
			  
			public IRepositoryT_M_CATEGORIA_PREGUNTAS RepositoryT_M_CATEGORIA_PREGUNTAS
			{
				get
				{

					if (repositoryT_M_CATEGORIA_PREGUNTAS == null)
					{
						repositoryT_M_CATEGORIA_PREGUNTAS = new RepositoryT_M_CATEGORIA_PREGUNTAS(context_ModelEntities);
					}
					return repositoryT_M_CATEGORIA_PREGUNTAS;
				}
			}

		
			private IRepositoryT_G_PREGUNTAS_FRECUENTES repositoryT_G_PREGUNTAS_FRECUENTES;
			  
			public IRepositoryT_G_PREGUNTAS_FRECUENTES RepositoryT_G_PREGUNTAS_FRECUENTES
			{
				get
				{

					if (repositoryT_G_PREGUNTAS_FRECUENTES == null)
					{
						repositoryT_G_PREGUNTAS_FRECUENTES = new RepositoryT_G_PREGUNTAS_FRECUENTES(context_ModelEntities);
					}
					return repositoryT_G_PREGUNTAS_FRECUENTES;
				}
			}

		
			private IRepositoryT_M_MARCA_VEHICULOS repositoryT_M_MARCA_VEHICULOS;
			  
			public IRepositoryT_M_MARCA_VEHICULOS RepositoryT_M_MARCA_VEHICULOS
			{
				get
				{

					if (repositoryT_M_MARCA_VEHICULOS == null)
					{
						repositoryT_M_MARCA_VEHICULOS = new RepositoryT_M_MARCA_VEHICULOS(context_ModelEntities);
					}
					return repositoryT_M_MARCA_VEHICULOS;
				}
			}

		
			private IRepositoryT_M_MODELOS_VEHICULO repositoryT_M_MODELOS_VEHICULO;
			  
			public IRepositoryT_M_MODELOS_VEHICULO RepositoryT_M_MODELOS_VEHICULO
			{
				get
				{

					if (repositoryT_M_MODELOS_VEHICULO == null)
					{
						repositoryT_M_MODELOS_VEHICULO = new RepositoryT_M_MODELOS_VEHICULO(context_ModelEntities);
					}
					return repositoryT_M_MODELOS_VEHICULO;
				}
			}

		
			private IRepositoryT_M_RUTA_VEHICULOS repositoryT_M_RUTA_VEHICULOS;
			  
			public IRepositoryT_M_RUTA_VEHICULOS RepositoryT_M_RUTA_VEHICULOS
			{
				get
				{

					if (repositoryT_M_RUTA_VEHICULOS == null)
					{
						repositoryT_M_RUTA_VEHICULOS = new RepositoryT_M_RUTA_VEHICULOS(context_ModelEntities);
					}
					return repositoryT_M_RUTA_VEHICULOS;
				}
			}

		
			private IRepositoryT_M_TIPOS_CARBURANTE repositoryT_M_TIPOS_CARBURANTE;
			  
			public IRepositoryT_M_TIPOS_CARBURANTE RepositoryT_M_TIPOS_CARBURANTE
			{
				get
				{

					if (repositoryT_M_TIPOS_CARBURANTE == null)
					{
						repositoryT_M_TIPOS_CARBURANTE = new RepositoryT_M_TIPOS_CARBURANTE(context_ModelEntities);
					}
					return repositoryT_M_TIPOS_CARBURANTE;
				}
			}

		
			private IRepositoryT_M_TIPOS_VEHICULO repositoryT_M_TIPOS_VEHICULO;
			  
			public IRepositoryT_M_TIPOS_VEHICULO RepositoryT_M_TIPOS_VEHICULO
			{
				get
				{

					if (repositoryT_M_TIPOS_VEHICULO == null)
					{
						repositoryT_M_TIPOS_VEHICULO = new RepositoryT_M_TIPOS_VEHICULO(context_ModelEntities);
					}
					return repositoryT_M_TIPOS_VEHICULO;
				}
			}

		
			private IRepositoryT_M_UBICACIONES repositoryT_M_UBICACIONES;
			  
			public IRepositoryT_M_UBICACIONES RepositoryT_M_UBICACIONES
			{
				get
				{

					if (repositoryT_M_UBICACIONES == null)
					{
						repositoryT_M_UBICACIONES = new RepositoryT_M_UBICACIONES(context_ModelEntities);
					}
					return repositoryT_M_UBICACIONES;
				}
			}

		
			private IRepositoryT_M_TIPO_SEGURO_VEHICULO repositoryT_M_TIPO_SEGURO_VEHICULO;
			  
			public IRepositoryT_M_TIPO_SEGURO_VEHICULO RepositoryT_M_TIPO_SEGURO_VEHICULO
			{
				get
				{

					if (repositoryT_M_TIPO_SEGURO_VEHICULO == null)
					{
						repositoryT_M_TIPO_SEGURO_VEHICULO = new RepositoryT_M_TIPO_SEGURO_VEHICULO(context_ModelEntities);
					}
					return repositoryT_M_TIPO_SEGURO_VEHICULO;
				}
			}

		
			private IRepositoryT_M_TIPO_TARJETA_COMBUSTIBLE repositoryT_M_TIPO_TARJETA_COMBUSTIBLE;
			  
			public IRepositoryT_M_TIPO_TARJETA_COMBUSTIBLE RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE
			{
				get
				{

					if (repositoryT_M_TIPO_TARJETA_COMBUSTIBLE == null)
					{
						repositoryT_M_TIPO_TARJETA_COMBUSTIBLE = new RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE(context_ModelEntities);
					}
					return repositoryT_M_TIPO_TARJETA_COMBUSTIBLE;
				}
			}

		
			private IRepositoryV_ALERTAS_CECOS_DELEGACION repositoryV_ALERTAS_CECOS_DELEGACION;
			  
			public IRepositoryV_ALERTAS_CECOS_DELEGACION RepositoryV_ALERTAS_CECOS_DELEGACION
			{
				get
				{

					if (repositoryV_ALERTAS_CECOS_DELEGACION == null)
					{
						repositoryV_ALERTAS_CECOS_DELEGACION = new RepositoryV_ALERTAS_CECOS_DELEGACION(context_ModelEntities);
					}
					return repositoryV_ALERTAS_CECOS_DELEGACION;
				}
			}

		
			private IRepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS repositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS;
			  
			public IRepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS RepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS
			{
				get
				{

					if (repositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS == null)
					{
						repositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS = new RepositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS(context_ModelEntities);
					}
					return repositoryV_TARJETAS_COMBUSTIBLE_EMPRESAS;
				}
			}

		
			private IRepositoryECAR_Datos_Conductor repositoryECAR_Datos_Conductor;
			  
			public IRepositoryECAR_Datos_Conductor RepositoryECAR_Datos_Conductor
			{
				get
				{

					if (repositoryECAR_Datos_Conductor == null)
					{
						repositoryECAR_Datos_Conductor = new RepositoryECAR_Datos_Conductor(context_ModelEntities);
					}
					return repositoryECAR_Datos_Conductor;
				}
			}

		
			private IRepositoryECAR_Tipo_Liquidacion repositoryECAR_Tipo_Liquidacion;
			  
			public IRepositoryECAR_Tipo_Liquidacion RepositoryECAR_Tipo_Liquidacion
			{
				get
				{

					if (repositoryECAR_Tipo_Liquidacion == null)
					{
						repositoryECAR_Tipo_Liquidacion = new RepositoryECAR_Tipo_Liquidacion(context_ModelEntities);
					}
					return repositoryECAR_Tipo_Liquidacion;
				}
			}

		
			private IRepositoryECAR_Datos_Vehiculo repositoryECAR_Datos_Vehiculo;
			  
			public IRepositoryECAR_Datos_Vehiculo RepositoryECAR_Datos_Vehiculo
			{
				get
				{

					if (repositoryECAR_Datos_Vehiculo == null)
					{
						repositoryECAR_Datos_Vehiculo = new RepositoryECAR_Datos_Vehiculo(context_ModelEntities);
					}
					return repositoryECAR_Datos_Vehiculo;
				}
			}

		
			private IRepositoryECAR_Datos_ITV repositoryECAR_Datos_ITV;
			  
			public IRepositoryECAR_Datos_ITV RepositoryECAR_Datos_ITV
			{
				get
				{

					if (repositoryECAR_Datos_ITV == null)
					{
						repositoryECAR_Datos_ITV = new RepositoryECAR_Datos_ITV(context_ModelEntities);
					}
					return repositoryECAR_Datos_ITV;
				}
			}

		
			private IRepositoryTMP_RENOVACION_ITV repositoryTMP_RENOVACION_ITV;
			  
			public IRepositoryTMP_RENOVACION_ITV RepositoryTMP_RENOVACION_ITV
			{
				get
				{

					if (repositoryTMP_RENOVACION_ITV == null)
					{
						repositoryTMP_RENOVACION_ITV = new RepositoryTMP_RENOVACION_ITV(context_ModelEntities);
					}
					return repositoryTMP_RENOVACION_ITV;
				}
			}

		
			private IRepositoryT_G_HIST_CAMBIOS_TARJETA repositoryT_G_HIST_CAMBIOS_TARJETA;
			  
			public IRepositoryT_G_HIST_CAMBIOS_TARJETA RepositoryT_G_HIST_CAMBIOS_TARJETA
			{
				get
				{

					if (repositoryT_G_HIST_CAMBIOS_TARJETA == null)
					{
						repositoryT_G_HIST_CAMBIOS_TARJETA = new RepositoryT_G_HIST_CAMBIOS_TARJETA(context_ModelEntities);
					}
					return repositoryT_G_HIST_CAMBIOS_TARJETA;
				}
			}

		
			private IRepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE repositoryT_G_HIST_CAMBIOS_CENTRO_COSTE;
			  
			public IRepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE RepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE
			{
				get
				{

					if (repositoryT_G_HIST_CAMBIOS_CENTRO_COSTE == null)
					{
						repositoryT_G_HIST_CAMBIOS_CENTRO_COSTE = new RepositoryT_G_HIST_CAMBIOS_CENTRO_COSTE(context_ModelEntities);
					}
					return repositoryT_G_HIST_CAMBIOS_CENTRO_COSTE;
				}
			}

		
			private IRepositoryECAR_Datos_SolRed repositoryECAR_Datos_SolRed;
			  
			public IRepositoryECAR_Datos_SolRed RepositoryECAR_Datos_SolRed
			{
				get
				{

					if (repositoryECAR_Datos_SolRed == null)
					{
						repositoryECAR_Datos_SolRed = new RepositoryECAR_Datos_SolRed(context_ModelEntities);
					}
					return repositoryECAR_Datos_SolRed;
				}
			}

		
			private IRepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR repositoryT_G_ALERTAS_CAMBIO_CONDUCTOR;
			  
			public IRepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR RepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR
			{
				get
				{

					if (repositoryT_G_ALERTAS_CAMBIO_CONDUCTOR == null)
					{
						repositoryT_G_ALERTAS_CAMBIO_CONDUCTOR = new RepositoryT_G_ALERTAS_CAMBIO_CONDUCTOR(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_CAMBIO_CONDUCTOR;
				}
			}

		
			private IRepositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR repositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR;
			  
			public IRepositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR RepositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR
			{
				get
				{

					if (repositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR == null)
					{
						repositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR = new RepositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_MULTA_CONFIRMACION_CONDUCTOR;
				}
			}

		
			private IRepositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR repositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR;
			  
			public IRepositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR RepositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR
			{
				get
				{

					if (repositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR == null)
					{
						repositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR = new RepositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_RENTING_CONFIRMACION_CONDUCTOR;
				}
			}

		
			private IRepositoryT_G_DATOS_LEASING repositoryT_G_DATOS_LEASING;
			  
			public IRepositoryT_G_DATOS_LEASING RepositoryT_G_DATOS_LEASING
			{
				get
				{

					if (repositoryT_G_DATOS_LEASING == null)
					{
						repositoryT_G_DATOS_LEASING = new RepositoryT_G_DATOS_LEASING(context_ModelEntities);
					}
					return repositoryT_G_DATOS_LEASING;
				}
			}

		
			private IRepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE repositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE;
			  
			public IRepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE RepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE
			{
				get
				{

					if (repositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE == null)
					{
						repositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE = new RepositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE(context_ModelEntities);
					}
					return repositoryT_M_CORRESPONDENCIA_COD_CLI_EMPRESA_TKE;
				}
			}

		
			private IRepositoryT_G_TARJETA_COMBUSTIBLE repositoryT_G_TARJETA_COMBUSTIBLE;
			  
			public IRepositoryT_G_TARJETA_COMBUSTIBLE RepositoryT_G_TARJETA_COMBUSTIBLE
			{
				get
				{

					if (repositoryT_G_TARJETA_COMBUSTIBLE == null)
					{
						repositoryT_G_TARJETA_COMBUSTIBLE = new RepositoryT_G_TARJETA_COMBUSTIBLE(context_ModelEntities);
					}
					return repositoryT_G_TARJETA_COMBUSTIBLE;
				}
			}

		
			private IRepositoryT_M_CUENTAS_CONTABLES repositoryT_M_CUENTAS_CONTABLES;
			  
			public IRepositoryT_M_CUENTAS_CONTABLES RepositoryT_M_CUENTAS_CONTABLES
			{
				get
				{

					if (repositoryT_M_CUENTAS_CONTABLES == null)
					{
						repositoryT_M_CUENTAS_CONTABLES = new RepositoryT_M_CUENTAS_CONTABLES(context_ModelEntities);
					}
					return repositoryT_M_CUENTAS_CONTABLES;
				}
			}

		
			private IRepositoryT_M_TIPOS_COSTE repositoryT_M_TIPOS_COSTE;
			  
			public IRepositoryT_M_TIPOS_COSTE RepositoryT_M_TIPOS_COSTE
			{
				get
				{

					if (repositoryT_M_TIPOS_COSTE == null)
					{
						repositoryT_M_TIPOS_COSTE = new RepositoryT_M_TIPOS_COSTE(context_ModelEntities);
					}
					return repositoryT_M_TIPOS_COSTE;
				}
			}

		
			private IRepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE repositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE;
			  
			public IRepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE RepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE
			{
				get
				{

					if (repositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE == null)
					{
						repositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE = new RepositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE(context_ModelEntities);
					}
					return repositoryT_R_TIPOSCOSTE_CUENTA_CONTABLE;
				}
			}

		
			private IRepositoryT_M_EMPRESAS_VEHICULOS repositoryT_M_EMPRESAS_VEHICULOS;
			  
			public IRepositoryT_M_EMPRESAS_VEHICULOS RepositoryT_M_EMPRESAS_VEHICULOS
			{
				get
				{

					if (repositoryT_M_EMPRESAS_VEHICULOS == null)
					{
						repositoryT_M_EMPRESAS_VEHICULOS = new RepositoryT_M_EMPRESAS_VEHICULOS(context_ModelEntities);
					}
					return repositoryT_M_EMPRESAS_VEHICULOS;
				}
			}

		
			private IRepositoryT_M_TARJETAS_CONBUSTIBLE repositoryT_M_TARJETAS_CONBUSTIBLE;
			  
			public IRepositoryT_M_TARJETAS_CONBUSTIBLE RepositoryT_M_TARJETAS_CONBUSTIBLE
			{
				get
				{

					if (repositoryT_M_TARJETAS_CONBUSTIBLE == null)
					{
						repositoryT_M_TARJETAS_CONBUSTIBLE = new RepositoryT_M_TARJETAS_CONBUSTIBLE(context_ModelEntities);
					}
					return repositoryT_M_TARJETAS_CONBUSTIBLE;
				}
			}

		
			private IRepositoryT_G_ALERTAS_MULTA repositoryT_G_ALERTAS_MULTA;
			  
			public IRepositoryT_G_ALERTAS_MULTA RepositoryT_G_ALERTAS_MULTA
			{
				get
				{

					if (repositoryT_G_ALERTAS_MULTA == null)
					{
						repositoryT_G_ALERTAS_MULTA = new RepositoryT_G_ALERTAS_MULTA(context_ModelEntities);
					}
					return repositoryT_G_ALERTAS_MULTA;
				}
			}

		
			private IRepositoryT_G_HIST_CAMBIOS_EMPRESA repositoryT_G_HIST_CAMBIOS_EMPRESA;
			  
			public IRepositoryT_G_HIST_CAMBIOS_EMPRESA RepositoryT_G_HIST_CAMBIOS_EMPRESA
			{
				get
				{

					if (repositoryT_G_HIST_CAMBIOS_EMPRESA == null)
					{
						repositoryT_G_HIST_CAMBIOS_EMPRESA = new RepositoryT_G_HIST_CAMBIOS_EMPRESA(context_ModelEntities);
					}
					return repositoryT_G_HIST_CAMBIOS_EMPRESA;
				}
			}

		
			private IRepositoryECAR_Datos_Multas repositoryECAR_Datos_Multas;
			  
			public IRepositoryECAR_Datos_Multas RepositoryECAR_Datos_Multas
			{
				get
				{

					if (repositoryECAR_Datos_Multas == null)
					{
						repositoryECAR_Datos_Multas = new RepositoryECAR_Datos_Multas(context_ModelEntities);
					}
					return repositoryECAR_Datos_Multas;
				}
			}

		
			private IRepositoryV_CONDUCTORES_USUARIOS_SAP repositoryV_CONDUCTORES_USUARIOS_SAP;
			  
			public IRepositoryV_CONDUCTORES_USUARIOS_SAP RepositoryV_CONDUCTORES_USUARIOS_SAP
			{
				get
				{

					if (repositoryV_CONDUCTORES_USUARIOS_SAP == null)
					{
						repositoryV_CONDUCTORES_USUARIOS_SAP = new RepositoryV_CONDUCTORES_USUARIOS_SAP(context_ModelEntities);
					}
					return repositoryV_CONDUCTORES_USUARIOS_SAP;
				}
			}

		 
		
		#endregion
			CoreaEntities _context_CoreaEntities = null;
        private  CoreaEntities context_CoreaEntities
        {
            get
            {
                if (_context_CoreaEntities == null)
                    _context_CoreaEntities = new CoreaEntities();

                return _context_CoreaEntities;
            }
        }

		 #region Repositories
	
			private IRepositoryCompañias_Seguro repositoryCompañias_Seguro;
			  
			public IRepositoryCompañias_Seguro RepositoryCompañias_Seguro
			{
				get
				{

					if (repositoryCompañias_Seguro == null)
					{
						repositoryCompañias_Seguro = new RepositoryCompañias_Seguro(context_CoreaEntities);
					}
					return repositoryCompañias_Seguro;
				}
			}

		
			private IRepositoryDatos_SolRed repositoryDatos_SolRed;
			  
			public IRepositoryDatos_SolRed RepositoryDatos_SolRed
			{
				get
				{

					if (repositoryDatos_SolRed == null)
					{
						repositoryDatos_SolRed = new RepositoryDatos_SolRed(context_CoreaEntities);
					}
					return repositoryDatos_SolRed;
				}
			}

		
			private IRepositoryDatos_Conductor repositoryDatos_Conductor;
			  
			public IRepositoryDatos_Conductor RepositoryDatos_Conductor
			{
				get
				{

					if (repositoryDatos_Conductor == null)
					{
						repositoryDatos_Conductor = new RepositoryDatos_Conductor(context_CoreaEntities);
					}
					return repositoryDatos_Conductor;
				}
			}

		
			private IRepositoryDatos_ITV repositoryDatos_ITV;
			  
			public IRepositoryDatos_ITV RepositoryDatos_ITV
			{
				get
				{

					if (repositoryDatos_ITV == null)
					{
						repositoryDatos_ITV = new RepositoryDatos_ITV(context_CoreaEntities);
					}
					return repositoryDatos_ITV;
				}
			}

		
			private IRepositoryDatos_LeasePlan repositoryDatos_LeasePlan;
			  
			public IRepositoryDatos_LeasePlan RepositoryDatos_LeasePlan
			{
				get
				{

					if (repositoryDatos_LeasePlan == null)
					{
						repositoryDatos_LeasePlan = new RepositoryDatos_LeasePlan(context_CoreaEntities);
					}
					return repositoryDatos_LeasePlan;
				}
			}

		
			private IRepositoryDatos_Multas repositoryDatos_Multas;
			  
			public IRepositoryDatos_Multas RepositoryDatos_Multas
			{
				get
				{

					if (repositoryDatos_Multas == null)
					{
						repositoryDatos_Multas = new RepositoryDatos_Multas(context_CoreaEntities);
					}
					return repositoryDatos_Multas;
				}
			}

		
			private IRepositoryDatos_Vehiculo repositoryDatos_Vehiculo;
			  
			public IRepositoryDatos_Vehiculo RepositoryDatos_Vehiculo
			{
				get
				{

					if (repositoryDatos_Vehiculo == null)
					{
						repositoryDatos_Vehiculo = new RepositoryDatos_Vehiculo(context_CoreaEntities);
					}
					return repositoryDatos_Vehiculo;
				}
			}

		
			private IRepositoryEmpresas_Leasing repositoryEmpresas_Leasing;
			  
			public IRepositoryEmpresas_Leasing RepositoryEmpresas_Leasing
			{
				get
				{

					if (repositoryEmpresas_Leasing == null)
					{
						repositoryEmpresas_Leasing = new RepositoryEmpresas_Leasing(context_CoreaEntities);
					}
					return repositoryEmpresas_Leasing;
				}
			}

		
			private IRepositoryTipo_Liquidacion repositoryTipo_Liquidacion;
			  
			public IRepositoryTipo_Liquidacion RepositoryTipo_Liquidacion
			{
				get
				{

					if (repositoryTipo_Liquidacion == null)
					{
						repositoryTipo_Liquidacion = new RepositoryTipo_Liquidacion(context_CoreaEntities);
					}
					return repositoryTipo_Liquidacion;
				}
			}

		
			private IRepositoryTipo_Seguro repositoryTipo_Seguro;
			  
			public IRepositoryTipo_Seguro RepositoryTipo_Seguro
			{
				get
				{

					if (repositoryTipo_Seguro == null)
					{
						repositoryTipo_Seguro = new RepositoryTipo_Seguro(context_CoreaEntities);
					}
					return repositoryTipo_Seguro;
				}
			}

		
			private IRepositoryTipo_Vehiculo repositoryTipo_Vehiculo;
			  
			public IRepositoryTipo_Vehiculo RepositoryTipo_Vehiculo
			{
				get
				{

					if (repositoryTipo_Vehiculo == null)
					{
						repositoryTipo_Vehiculo = new RepositoryTipo_Vehiculo(context_CoreaEntities);
					}
					return repositoryTipo_Vehiculo;
				}
			}

		 
		
		#endregion
			TKRepositorioEntities _context_TKRepositorioEntities = null;
        private  TKRepositorioEntities context_TKRepositorioEntities
        {
            get
            {
                if (_context_TKRepositorioEntities == null)
                    _context_TKRepositorioEntities = new TKRepositorioEntities();

                return _context_TKRepositorioEntities;
            }
        }

		 #region Repositories
	
			private IRepositoryMERSY_USERS_ZONAS repositoryMERSY_USERS_ZONAS;
			  
			public IRepositoryMERSY_USERS_ZONAS RepositoryMERSY_USERS_ZONAS
			{
				get
				{

					if (repositoryMERSY_USERS_ZONAS == null)
					{
						repositoryMERSY_USERS_ZONAS = new RepositoryMERSY_USERS_ZONAS(context_TKRepositorioEntities);
					}
					return repositoryMERSY_USERS_ZONAS;
				}
			}

		
			private IRepositoryMERSY_ZONAS repositoryMERSY_ZONAS;
			  
			public IRepositoryMERSY_ZONAS RepositoryMERSY_ZONAS
			{
				get
				{

					if (repositoryMERSY_ZONAS == null)
					{
						repositoryMERSY_ZONAS = new RepositoryMERSY_ZONAS(context_TKRepositorioEntities);
					}
					return repositoryMERSY_ZONAS;
				}
			}

		
			private IRepositorySAPHR_Delegaciones repositorySAPHR_Delegaciones;
			  
			public IRepositorySAPHR_Delegaciones RepositorySAPHR_Delegaciones
			{
				get
				{

					if (repositorySAPHR_Delegaciones == null)
					{
						repositorySAPHR_Delegaciones = new RepositorySAPHR_Delegaciones(context_TKRepositorioEntities);
					}
					return repositorySAPHR_Delegaciones;
				}
			}

		
			private IRepositorySAPHR_DireccionesArea repositorySAPHR_DireccionesArea;
			  
			public IRepositorySAPHR_DireccionesArea RepositorySAPHR_DireccionesArea
			{
				get
				{

					if (repositorySAPHR_DireccionesArea == null)
					{
						repositorySAPHR_DireccionesArea = new RepositorySAPHR_DireccionesArea(context_TKRepositorioEntities);
					}
					return repositorySAPHR_DireccionesArea;
				}
			}

		
			private IRepositorySAPHR_DireccionesTerritoriales repositorySAPHR_DireccionesTerritoriales;
			  
			public IRepositorySAPHR_DireccionesTerritoriales RepositorySAPHR_DireccionesTerritoriales
			{
				get
				{

					if (repositorySAPHR_DireccionesTerritoriales == null)
					{
						repositorySAPHR_DireccionesTerritoriales = new RepositorySAPHR_DireccionesTerritoriales(context_TKRepositorioEntities);
					}
					return repositorySAPHR_DireccionesTerritoriales;
				}
			}

		
			private IRepositorySAPHR_Empresas repositorySAPHR_Empresas;
			  
			public IRepositorySAPHR_Empresas RepositorySAPHR_Empresas
			{
				get
				{

					if (repositorySAPHR_Empresas == null)
					{
						repositorySAPHR_Empresas = new RepositorySAPHR_Empresas(context_TKRepositorioEntities);
					}
					return repositorySAPHR_Empresas;
				}
			}

		
			private IRepositorySAPHR_FiltroSeleccionCentrosCoste repositorySAPHR_FiltroSeleccionCentrosCoste;
			  
			public IRepositorySAPHR_FiltroSeleccionCentrosCoste RepositorySAPHR_FiltroSeleccionCentrosCoste
			{
				get
				{

					if (repositorySAPHR_FiltroSeleccionCentrosCoste == null)
					{
						repositorySAPHR_FiltroSeleccionCentrosCoste = new RepositorySAPHR_FiltroSeleccionCentrosCoste(context_TKRepositorioEntities);
					}
					return repositorySAPHR_FiltroSeleccionCentrosCoste;
				}
			}

		
			private IRepositorySAPHR_UsuariosSAP repositorySAPHR_UsuariosSAP;
			  
			public IRepositorySAPHR_UsuariosSAP RepositorySAPHR_UsuariosSAP
			{
				get
				{

					if (repositorySAPHR_UsuariosSAP == null)
					{
						repositorySAPHR_UsuariosSAP = new RepositorySAPHR_UsuariosSAP(context_TKRepositorioEntities);
					}
					return repositorySAPHR_UsuariosSAP;
				}
			}

		
			private IRepositorySAPHR_CentrosCoste repositorySAPHR_CentrosCoste;
			  
			public IRepositorySAPHR_CentrosCoste RepositorySAPHR_CentrosCoste
			{
				get
				{

					if (repositorySAPHR_CentrosCoste == null)
					{
						repositorySAPHR_CentrosCoste = new RepositorySAPHR_CentrosCoste(context_TKRepositorioEntities);
					}
					return repositorySAPHR_CentrosCoste;
				}
			}

		
			private IRepositorySAPHR_UnidadesOrganizativas repositorySAPHR_UnidadesOrganizativas;
			  
			public IRepositorySAPHR_UnidadesOrganizativas RepositorySAPHR_UnidadesOrganizativas
			{
				get
				{

					if (repositorySAPHR_UnidadesOrganizativas == null)
					{
						repositorySAPHR_UnidadesOrganizativas = new RepositorySAPHR_UnidadesOrganizativas(context_TKRepositorioEntities);
					}
					return repositorySAPHR_UnidadesOrganizativas;
				}
			}

		
			private IRepositoryGENERAL_PARAMETROS_APLICACIONES repositoryGENERAL_PARAMETROS_APLICACIONES;
			  
			public IRepositoryGENERAL_PARAMETROS_APLICACIONES RepositoryGENERAL_PARAMETROS_APLICACIONES
			{
				get
				{

					if (repositoryGENERAL_PARAMETROS_APLICACIONES == null)
					{
						repositoryGENERAL_PARAMETROS_APLICACIONES = new RepositoryGENERAL_PARAMETROS_APLICACIONES(context_TKRepositorioEntities);
					}
					return repositoryGENERAL_PARAMETROS_APLICACIONES;
				}
			}

		
			private IRepositoryGENERAL_IDIOMAS repositoryGENERAL_IDIOMAS;
			  
			public IRepositoryGENERAL_IDIOMAS RepositoryGENERAL_IDIOMAS
			{
				get
				{

					if (repositoryGENERAL_IDIOMAS == null)
					{
						repositoryGENERAL_IDIOMAS = new RepositoryGENERAL_IDIOMAS(context_TKRepositorioEntities);
					}
					return repositoryGENERAL_IDIOMAS;
				}
			}

		 
		
		#endregion
		

        public void Commit()
        {
            context_ModelEntities.SaveChanges();
        }

        public bool LazyLoadingEnabled
        {
            get { return context_ModelEntities.Configuration.LazyLoadingEnabled; }
            set { context_ModelEntities.Configuration.LazyLoadingEnabled = value; }
        }

        public bool ProxyCreationEnabled
        {
            get { return context_ModelEntities.Configuration.ProxyCreationEnabled; }
            set { context_ModelEntities.Configuration.ProxyCreationEnabled = value; }
        }

        public string ConnectionString
        {
            get { return context_ModelEntities.Database.Connection.ConnectionString; }
            set { context_ModelEntities.Database.Connection.ConnectionString = value; }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context_ModelEntities.Dispose();
					
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
 
  
