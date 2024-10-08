using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TK_ECAR.Content.resources;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using TKUtilidades;

namespace TK_ECAR.Application_Services
{
    public class InformeFlotaService
    {
        private int rowUserDate = 1;
        private int rowTitle = 2;
        private int rowTitleSec = 3;
        private int rowFilter = 4;
        private int rowHeader = 0;
        private int numColumns = 11;

        private int col_empresa = 1;
        private int col_Ceco = 2;
        private int col_Matricula = 3;
        private int col_Marca = 4;
        private int col_Modelo = 5;
        private int col_CiaLeasing = 6;
        private int col_FechaVtoLeasing = 7;
        private int col_CiaSeguro = 8;
        private int col_Poliza_Seguro = 9;
        private int col_ImpSeguro = 10;
        private int col_FechaVtoSeguro = 11;




        #region obtención datos vehículo
        public InformeContratosRentigPorFechaFinalizacionModels InformeFlotaPeriodoAltaBajaFechaContrato(List<string> cecosSel, FilterInformeFlotaModel modelo)
        {
            var Paso = "";
            var datosVehiculo = new InformeContratosRentigPorFechaFinalizacionModels();
            var filtro = new FilterInformeFlotaModel();
            datosVehiculo.vehiculosLeasing = new List<InfFlotaFechaContratoModels>();

            var cecos = cecosSel;

            var empLeasing = GetListIntFromSplit(modelo.EmpresasLeasingSeleccionadas, ',');
            var marcas = GetListIntFromSplit(modelo.MarcasSeleccionadas, ',');
            var modelos = GetListIntFromSplit(modelo.ModelosSeleccionados, ',');

            if (modelo.FechaHasta == null)
            {
                modelo.FechaHasta = DateTime.Now.AddYears(10);
            }
            var firstDayOfMonth = new DateTime(modelo.FechaDesde.Value.Year, modelo.FechaDesde.Value.Month, 1);
            var lastDayOfMonth = modelo.FechaHasta.Value.AddMonths(1).AddDays(-1);

            var cecosModeList = ((UserModel)Util.GetItemFromMemory("userProfile")).CecosModelList;

            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    Paso = "Obteniendo primeros datos JOIN";
                    datosVehiculo.vehiculosLeasing = (from vehiculo in unitOfWork.RepositoryECAR_Datos_Vehiculo.Fetch().Where(x => x.Baja == false)
                                                      join
                                                      leasing in unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Fetch() on vehiculo.EmpresaLeasing equals leasing.ID_EMPRESA
                                                      join
                                                      seguros in unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Fetch() on vehiculo.Cia_Seguro equals seguros.ID_EMPRESA into compañias
                                                      from compañiasSeguro in compañias.DefaultIfEmpty()
                                                      where
                                                             ((cecos.Contains(vehiculo.CC)) || (!cecos.Any()))
                                                             &&
                                                             (vehiculo.Fecha_Vto_Contrato >= modelo.FechaDesde && vehiculo.Fecha_Vto_Contrato <= modelo.FechaHasta)
                                                             &&
                                                             (marcas.Contains(vehiculo.T_M_MARCA_VEHICULOS.ID_MARCA) || !marcas.Any())
                                                             &&
                                                             (modelos.Contains(vehiculo.T_M_MODELOS_VEHICULO.ID_MODELO) || !modelos.Any())
                                                             &&
                                                             ((empLeasing.Contains(vehiculo.EmpresaLeasing == null ? 0 : (int)vehiculo.EmpresaLeasing)) || (!empLeasing.Any()))
                                                      select new InfFlotaFechaContratoModels
                                                      {
                                                          Matricula = vehiculo.Matricula,
                                                          MarcaVehiculo = vehiculo.T_M_MARCA_VEHICULOS.DESCRIPCION,
                                                          ModeloVehiculo = vehiculo.T_M_MODELOS_VEHICULO.DESCRIPCION,
                                                          TipoVehiculo = (vehiculo.T_M_TIPOS_VEHICULO.ID_TIPO_VEHICULO == null ? 0 : vehiculo.T_M_TIPOS_VEHICULO.ID_TIPO_VEHICULO),
                                                          Bastidor = vehiculo.Num_Bastidor,
                                                          CiaLeasing = leasing.NOMBRE,
                                                          CiaSeguro = compañiasSeguro.NOMBRE,
                                                          Poliza_Seguro = vehiculo.Poliza_Seguro,
                                                          ImpSeguro = vehiculo.Importe_Seguro,
                                                          FechaVtoSeguro = vehiculo.Vto_Seguro,
                                                          CodCeco = vehiculo.CC,
                                                          NumContratoLeasing = vehiculo.Num_Contrato,
                                                          FechaVtoLeasing = (DateTime)vehiculo.Fecha_Vto_Contrato,
                                                          CodEmpresa = vehiculo.Sociedad,
                                                      }).OrderBy(x => x.CodEmpresa).OrderBy(x => x.CodCeco).OrderBy(x => x.FechaVtoLeasing).ToList();

                    Paso = "Actualizando descripciones ForEach";
                    datosVehiculo.vehiculosLeasing.ForEach
                        (x =>
                        {
                            x.Empresa = cecosModeList.FirstOrDefault(c => c.IdCeco == x.CodCeco).Empresa;
                            x.Delegacion = cecosModeList.FirstOrDefault(c => c.IdCeco == x.CodCeco).Delegacion;
                            x.DirTerritorial = cecosModeList.FirstOrDefault(c => c.IdCeco == x.CodCeco).DireccionTerritorial;
                            x.NombreCeco = cecosModeList.FirstOrDefault(c => c.IdCeco == x.CodCeco).Ceco;
                        });

                    datosVehiculo.FiltroUtilizado = EstableceDatosFiltroParaInforme(unitOfWork, modelo);
                }
            }
            catch(Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, $"<InformeFlotaPeriodoAltaBajaFechaContrato> {Paso}. {Environment.NewLine} {Global.GetMessageError(ex)}");
                throw ex;
            }

            return datosVehiculo;
        }
        #endregion obtención datos vehículo

        #region generación archivo excel
        public MemoryStream ExportReportContratosRentingToExcel(InformeContratosRentigPorFechaFinalizacionModels vehiculosReport)
        {
            MemoryStream ms = new MemoryStream();
            string rptTittle = string.Empty;

            try
            {
                using (ExcelPackage pck = new ExcelPackage(ms))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Vehículos");

                    rowHeader = SetFilter(ws, vehiculosReport.FiltroUtilizado, ((UserModel)Util.GetItemFromMemory("userProfile")).Nombre, 
                                        TK_ECAR_Resource.ResourceManager.GetString("TitleInformeFlotaFechaContratoInf"));


                    rowHeader ++;

                    var rowDetail = rowHeader++;
                    var asterisco = "";
                    var fechaAnt = "";
                    var totalParcial = 0;
                    var totalVehiculos = 0;
                    var rowInicioRecuadro = 0;
                    var rowFinRecuadro = 0;
                    foreach (InfFlotaFechaContratoModels vehiculo in vehiculosReport.vehiculosLeasing)
                    {
                        totalVehiculos++;
                        if (fechaAnt != vehiculo.MesAñoVtoLeasingText)
                        {
                            if (fechaAnt != "")
                            {
                                rowFinRecuadro = rowHeader;

                                SetRecuadro(ws, rowInicioRecuadro, rowFinRecuadro);

                                rowHeader++;
                                SetTotalVehiculos(ws, rowHeader, $"Total vehículos en {fechaAnt}   - {totalParcial.ToString("###,###,##0")} -");
                                rowHeader+=2;
                            }
                            rowInicioRecuadro = rowHeader;
                            SetHeader(ws, rowHeader);
                            fechaAnt = vehiculo.MesAñoVtoLeasingText;
                            totalParcial = 0;
                        }
                        totalParcial++;
                        rowHeader++;

                        asterisco = (vehiculo.TipoVehiculo == Global.ID_TURISMO_DIRECCION() ? "(*)" : "");

                        ws.Cells[rowHeader, col_empresa].Value = vehiculo.CodEmpresa;
                        ws.Cells[rowHeader, col_empresa].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[rowHeader, col_Ceco].Value = vehiculo.codCecoMasNombre;
                        ws.Cells[rowHeader, col_Matricula].Value = $"{vehiculo.Matricula} {asterisco}";
                        ws.Cells[rowHeader, col_Marca].Value = vehiculo.MarcaVehiculo;
                        ws.Cells[rowHeader, col_Modelo].Value = vehiculo.ModeloVehiculo;
                        ws.Cells[rowHeader, col_CiaLeasing].Value = vehiculo.CiaLeasing;
                        ws.Cells[rowHeader, col_FechaVtoLeasing].Value = vehiculo.FechaVtoLeasing.ToString("dd/MM/yyyy");
                        ws.Cells[rowHeader, col_FechaVtoLeasing].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[rowHeader, col_CiaSeguro].Value = vehiculo.CiaSeguro;
                        ws.Cells[rowHeader, col_Poliza_Seguro].Value = vehiculo.Poliza_Seguro;
                        ws.Cells[rowHeader, col_ImpSeguro].Value = vehiculo.ImpSeguro_FORMATEADO;
                        ws.Cells[rowHeader, col_ImpSeguro].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        ws.Cells[rowHeader, col_FechaVtoSeguro].Value = vehiculo.FechaVtoSeguro == null ? "" : vehiculo.FechaVtoSeguro.Value.ToString("dd/MM/yyyy");
                        ws.Cells[rowHeader, col_FechaVtoSeguro].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        using (var range = ws.Cells[rowHeader, 1, rowHeader, numColumns])
                        {
                            range.Style.Font.Size = 9;
                            range.Style.Font.Name = "TKTypeRegular";
                            if (asterisco != "")
                            {
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 200, 50));
                            }
                        }
                    }
                    if (vehiculosReport.vehiculosLeasing.Count > 0)
                    {
                        rowHeader++;
                        SetTotalVehiculos(ws, rowHeader, $"Total vehículos en {fechaAnt}   - {totalParcial.ToString("###,###,##0")} -");
                        rowHeader+=2;
                        SetTotalVehiculos(ws, rowHeader++, $"Total vehículos   - {totalVehiculos.ToString("###,###,##0")} -");
                    }

                    AutoFitColumns(ws);

                    pck.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ms;
        }

        #region Filtro
        private int SetFilter(ExcelWorksheet ws, FilterInformeFlotaModel xlsFilter, string user, string Title, string secTitle = "")
        {
            string lDate = DateTime.Now.ToUniversalTime().ToString("dd/MM/yyyy");
            string _filterName = string.Empty;
            int rowF = rowFilter;

            ws.Cells[rowUserDate, 1].Value = user;
            ws.Cells[rowUserDate, numColumns].Value = lDate;
            ws.Cells[rowUserDate, numColumns].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            using (var range = ws.Cells[rowUserDate, 1, rowTitle, numColumns])
            {
                range.Style.Font.Size = 9;
                range.Style.Font.Name = "TKTypeRegular";
            }

            using (var range = ws.Cells[rowTitle, 1, rowTitle, numColumns])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 14;
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.Font.Name = "TKTypeRegular";
                range.Value = Title;
            }

            if (secTitle != "")
            {
                using (var range = ws.Cells[rowTitleSec, 1, rowTitleSec, numColumns])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 14;
                    range.Merge = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.Font.Name = "TKTypeRegular";
                    range.Value = secTitle;
                }

            }

            var inicioFiltro = 0;
            if (xlsFilter != null)
            {
                rowF++;
                _filterName = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroUtilizado");
                ws.Cells[rowF, 1].Value = _filterName;
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";

                rowF++;
                inicioFiltro = rowF;
                ws.Cells[rowF, 1].Value = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroEmpresas");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.EmpresasSeleccionadas;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroDelegaciones");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.DelegacionesSeleccionadas;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroDTs");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.DireccionesTerritorialesSeleccionadas;
                ws.Cells[rowF, 3].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroCecos");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.CentrosCosteSeleccionados;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroEmpLeasing");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.EmpresasLeasingSeleccionadas;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroMarcas");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.MarcasSeleccionadas;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = TK_ECAR_Resource.ResourceManager.GetString("TitleInfFiltroModelos");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.ModelosSeleccionados;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = ModelsResources.ResourceManager.GetString("lblFechaContDesde");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.MesAñoFechaDesdeText;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                rowF++;
                ws.Cells[rowF, 1].Value = ModelsResources.ResourceManager.GetString("lblFechaContHasta");
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 2].Value = xlsFilter.MesAñoFechaHastaText;
                ws.Cells[rowF, 2].Style.Font.Name = "TKTypeRegular";

                using (var range = ws.Cells[inicioFiltro, 1, rowF, 2])
                {
                    //range.Style.Font.Bold = true;
                    range.Style.Font.Size = 10;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(115, 195, 240));
                    //range.Merge = true;
                    range.Style.Font.Name = "TKTypeRegular";
                    //range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }
                rowF+=2;

                ws.Cells[rowF, 1].Value = "(*) Vehículo Directivo";
                ws.Cells[rowF, 1].Style.Font.Bold = true;
                ws.Cells[rowF, 1].Style.Font.Size = 7;
                ws.Cells[rowF, 1].Style.Font.Name = "TKTypeRegular";
                ws.Cells[rowF, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowF, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 200, 50));

                rowF++;
            }

            return rowF;
        }
        #endregion Filtro

        #region Cabecera campos
        private void SetHeader(ExcelWorksheet ws, int rowHeader)
        {
            int c = 1;

            var _columnName = ModelsResources.ResourceManager.GetString("lblEmpresa");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblCECO");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblMatricula");
            ws.Cells[rowHeader, c].Value = $"{_columnName} (*)" ;
            c++;
           _columnName = ModelsResources.ResourceManager.GetString("lblMarca");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblModelo");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblEmpresaLeasing");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblFechaFinalizacion");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblCompañiaSeguros");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblPolizaSeguro");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblImpSeguro");
            ws.Cells[rowHeader, c].Value = _columnName;
            c++;
            _columnName = ModelsResources.ResourceManager.GetString("lblFechaVtoSeguro");
            ws.Cells[rowHeader, c].Value = _columnName;


            using (var range = ws.Cells[rowHeader, 1, rowHeader, c])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 10;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 60, 125));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                range.Style.Font.Name = "TKTypeRegular";
                range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            }
        }
        #endregion Cabecera campos

        #region Total
        private void SetTotalVehiculos(ExcelWorksheet ws, int rowHeader, string texto)
        {
            using (var range = ws.Cells[rowHeader, 1, rowHeader, numColumns])
            {
                range.Style.Font.Bold = true;
                range.Merge = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                range.Style.Font.Name = "TKTypeRegular";
                range.Value = texto;
            }

        }
        #endregion Total

        #region Recuadro
        private void SetRecuadro(ExcelWorksheet ws, int rowInicio, int rowFin)
        {
            using (var range = ws.Cells[rowInicio, 1, rowFin, numColumns])
            {
                range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Double);
            }
        }
        #endregion Recuadro
        #region Autoajustado de las columnas

        /// <summary>
        /// Ajusta el tamaño de las columnas
        /// </summary>
        private void AutoFitColumns(ExcelWorksheet ws)
        {
            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            //for (int c = 1; c <= numColumns; c++)
            //{
            //    ws.Column(c).AutoFit();
            //}
        }
        #endregion Autoajustado de las columnas

        #endregion generación archivo excel

        #region Métodos Privados
        private List<int> GetListIntFromSplit(string valores, char separador)
        {
            List<int> returnList = new List<int>();
            if (!string.IsNullOrEmpty(valores))
            {
                returnList = valores.Split(separador).Select(x => Convert.ToInt32(x)).ToList();
            }

            return returnList;
        }

        private List<string> GetListStringFromSplit(string valores, char separador)
        {
            List<string> returnList = new List<string>();
            if (!string.IsNullOrEmpty(valores))
            {
                returnList = valores.Split(separador).ToList();
            }

            return returnList;
        }

        private FilterInformeFlotaModel EstableceDatosFiltroParaInforme(UnitOfWork unitOfWork, FilterInformeFlotaModel modelo)
        {
            FilterInformeFlotaModel filtro = new FilterInformeFlotaModel();

            if (!string.IsNullOrEmpty(modelo.EmpresasSeleccionadas))
            {
                filtro.EmpresasSeleccionadas = modelo.EmpresasSeleccionadas.Replace(",", ";");
            }

            if (!string.IsNullOrEmpty(modelo.DelegacionesSeleccionadas))
            {
                filtro.DelegacionesSeleccionadas = modelo.DelegacionesSeleccionadas.Replace(",", ";");
            }

            if (!string.IsNullOrEmpty(modelo.DireccionesTerritorialesSeleccionadas))
            {
                filtro.DireccionesTerritorialesSeleccionadas = modelo.DireccionesTerritorialesSeleccionadas.Replace(",", ";");
            }

            if (!string.IsNullOrEmpty(modelo.CentrosCosteSeleccionados))
            {
                filtro.CentrosCosteSeleccionados = modelo.CentrosCosteSeleccionados.Replace(",", ";");
            }

            if (!string.IsNullOrEmpty(modelo.AgrupacionesFiltroSeleccionadas))
            {
                filtro.CentrosCosteSeleccionados = modelo.AgrupacionesFiltroSeleccionadas.Replace(",", ";");
            }

            var NomEmpLeasing = string.Empty;
            if (!string.IsNullOrEmpty(modelo.EmpresasLeasingSeleccionadas))
            {
                var empLeasing = GetListIntFromSplit(modelo.EmpresasLeasingSeleccionadas, ',');
                var specLeasing = new T_M_EMPRESAS_VEHICULOSSpecification
                {
                    ID_EMPRESAIN = empLeasing.Select(x=>(int?)x).ToList()
                };
                foreach (string item in unitOfWork.RepositoryT_M_EMPRESAS_VEHICULOS.Where(specLeasing).Select(x => x.NOMBRE))
                {
                    NomEmpLeasing = NomEmpLeasing + (NomEmpLeasing == "" ? "" : "; ") + item;
                }
            }
            filtro.EmpresasLeasingSeleccionadas = NomEmpLeasing;

            var NomMarcas = string.Empty;
            if (!string.IsNullOrEmpty(modelo.MarcasSeleccionadas))
            {
                var marcas = GetListIntFromSplit(modelo.MarcasSeleccionadas, ',');
                var specMarcas = new T_M_MARCA_VEHICULOSSpecification
                {
                    ID_MARCAIN = marcas.Select(x => (int?)x).ToList()
                };
                foreach (string item in unitOfWork.RepositoryT_M_MARCA_VEHICULOS.Where(specMarcas).Select(x => x.DESCRIPCION))
                {
                    NomMarcas = NomMarcas + (NomMarcas == "" ? "" : "; ") + item;
                }
            }
            filtro.MarcasSeleccionadas = NomMarcas;

            var nomModelos = string.Empty;
            if (!string.IsNullOrEmpty(modelo.ModelosSeleccionados))
            {
                var modelos = GetListIntFromSplit(modelo.ModelosSeleccionados, ',');
                var specModelos = new T_M_MODELOS_VEHICULOSpecification
                {
                    ID_MODELOIN = modelos.Select(x => (int?)x).ToList()
                };
                foreach (string item in unitOfWork.RepositoryT_M_MODELOS_VEHICULO.Where(specModelos).Select(x => x.DESCRIPCION))
                {
                    nomModelos = nomModelos + (nomModelos == "" ? "" : "; ") + item;
                }
            }
            filtro.ModelosSeleccionados = nomModelos;

            filtro.FechaDesde = modelo.FechaDesde;
            filtro.FechaHasta = modelo.FechaHasta;

            return filtro;
        }
        #endregion Métodos Privados

    }
}