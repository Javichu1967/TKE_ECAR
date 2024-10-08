using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using TK_ECAR.ApplicationServices;
using TK_ECAR.Content.resources;
using TK_ECAR.Framework;
using TK_ECAR.Framework.Models;
using TK_ECAR.ImportacionFlota.ApplicationServices;
using TK_ECAR.Models;
using TK_ECAR.PortugalImportacion.ApplicationServices;

namespace TK_ECAR.Utils
{
    public class GlobalProcesosSignalR
    {
        private IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
        private ResumenImportacionModels resumenImportacion = new ResumenImportacionModels();
        private string msgImportacion = string.Empty;

        #region Flota
        public bool ImportarFlota(ImportacionFlotaModels modelo, ResumenImportacionModels resumenImportacionParam, IHubContext hubContextParam, string user)
        {
            var result = true;
            int fileProgress = 0;

            hubContext = hubContextParam;

            resumenImportacion = resumenImportacionParam;
            resumenImportacion.ListadoResumen = new List<Incidencia>();

            // Initialize Hub context
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
            hubContext.Clients.All.sendMessage(TK_ECAR_Resource.ResourceManager.GetString("msgIniciandoImportacion"), fileProgress);

            ImportarFlotaService ImportarFlota = new ImportarFlotaService();

            //Establecer eventos del proceso de importación.
            ImportarFlota.EventProcesingImportFlota += this.OnGetEventProcesingImport;
            ImportarFlota.EventIncidenciaImportFlota += this.OnEventIncidenciaImport;
            ImportarFlota.EventErrorImportFlota += this.OnGetEventErrorImport;
            ImportarFlota.EventFinishedImportFlota += this.OnGetEventFinishedImport;

            try
            {
                FileUtilities.UploadFile(modelo.FileToImport, Global.PathToPathToImportFilesFlota);

                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportación_FechaImportacion")} {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}"
                });
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = TK_ECAR_Resource.ResourceManager.GetString("msgImportación_Cabecera").Replace("@@", FileUtilities.GetFileName(modelo.FileToImport))
                });

                ImportarFlota.startImport(modelo, user);
            }

            catch (Exception ex)
            {
                result = false;
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Error,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ErrorGeneral")}. {Environment.NewLine}{Global.GetMessageError(ex)}"
                });
                hubContext.Clients.All.sendMessageFinished(TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ProcesoFinalizado"), 0);
            }

            return result;
        }
        #endregion Flota

        #region ViaVerde
        public bool ImportarViaVerde(ImportacionDatosModels modelo, ResumenImportacionModels resumenImportacionParam, IHubContext hubContextParam)
        {
            var result = true;
            int fileProgress = 0;

            hubContext = hubContextParam;

            resumenImportacion = resumenImportacionParam;
            resumenImportacion.ListadoResumen = new List<Incidencia>();

            // Initialize Hub context
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
            hubContext.Clients.All.sendMessage(TK_ECAR_Resource.ResourceManager.GetString("msgIniciandoImportacion"), fileProgress);

            ImportacionPortugalService ImportarViaVerde = new ImportacionPortugalService();

            //Establecer eventos del proceso de importación de vehículos.
            ImportarViaVerde.EventProcessingImportDatosPortugal += this.OnGetEventProcesingImport;
            ImportarViaVerde.EventSubProcessingImportDatosPortugal += this.OnGetEventSubProcessingImport;
            ImportarViaVerde.EventIncidenciaImportDatosPortugal += this.OnEventIncidenciaImport;
            ImportarViaVerde.EventErrorImportDatosPortugal += this.OnGetEventErrorImport;
            ImportarViaVerde.EventFinishedImportDatosPortugal += this.OnGetEventFinishedImport;

            try
            {
                FileUtilities.UploadFile(modelo.FileToImport, Global.PathToPathToImportFilesViaVerde);

                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportación_FechaImportacion")} {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}"
                });
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = TK_ECAR_Resource.ResourceManager.GetString("msgImportación_Cabecera").Replace("@@", FileUtilities.GetFileName(modelo.FileToImport))
                });

                ImportarViaVerde.ProcessViaVerde(modelo);
            }

            catch (Exception ex)
            {
                result = false;
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Error,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ErrorGeneral")}. {Environment.NewLine}{Global.GetMessageError(ex)}"
                });
                hubContext.Clients.All.sendMessageFinished(TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ProcesoFinalizado"), 0);
            }

            return result;
        }
        #endregion ViaVerde

        #region TarjetasCombustible
        public bool ImportarConsumosCombustibleCombustible(ImportacionDatosModels modelo, ResumenImportacionModels resumenImportacionParam, IHubContext hubContextParam)
        {
            var result = true;
            int fileProgress = 0;

            hubContext = hubContextParam;

            resumenImportacion = resumenImportacionParam;
            resumenImportacion.ListadoResumen = new List<Incidencia>();

            // Initialize Hub context
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
            hubContext.Clients.All.sendMessage(TK_ECAR_Resource.ResourceManager.GetString("msgIniciandoImportacion"), fileProgress);

            ImportacionConsumoCombustibleService importarConsumo = new ImportacionConsumoCombustibleService();

            //Establecer eventos del proceso de importación de vehículos.
            importarConsumo.EventProcessingImportConsumoCombustible += this.OnGetEventProcesingImport;
            importarConsumo.EventSubProcessingImportConsumoCombustible += this.OnGetEventSubProcessingImport;
            importarConsumo.EventIncidenciaImportConsumoCombustible += this.OnEventIncidenciaImport;
            importarConsumo.EventErrorImportConsumoCombustible += this.OnGetEventErrorImport;
            importarConsumo.EventFinishedImportConsumoCombustible += this.OnGetEventFinishedImport;

            try
            {
                FileUtilities.UploadFile(modelo.FileToImport, Global.PathToPathToImportFilesConsumoCombustible);

                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportación_FechaImportacion")} {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}"
                });
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = TK_ECAR_Resource.ResourceManager.GetString("msgImportación_Cabecera").Replace("@@", FileUtilities.GetFileName(modelo.FileToImport))
                });

                importarConsumo.ImportaConsumoCombustible(modelo);
            }

            catch (Exception ex)
            {
                result = false;
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Error,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ErrorGeneral")}. {Environment.NewLine}{Global.GetMessageError(ex)}"
                });
                hubContext.Clients.All.sendMessageFinished(TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ProcesoFinalizado"), 0);
            }

            return result;
        }
        #endregion TarjetasCombustible


        #region LEASING
        public bool ImportarLEASING(ImportacionLeasingModels modelo, ResumenImportacionModels resumenImportacionParam, IHubContext hubContextParam)
        {
            var result = true;
            int fileProgress = 0;

            hubContext = hubContextParam;

            resumenImportacion = resumenImportacionParam;
            resumenImportacion.ListadoResumen = new List<Incidencia>();

            // Initialize Hub context
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
            hubContext.Clients.All.sendMessage(TK_ECAR_Resource.ResourceManager.GetString("msgIniciandoImportacion"), fileProgress);

            ImportacionLeasingService importarLEASING = new ImportacionLeasingService();

            //Establecer eventos del proceso de importación de vehículos.
            importarLEASING.EventProcessingImportDatosLeasing += this.OnGetEventProcesingImport;
            importarLEASING.EventSubProcessingImportDatosLeasing += this.OnGetEventSubProcessingImport;
            importarLEASING.EventIncidenciaImportDatosLeasing += this.OnEventIncidenciaImport;
            importarLEASING.EventErrorImportDatosLeasing += this.OnGetEventErrorImport;
            importarLEASING.EventFinishedImportDatosLeasing += this.OnGetEventFinishedImport;

            try
            {
                FileUtilities.UploadFile(modelo.FileToImport, Global.PathToPathToImportFilesRenting);

                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportación_FechaImportacion")} {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}"
                });
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = TK_ECAR_Resource.ResourceManager.GetString("msgImportación_Cabecera").Replace("@@", FileUtilities.GetFileName(modelo.FileToImport))
                });

                importarLEASING.ImportaDatosLEASING(modelo);
            }

            catch (Exception ex)
            {
                result = false;
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Error,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ErrorGeneral")}. {Environment.NewLine}{Global.GetMessageError(ex)}"
                });
                hubContext.Clients.All.sendMessageFinished(TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ProcesoFinalizado"), 0);
            }

            return result;
        }
        #endregion LEASING

        #region FACTURACIÓN
        public bool GenerarFacturacion(GeneracionFacturacionModels modelo, ResumenImportacionModels resumenImportacionParam, IHubContext hubContextParam, 
                                        List<FacturaModels> FacturasLeasing, List<FacturaRepartoModels> RepartoFacturasLeasing)
        {
            var result = true;
            int fileProgress = 0;

            hubContext = hubContextParam;

            resumenImportacion = resumenImportacionParam;
            resumenImportacion.ListadoResumen = new List<Incidencia>();

            // Initialize Hub context
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ECAR_ProgressHub>();
            hubContext.Clients.All.sendMessage(TK_ECAR_Resource.ResourceManager.GetString("msgIniciandoGeneracionFacturas"), fileProgress);

            FacturacionLeasingService generarFacturas = new FacturacionLeasingService();

            //Establecer eventos del proceso de importación de vehículos.
            generarFacturas.EventProcessingFacturaDatosLeasing += this.OnGetEventProcesingImport;
            generarFacturas.EventSubProcessingFacturaDatosLeasing += this.OnGetEventSubProcessingImport;
            generarFacturas.EventIncidenciaFacturaDatosLeasing += this.OnEventIncidenciaImport;
            generarFacturas.EventErrorFacturaDatosLeasing += this.OnGetEventErrorImport;
            generarFacturas.EventFinishedFacturaDatosLeasing += this.OnGetEventFinishedImport;

            try
            {
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Normal,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportación_FechaProcesamientoFacturas")} {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}"
                });

                generarFacturas.FacturacionDatosLEASING(modelo, FacturasLeasing, RepartoFacturasLeasing);
            }

            catch (Exception ex)
            {
                result = false;
                resumenImportacion.ListadoResumen.Add(new Incidencia
                {
                    TipoLinea = EnumTipoLineaImportacion.Error,
                    Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ErrorGeneral")}. {Environment.NewLine}{Global.GetMessageError(ex)}"
                });
                hubContext.Clients.All.sendMessageFinished(TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ProcesoFinalizado"), 0);
            }

            return result;
        }
        #endregion FACTURACIÓN


        #region Eventos importación
        private void OnGetEventProcesingImport(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            var PercentLineProgress = 0;
            msgImportacion = string.Empty;

            EstableceValoresProcesingImport(lineProsessing, TotalLinesToProcess, msg, ref PercentLineProgress);

            hubContext.Clients.All.sendMessage(msgImportacion, PercentLineProgress);
        }
        private void OnGetEventSubProcessingImport(int lineProsessing, int TotalLinesToProcess, string msg)
        {
            var PercentLineProgress = 0;
            msgImportacion = string.Empty;

            EstableceValoresProcesingImport(lineProsessing, TotalLinesToProcess, msg, ref PercentLineProgress);

            hubContext.Clients.All.sendMessageSubProcess(msgImportacion, PercentLineProgress);
        }
        private void EstableceValoresProcesingImport(int lineProsessing, int TotalLinesToProcess, string msg, ref int PercentLineProgress)
        {
            if (lineProsessing > 0 && TotalLinesToProcess > 0)
            {
                PercentLineProgress = Convert.ToInt32((100 * lineProsessing) / TotalLinesToProcess);
            }
            if (PercentLineProgress > 100)
            {
                PercentLineProgress = 100;
            }

            if (lineProsessing == 0 && TotalLinesToProcess == 0)
            {
                msgImportacion = msg;
            }
            else if (TotalLinesToProcess == 0)
            {
                msgImportacion = string.Format
                                (msg + " {0}", lineProsessing.ToString("###,###,##0"));
            }
            else
            {
                msgImportacion = string.Format
                                (msg + " {0} de {1}", lineProsessing.ToString("###,###,##0"), TotalLinesToProcess.ToString("###,###,##0"));
            }
        }

        private void OnGetEventErrorImport(int lineProsessing, string msgError)
        {
            resumenImportacion.ListadoResumen.Add(new Incidencia
            {
                TipoLinea = EnumTipoLineaImportacion.Error,
                Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("TitleLinea")} - {lineProsessing} {msgError}"
            });
            //hubContext.Clients.All.sendMessage(msgError, lineProsessing);
        }

        private void OnEventIncidenciaImport(int lineProsessing, string matricula, EnumTipoLineaImportacion tipoIncidencia)
        {
            string msg = string.Empty;

            var textoMatricula = string.Empty;

            if (!string.IsNullOrEmpty(matricula))
            {
                textoMatricula = $"{TK_ECAR_Resource.ResourceManager.GetString("lblMatricula")} [{matricula}] ";
            }

            switch (tipoIncidencia)
            {
                case EnumTipoLineaImportacion.MatriculaVacia:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{ TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaMatriculaVacia")}";
                    break;
                case EnumTipoLineaImportacion.NoHayConductor:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{ TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaSinConductor")}";
                    break;
                case EnumTipoLineaImportacion.ConductorNuevo:
                    msg = $"{textoMatricula} {TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaConductorNuevo")}";
                    break;
                case EnumTipoLineaImportacion.SinFechaMatriculacion:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{ TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaSinFechaMatriculacion")}";
                    break;
                case EnumTipoLineaImportacion.SinTipoDeVehiculo:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{ TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaSinTipoDeVehiculo")}";
                    break;
                case EnumTipoLineaImportacion.VehiculoExistente:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaVehiculoExistente")}";
                    break;
                case EnumTipoLineaImportacion.MatriculaInexistente:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{TK_ECAR_Resource.ResourceManager.GetString("ImportacionMatriculaInexistente")}";
                    break;
                case EnumTipoLineaImportacion.ConductorNoEncontrado:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaConductorNoEncontrado")}";
                    break;
                case EnumTipoLineaImportacion.ConceptoLeasinNoContemplado:
                    msg = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Incidencia")} {textoMatricula}";
                    msg = msg + $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_IncidenciaConceptoLeasinNoContemplado")}";
                    break;
            }

            resumenImportacion.ListadoResumen.Add(new Incidencia
            {
                TipoLinea = tipoIncidencia,
                Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("TitleLinea")} - {lineProsessing} {msg}"
            });
            //hubContext.Clients.All.sendMessage(msg, lineProsessing);
        }

        private void OnGetEventFinishedImport(int TotalLinesToProcess, int TotalLinesProcessedOK, int TotalLinesProcessedIncidencia, int TotalLinesProcessedERROR)
        {
            resumenImportacion.ListadoResumen.Add(new Incidencia
            {
                TipoLinea = EnumTipoLineaImportacion.Normal,
                Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportación_FinalizaImportacion")} {DateTime.Now.ToString("dd/MM/yyyy hh:mm")}"
            });

            resumenImportacion.ListadoResumen.Add(new Incidencia
            {
                TipoLinea = EnumTipoLineaImportacion.Resumen,
                Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_Total")} {TotalLinesToProcess.ToString("###,##0")}"
            });
            resumenImportacion.ListadoResumen.Add(new Incidencia
            {
                TipoLinea = EnumTipoLineaImportacion.Resumen,
                Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_TotalOK")} {TotalLinesProcessedOK.ToString("###,##0")}"
            });
            resumenImportacion.ListadoResumen.Add(new Incidencia
            {
                TipoLinea = EnumTipoLineaImportacion.Resumen,
                Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_TotalIncidencia")} {TotalLinesProcessedIncidencia.ToString("###,##0")}"
            });
            resumenImportacion.ListadoResumen.Add(new Incidencia
            {
                TipoLinea = EnumTipoLineaImportacion.Resumen,
                Texto = $"{TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_TotalERROR")} {TotalLinesProcessedERROR.ToString("###,##0")}"
            });

            hubContext.Clients.All.sendMessageFinished(TK_ECAR_Resource.ResourceManager.GetString("msgImportacion_ProcesoFinalizado"), 0);
        }
        #endregion
    }
}