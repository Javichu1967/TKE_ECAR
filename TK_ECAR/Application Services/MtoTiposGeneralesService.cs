using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;

namespace TK_ECAR.Application_Services
{
    public class MtoTiposGeneralesService
    {
        #region Get tipos
        /// <summary>
        /// Devuelve los datos de la tabla de tipos, según el mantenimiento que se pase
        /// </summary>
        /// <param name="tipoMantenimiento"></param>
        /// <returns>List<MtoDatosGeneralesModel></returns>
        public List<MtoGenericoTiposModels> GetMtoGeneralTiposDatatable(int tipoMantenimiento, bool filtrarBaja = true, int ID_MtoGeneral = 0, int foreignKey = 0, string term = "")
        {
            List<MtoGenericoTiposModels> listaTipos = new List<MtoGenericoTiposModels>();

            switch (tipoMantenimiento)
            {
                case (int)EnumMtoTiposGenerales.Carburante:
                    listaTipos = GetCarburantesDataTable(filtrarBaja, ID_MtoGeneral, term);
                    break;
                case (int)EnumMtoTiposGenerales.Marcas:
                    listaTipos = GetMarcasDataTable(filtrarBaja, ID_MtoGeneral, term);
                    break;
                case (int)EnumMtoTiposGenerales.Modelos:
                    listaTipos = GetModelosDataTable(filtrarBaja, ID_MtoGeneral, foreignKey, term);
                    break;
                case (int)EnumMtoTiposGenerales.Ruta:
                    listaTipos = GetRutasDataTable(filtrarBaja, ID_MtoGeneral, term);
                    break;
                case (int)EnumMtoTiposGenerales.Seguro:
                    listaTipos = GetTiposSeguroDataTable(filtrarBaja, ID_MtoGeneral, term); 
                    break;
                case (int)EnumMtoTiposGenerales.TarjetaCombustible:
                    listaTipos = GetTiposTarjetaCombustibleDataTable(filtrarBaja, ID_MtoGeneral, term);
                    break;
                case (int)EnumMtoTiposGenerales.Ubicaciones:
                    listaTipos = GetUbicacionesDataTable(filtrarBaja, ID_MtoGeneral, term);
                    break;
                case (int)EnumMtoTiposGenerales.Vehiculos:
                    listaTipos = GetTiposVehiculosDataTable(filtrarBaja, ID_MtoGeneral, term);
                    break;
                case (int)EnumMtoTiposGenerales.TipoLiquidacionSeguro:
                    listaTipos = GetTipoLiquidacionSeguroDataTable(filtrarBaja, ID_MtoGeneral, term);
                    break;
            }

            return listaTipos;
        }

        private List<MtoGenericoTiposModels> GetCarburantesDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {

            T_M_TIPOS_CARBURANTESpecification spec = new T_M_TIPOS_CARBURANTESpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_CARBURANTE = (int?)ID_MtoGeneral;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_TIPOS_CARBURANTE.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) ||
                                            tipoMto.ID_CARBURANTE.ToString().Contains(term) : true)
                                         select new MtoGenericoTiposModels
                                         {
                                             ID_Tipo = tipoMto.ID_CARBURANTE,
                                             ID_Tipo_FOREIGN = 0,
                                             Descripcion = tipoMto.DESCRIPCION,
                                             TipoMtoGenerico = EnumMtoTiposGenerales.Carburante,
                                             Accion= EnumAccionEntity.SinAccion,
                                         }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }

        private List<MtoGenericoTiposModels> GetMarcasDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {
            //ISpecification<T_M_MARCA_VEHICULOS> spec = null;

            T_M_MARCA_VEHICULOSSpecification spec = new T_M_MARCA_VEHICULOSSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_MARCA = (int?)ID_MtoGeneral;
            }


            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_MARCA_VEHICULOS.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) : true)
                             let countModelos = tipoMto.T_M_MODELOS_VEHICULO.Count(x=>x.BAJA==false)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_MARCA,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 PermiteBorrado = (countModelos == 0),
                                 TipoMtoGenerico = EnumMtoTiposGenerales.Marcas,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }


        public MtoGenericoTiposModels GetMarcaByModelo(int IDModelo, bool filtrarBaja = true)
        {
            T_M_MARCA_VEHICULOSSpecification spec = new T_M_MARCA_VEHICULOSSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var marca = (from tipoMto in unitOfWork.RepositoryT_M_MARCA_VEHICULOS.Where(spec)
                             .Where(x=>x.ID_MARCA == x.T_M_MODELOS_VEHICULO.Where(o=>o.ID_MODELO ==IDModelo).FirstOrDefault().ID_MARCA)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_MARCA,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.Marcas,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).FirstOrDefault();

                return marca;
            }
        }

        public List<SelectListItem> GetMarcasSelectListItem(int? itemSeleccionado,bool filtrarBaja = true)
        {

            T_M_MARCA_VEHICULOSSpecification spec = new T_M_MARCA_VEHICULOSSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_MARCA_VEHICULOS.Where(spec)
                             orderby tipoMto.DESCRIPCION
                             select new SelectListItem
                             {
                                 Selected = tipoMto.ID_MARCA == itemSeleccionado ? true : false,
                                 Text = tipoMto.DESCRIPCION,
                                 Value = tipoMto.ID_MARCA.ToString(),
                             }).OrderBy(o => o.Text).ToList();

                return lista;
            }
        }


        private List<MtoGenericoTiposModels> GetModelosDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, int foreignKey = 0, string term = "")
        {

            T_M_MODELOS_VEHICULOSpecification spec = new T_M_MODELOS_VEHICULOSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_MODELO = (int?)ID_MtoGeneral;
            }

            if (foreignKey != 0)
            {
                spec.ID_MARCA = foreignKey;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_MODELOS_VEHICULO.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) : true)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_MODELO,
                                 ID_Tipo_FOREIGN = tipoMto.ID_MARCA,
                                 Descripcion_FOREIGN = tipoMto.T_M_MARCA_VEHICULOS.DESCRIPCION,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 TieneITV = false,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.Modelos,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion_FOREIGN).ThenBy(o=>o.Descripcion).ToList();

                return lista;
            }
        }

        private List<MtoGenericoTiposModels> GetRutasDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {

            T_M_RUTA_VEHICULOSSpecification spec = new T_M_RUTA_VEHICULOSSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_RUTA_VEHICULO = (int?)ID_MtoGeneral;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_RUTA_VEHICULOS.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) : true)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_RUTA_VEHICULO,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 TieneITV = false,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.Ruta,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }

        private List<MtoGenericoTiposModels> GetTiposSeguroDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {

            T_M_TIPO_SEGURO_VEHICULOSpecification spec = new T_M_TIPO_SEGURO_VEHICULOSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_SEGURO_VEHICULO = (int?)ID_MtoGeneral;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_TIPO_SEGURO_VEHICULO.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) : true)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_SEGURO_VEHICULO,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 TieneITV = false,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.Seguro,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }


        public List<SelectChosen> GetTiposTarjetaCombustibleByIDChosen(int id)
        {

            T_M_TIPO_TARJETA_COMBUSTIBLESpecification spec = new T_M_TIPO_TARJETA_COMBUSTIBLESpecification
            {
                ID_TARJETA_COMBUSTIBLE = id,
            };

            return GetTiposTarjetaCombustible_Chosen(spec);
        }


        public List<SelectChosen> GetTiposTarjetaCombustibleChosen(string term, bool filtrarBaja = true)
        {

            T_M_TIPO_TARJETA_COMBUSTIBLESpecification spec = new T_M_TIPO_TARJETA_COMBUSTIBLESpecification
            {
                DESCRIPCIONContains = term,
            };

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            return GetTiposTarjetaCombustible_Chosen(spec);
        }

        private List<SelectChosen> GetTiposTarjetaCombustible_Chosen(T_M_TIPO_TARJETA_COMBUSTIBLESpecification spec)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipo in unitOfWork.RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE.Where(spec)
                             select new SelectChosen
                             {
                                 DevolverValueFormateado = false,
                                 text = tipo.DESCRIPCION,
                                 value = tipo.ID_TARJETA_COMBUSTIBLE.ToString(),
                             }).OrderBy(o => o.text).ToList();

                return lista;
            }
        }



        private List<MtoGenericoTiposModels> GetTiposTarjetaCombustibleDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {

            T_M_TIPO_TARJETA_COMBUSTIBLESpecification spec = new T_M_TIPO_TARJETA_COMBUSTIBLESpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_TARJETA_COMBUSTIBLE = (int?)ID_MtoGeneral;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) : true)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_TARJETA_COMBUSTIBLE,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 TieneITV = false,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.TarjetaCombustible,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }

        private List<MtoGenericoTiposModels> GetUbicacionesDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {
            T_M_UBICACIONESSpecification spec = new T_M_UBICACIONESSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_UBICACION = (int?)ID_MtoGeneral;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_UBICACIONES.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) : true)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_UBICACION,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 TieneITV = false,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.Ubicaciones,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }


        private List<MtoGenericoTiposModels> GetTiposVehiculosDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {
            T_M_TIPOS_VEHICULOSpecification spec = new T_M_TIPOS_VEHICULOSpecification();

            if (filtrarBaja)
            {
                spec.BAJA = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.ID_TIPO_VEHICULO = (int?)ID_MtoGeneral;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryT_M_TIPOS_VEHICULO.Where(spec)
                             where (term != "" ? tipoMto.DESCRIPCION.ToUpper().Contains(term.ToUpper()) : true)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.ID_TIPO_VEHICULO,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.DESCRIPCION,
                                 TieneITV = tipoMto.TIENE_ITV,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.Vehiculos,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }

        private List<MtoGenericoTiposModels> GetTipoLiquidacionSeguroDataTable(bool filtrarBaja = true, int ID_MtoGeneral = 0, string term = "")
        {
            ECAR_Tipo_LiquidacionSpecification spec = new ECAR_Tipo_LiquidacionSpecification();

            if (filtrarBaja)
            {
                spec.Baja = false;
            }

            if (ID_MtoGeneral != 0)
            {
                spec.Cod_tipo = (int?)ID_MtoGeneral;
            }

            using (var unitOfWork = new UnitOfWork())
            {
                var lista = (from tipoMto in unitOfWork.RepositoryECAR_Tipo_Liquidacion.Where(spec)
                             where (term != "" ? tipoMto.Descripcion.ToUpper().Contains(term.ToUpper()) : true)
                             select new MtoGenericoTiposModels
                             {
                                 ID_Tipo = tipoMto.Cod_tipo,
                                 ID_Tipo_FOREIGN = 0,
                                 Descripcion = tipoMto.Descripcion,
                                 TieneITV = false,
                                 TipoMtoGenerico = EnumMtoTiposGenerales.TipoLiquidacionSeguro,
                                 Accion = EnumAccionEntity.SinAccion,
                             }).OrderBy(o => o.Descripcion).ToList();



                return lista;
            }
        }
        #endregion

        #region Mantenimiento Tipos
        public bool SaveMtoTipoGeneral(MtoGenericoTiposModels modelo)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        switch (modelo.TipoMtoGenerico)
                        {
                            case EnumMtoTiposGenerales.Carburante:
                                SaveMtoCarburante(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.Marcas:
                                SaveMtoMarca(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.Modelos:
                                SaveMtoModelo(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.Ruta:
                                SaveMtoRuta(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.Seguro:
                                SaveMtoSeguro(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.TarjetaCombustible:
                                SaveMtoTarjetaCombustible(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.Ubicaciones:
                                SaveMtoUbicaciones(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.Vehiculos:
                                SaveMtoTiposVehiculo(unitOfWork, modelo);
                                break;
                            case EnumMtoTiposGenerales.TipoLiquidacionSeguro:
                                SaveMtoTipoLiquidacionSeguro(unitOfWork, modelo);
                                break;
                        }

                        unitOfWork.Commit();
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Global.EscribeLogApp(Global.TipoDeLog.ERROR, ex.Message);
                return false;
            }

            return true;
        }

        private void SaveMtoCarburante(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_TIPOS_CARBURANTE
            {
                DESCRIPCION = modelo.Descripcion,
                BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_CARBURANTE = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_TIPOS_CARBURANTE.Update(item);
            }
            else
            {
                unitOfWork.RepositoryT_M_TIPOS_CARBURANTE.Insert(item);
            }
        }

        private void SaveMtoModelo(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_MODELOS_VEHICULO
            {
               DESCRIPCION = modelo.Descripcion,
               ID_MARCA = (int)modelo.ID_Tipo_FOREIGN,
               BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_MODELO = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_MODELOS_VEHICULO.Update(item);
            }
            else
            {
                unitOfWork.RepositoryT_M_MODELOS_VEHICULO.Insert(item);
            }
        }

        private void SaveMtoMarca(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_MARCA_VEHICULOS
            {
                DESCRIPCION = modelo.Descripcion,
                BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_MARCA = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_MARCA_VEHICULOS.Update(item);
            }
            else
            {
                unitOfWork.RepositoryT_M_MARCA_VEHICULOS.Insert(item);
            }
        }


        private void SaveMtoRuta(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_RUTA_VEHICULOS
            {
                DESCRIPCION = modelo.Descripcion,
                BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_RUTA_VEHICULO = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_RUTA_VEHICULOS.Update(item);
            }
            else
            {
                unitOfWork.RepositoryT_M_RUTA_VEHICULOS.Insert(item);
            }
        }

        private void SaveMtoSeguro(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_TIPO_SEGURO_VEHICULO
            {
                DESCRIPCION = modelo.Descripcion,
                BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_SEGURO_VEHICULO = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_TIPO_SEGURO_VEHICULO.Update(item);
            }
            else
            {
                unitOfWork.RepositoryT_M_TIPO_SEGURO_VEHICULO.Insert(item);
            }
        }

        private void SaveMtoTarjetaCombustible(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_TIPO_TARJETA_COMBUSTIBLE
            {
                DESCRIPCION = modelo.Descripcion,
                BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_TARJETA_COMBUSTIBLE = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE.Update(item);

                new TarjetasCombustibleService().DeleteTarjetasByIdEmpresaEmisora(item.ID_TARJETA_COMBUSTIBLE, unitOfWork);
            }
            else
            {
                unitOfWork.RepositoryT_M_TIPO_TARJETA_COMBUSTIBLE.Insert(item);
            }

        }

        private void SaveMtoUbicaciones(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_UBICACIONES
            {
                DESCRIPCION = modelo.Descripcion,
                BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_UBICACION = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_UBICACIONES.Update(item);
            }
            else
            {
                unitOfWork.RepositoryT_M_UBICACIONES.Insert(item);
            }
        }

        private void SaveMtoTiposVehiculo(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new T_M_TIPOS_VEHICULO
            {
                DESCRIPCION = modelo.Descripcion,
                TIENE_ITV = modelo.TieneITV,
                BAJA = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.ID_TIPO_VEHICULO = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.BAJA = true;
                }
                unitOfWork.RepositoryT_M_TIPOS_VEHICULO.Update(item);
            }
            else
            {
                unitOfWork.RepositoryT_M_TIPOS_VEHICULO.Insert(item);
            }
        }

        private void SaveMtoTipoLiquidacionSeguro(UnitOfWork unitOfWork, MtoGenericoTiposModels modelo)
        {
            var item = new ECAR_Tipo_Liquidacion
            {
                Descripcion = modelo.Descripcion,
                Baja = false,
            };

            if (modelo.Accion != EnumAccionEntity.Alta)
            {
                item.Cod_tipo = modelo.ID_Tipo;
                if (modelo.Accion == EnumAccionEntity.Baja)
                {
                    item.Baja = true;
                }
                unitOfWork.RepositoryECAR_Tipo_Liquidacion.Update(item);
            }
            else
            {
                unitOfWork.RepositoryECAR_Tipo_Liquidacion.Insert(item);
            }
        }
        #endregion
    }
}