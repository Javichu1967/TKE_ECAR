using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK_ECAR.Domain;
using TK_ECAR.Domain.Specifications;
using TK_ECAR.Framework;
using TK_ECAR.Infraestructure;
using TK_ECAR.Models;
using TK_ECAR.Utils;
using TKUtilidades;

namespace TK_ECAR.Application_Services
{
    public class UsersService
    {

        private class Dni_Email_IdEmpresa
        {
            public string Dni { get; set; }

            public string Email { get; set; }

            public int IdEmpresa { get; set; }

        }

        public class userInfo
        {
            public string Login { get; set; }

            public int IdEmpresaUsuario { get; set; }

            public int IdPerfil { get; set; }

            public string Nombre { get; set; }

            public string Email { get; set; }

            public string Dni { get; set; }

            public IEnumerable<int> Empresas { get; set; }

            public IEnumerable<string> DireccionesTerritoriales { get; set; }

            public IEnumerable<string> Delegaciones { get; set; }

            public IEnumerable<string> Cecos { get; set; }

            public IEnumerable<MenuUser> Menus { get; set; }

            public FuncionModel Funcion { get; set; }

            public class FuncionModel
            {
                public int IdFuncion { get; set; }
                public string Funcion { get; set; }
            }
        }
        public class MenuUser
        {
            public string Descripcion { get; set; }
            public int idMenu { get; set; }
            public int? idMenuPadre { get; set; }
        }

        public UserModel GetUser(string  logon)
        {
            UserModel userModel = null;

            using (var unitOfWork = new UnitOfWork())
            {
              
                if (unitOfWork.RepositorySAPHR_UsuariosSAP.ExistUserInSAP(logon))
                {
                    var userInfo = (from user in unitOfWork.RepositoryT_G_USUARIOS.GetUser(logon)
                                select new userInfo
                                {
                                    Login = user.LOGIN,

                                    IdPerfil = user.ID_PERFIL,

                                    Email = user.EMAIL,

                                    Nombre = user.NOMBRE,

                                    Empresas = (from empresa in user.T_G_USUARIOS_EMPRESAS                                               
                                                select empresa.ID_EMPRESA),

                                    DireccionesTerritoriales = (from dir in user.T_G_USUARIOS_DIR_TERRITORIAL
                                                                select dir.ID_DT),

                                    Delegaciones = (from delegacion in user.T_G_USUARIOS_DELEGACION
                                                    select delegacion.ID_DELEGACION),

                                    Cecos = (from ceco in user.T_G_USUARIOS_CECO
                                             select ceco.ID_CECO),

                                    Menus =  (from menu in user.T_M_PERFILES.T_R_PERFILES_MENU
                                              where menu.B_ACTIVO
                                              where user.T_M_PERFILES.B_ACTIVO
                                              orderby menu.T_G_MENUS.ORDEN
                                             select  new MenuUser
                                             {
                                                 Descripcion = menu.T_G_MENUS.DESCRIPCION,
                                                 idMenu = menu.ID_MENU,
                                                 idMenuPadre = menu.T_G_MENUS.ID_MENU_PARENT
                                             })


                                }).SingleOrDefault();

                    
                    if (userInfo != null)
                    {//completamos su configuración a donde corresponda.
                        setConfiguracionSecurity(unitOfWork, userInfo);

                        SAPHR_UsuariosSAPSpecification spec = new SAPHR_UsuariosSAPSpecification
                        {
                            Logon = logon
                        };
                        var Dni_Email_IdEmpresa = (from usuario in unitOfWork.RepositorySAPHR_UsuariosSAP.Where(spec)
                                        select new Dni_Email_IdEmpresa
                                        {
                                            Dni = usuario.Dni,
                                            Email = usuario.Email,
                                            IdEmpresa = usuario.CodigoEmpresa,
                                        }).FirstOrDefault();

                        userInfo.Dni = Dni_Email_IdEmpresa.Dni;
                        userInfo.Email = Dni_Email_IdEmpresa.Email ?? "";
                        userInfo.IdEmpresaUsuario = Dni_Email_IdEmpresa.IdEmpresa;

                        userModel = new UserModel
                        {
                            Login = userInfo.Login,

                            IdEmpresaUsuario = userInfo.IdEmpresaUsuario,

                            IdPerfil = userInfo.IdPerfil,

                            Email = userInfo.Email,

                            Nombre = userInfo.Nombre,

                            Dni = userInfo.Dni,

                            CecosModelList = getCecosModelUser(unitOfWork, userInfo.Cecos),

                            Empresas = getEmpresasModelUser(unitOfWork, userInfo.Empresas),

                            OpcionesMenu = (from menu in userInfo.Menus
                                            select new MenuModel
                                            {
                                                idMenu = menu.idMenu,
                                                Descripcion = menu.Descripcion,
                                                idMenuPadre = menu.idMenuPadre
                                            })

                        };
                        

                    }
                        

                }

            }

            return userModel;
               
        }

        public List<string> GetReceiveEmail(string idceco)
        {
            List<string> emailList = new List<string>();

            using (var unitOfWork = new UnitOfWork())
            {

              
                var users = (from user in unitOfWork.RepositoryT_G_USUARIOS.GetAlertaEmail()
                             select new userInfo
                                {
                                    Login = user.LOGIN,

                                    IdPerfil = user.ID_PERFIL,

                                    Email = user.EMAIL,

                                    Nombre = user.NOMBRE,

                                    Empresas = (from empresa in user.T_G_USUARIOS_EMPRESAS
                                                select empresa.ID_EMPRESA),

                                    DireccionesTerritoriales = (from dir in user.T_G_USUARIOS_DIR_TERRITORIAL
                                                                select dir.ID_DT),

                                    Delegaciones = (from delegacion in user.T_G_USUARIOS_DELEGACION
                                                    select delegacion.ID_DELEGACION),

                                    Cecos = (from ceco in user.T_G_USUARIOS_CECO
                                                select ceco.ID_CECO),
 

                                }).ToList();


 
                var loginsExistSAP = unitOfWork.RepositorySAPHR_UsuariosSAP
                    .ReturnLoginExistInSAP(users.Select(x => x.Login)) ;

                foreach (var user in users)
                {
                    //comprobamos que existe en SAP
                    if (loginsExistSAP.Any(x=>x.Equals(user.Login.ToUpper())))
                    {
                        setConfiguracionSecurity(unitOfWork, user);

                        if (user.Cecos.Any(x => x.TrimStart('0').Equals(idceco)))//y si puede ver el ceco
                        {
                            emailList.Add(user.Email);

                        }
                    }
                   
                }


                return emailList;
 
            }

        }

        public List<string> GetReceiveEmailGestores()
        {
            List<string> emailList = new List<string>();

            using (var unitOfWork = new UnitOfWork())
            {
                var users = (from user in unitOfWork.RepositoryT_G_USUARIOS.GetUsuarioByPerfil((int)EnumTipoPerfil.SuperUsuario)
                             select new userInfo
                             {
                                 Login = user.LOGIN,

                                 Email = user.EMAIL,

                                 Nombre = user.NOMBRE,

                             }).ToList();



                var loginsExistSAP = unitOfWork.RepositorySAPHR_UsuariosSAP
                    .ReturnLoginExistInSAP(users.Select(x => x.Login));

                foreach (var user in users)
                {
                    //comprobamos que existe en SAP
                    if (loginsExistSAP.Any(x => x.Equals(user.Login.ToUpper())))
                    {
                        emailList.Add(user.Email);
                    }
                }

                return emailList;
            }

        }




        private void setConfiguracionSecurity(IUnitOfWork unitOfWork, userInfo userInfo )
        {
            if (!userInfo.Empresas.Any())
            {
                //configuración desde sap
                setConfiguracionSAP(unitOfWork, userInfo );

            }

            //comprobamos que tiene dir. territoriales
            if (!userInfo.DireccionesTerritoriales.Any())
            {
                //nos traemos las dir.territorial de las empresas
                //si es una empresa diferente a la 8100 no tendrá dir. territorial
                setDireccionesTerritorialesUser(unitOfWork, userInfo);
            }
            //comprobamos que tiene delegaciones
            if (!userInfo.Delegaciones.Any())
            {
                //nos traemos las delegaciones de su dirrección territorial o de sus empresas
                // si no tuviera dirrección territorial
                setDelegacionesUser(unitOfWork, userInfo);
            }
            //comprobamos que tiene cecos
            if (!userInfo.Cecos.Any())
            {
                //nos traemos los cecos de sus delegaciones  
                setCecosUser(unitOfWork, userInfo);
            }
        }

        private void setConfiguracionSAP(IUnitOfWork unitOfWork, userInfo userInfo  )
        {
            var esDelegadoZona = userInfo.IdPerfil == (int)EnumTipoPerfil.DelegadoZona;
            if (esDelegadoZona)
            {
                //si es delegado de zona y tiene config. de zonas
                esDelegadoZona = setConfigDelegadoZonaUser(unitOfWork, userInfo);
            }

            
            if (!esDelegadoZona)
            {//Configuración por defecto de sap 
                var userSap = unitOfWork.RepositorySAPHR_UsuariosSAP.FindOne(x => x.Logon.ToUpper().Equals(userInfo.Login));

                userInfo.Empresas = new List<int> { userSap.CodigoEmpresa };

                //El superUsuario hasta el nivel de empresas
                //El dir. territorial , controller y delegado de zona  hasta el nivel de dir. territorial
                //Administrativo   hasta el nivel de delegacion  

                if (!string.IsNullOrEmpty(userSap.IdDT) && userInfo.IdPerfil != (int)EnumTipoPerfil.SuperUsuario)
                {
                    userInfo.DireccionesTerritoriales = new List<string> { userSap.IdDT };
                }

                if (!string.IsNullOrEmpty(userSap.IdDelegacion) &&
                        userInfo.IdPerfil != (int)EnumTipoPerfil.DirectorTerritorial &&
                        userInfo.IdPerfil != (int)EnumTipoPerfil.Controller &&
                        userInfo.IdPerfil != (int)EnumTipoPerfil.DelegadoZona)
                {
                    userInfo.Delegaciones = new List<string> { userSap.IdDelegacion };
                }
            }
        }

        private void setDireccionesTerritorialesUser(IUnitOfWork unitOfWork, userInfo userInfo)
        {

            userInfo.DireccionesTerritoriales = unitOfWork.RepositorySAPHR_DireccionesTerritoriales
                .GetDirrecionesTerritorialesByEmpresas(userInfo.Empresas)
                .Select(x => x.IdDT)
                .ToList();
        }

        private void setDelegacionesUser(IUnitOfWork unitOfWork, userInfo userInfo)
        {
            userInfo.Delegaciones =  unitOfWork.RepositorySAPHR_Delegaciones
                .GetDelegacionesByEmpresasOrDT(userInfo.Empresas, userInfo.DireccionesTerritoriales)
                 .Select(x => x.IdDelegacion)                
                .ToList();

        }

        private void setCecosUser(IUnitOfWork unitOfWork, userInfo userInfo)
        {

            userInfo.Cecos = unitOfWork.RepositorySAPHR_CentrosCoste
                .GetCecosByDelegaciones(userInfo.Delegaciones, userInfo.Empresas, userInfo.DireccionesTerritoriales)
                .Select(x => x.IdCeco)
                    .ToList(); 

        }

        /// <summary>
        /// Devuelve TRUE si tiene una configuración por zona en caso contrario FALSE.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private bool setConfigDelegadoZonaUser(IUnitOfWork unitOfWork, userInfo userInfo)
        {
 

            var delegacionesSap = (from delegacion in unitOfWork.RepositorySAPHR_Delegaciones.GetDelegacionesByUserZona(userInfo.Login)
                                   select new
                                   {
                                       delegacion.IdDelegacion,
                                       delegacion.IdDT,
                                       delegacion.Empresa 
                                   }).ToList();
            bool hasConfigZona = delegacionesSap.Any();

            if (hasConfigZona)
            {

                userInfo.Empresas = delegacionesSap.Select(x => x.Empresa).Distinct().ToList();

                userInfo.DireccionesTerritoriales = delegacionesSap.Where(x => !string.IsNullOrEmpty(x.IdDT)).Select(x => x.IdDT).Distinct().ToList();

                userInfo.Delegaciones = delegacionesSap.Where(x => !string.IsNullOrEmpty(x.IdDelegacion)).Select(x => x.IdDelegacion).Distinct().ToList();
            }

            return hasConfigZona;
        }

        private List<CecosModel> getCecosModelUser(IUnitOfWork unitOfWork, IEnumerable<string> cecos)
        {
            var cecosList =  (from ceco in unitOfWork.RepositorySAPHR_CentrosCoste.GetCecos(cecos, false) 
                    select new CecosModel
                    {
                        CodigoEmpresa = ceco.Empresa,
                        IdDelegacion = ceco.IdDelegacion,
                        IdDT = ceco.IdDT,
                        IdCeco = ceco.IdCeco ,
                        IdCecoFormatted = ceco.IdCeco,
                        Ceco = ceco.Nombre,                                       
                        Empresa = ceco.SAPHR_Empresas.Nombre,
                        Delegacion = ceco.SAPHR_Delegaciones.Nombre,
                        DireccionTerritorial = ceco.SAPHR_DireccionesTerritoriales.Nombre,
                        Baja = ceco.Baja,                                   
                    }).ToList();

            cecosList.ForEach(x => x.IdCeco = x.IdCeco.TrimStart('0'));

            return cecosList;


         }

        private List<int> getEmpresasModelUser(IUnitOfWork unitOfWork, IEnumerable<int> empresas)
        {
            var empresasList = (from emp in unitOfWork.RepositorySAPHR_Empresas.GetEmpresas(empresas, false)
                             select new 
                             {
                                 emp.CodigoEmpresa,
                             }).ToList();


            return empresasList.Select(x=> x.CodigoEmpresa).ToList();
        }


        //private List<MenuModel> getMenusModelUser(IEnumerable<MenuUser> menuUsuario)
        //{
        //    List<MenuModel> menu = new List<MenuModel>();

        //    foreach (var item in menuUsuario.Where(o=>o.idMenuPadre == null))
        //    {
        //        menu.Add(new MenuModel
        //        { idMenu = item.idMenu,
        //          Descripcion = item.Descripcion
        //        });
        //    }


        //    foreach (var item in menuUsuario.Where(o => o.idMenuPadre != null))
        //    {
        //        menu.Find(o => o.idMenu == item.idMenuPadre).subMenus = getSubMenusModelUser(menuUsuario, item.idMenuPadre);
        //    }
        //    return menu;
        //}

        //private List<MenuModel> getSubMenusModelUser(IEnumerable<MenuUser> _menuUsuario, int? menuPadre)
        //{
        //    List<MenuModel> subMenu = new List<MenuModel>();

        //    foreach (var item in _menuUsuario.Where(o => o.idMenuPadre == menuPadre))
        //    {
        //        subMenu.Add(new MenuModel
        //        { idMenu = item.idMenu,
        //          Descripcion = item.Descripcion
        //        });
        //    }
        //    return subMenu;
        //}

        public IEnumerable<SelectListItem> GetIdiomas(string valorDefecto = "")
        {
            List<SelectListItem> Listidiomas = new List<SelectListItem>();

            GENERAL_IDIOMASSpecification spec = new GENERAL_IDIOMASSpecification
            {
                ACTIVO = true,
                ID_APLICACION = Global.codigoAplicacion,
            };

            using (var unitOfWork = new UnitOfWork())
            {
                Listidiomas = (from idioma in unitOfWork.RepositoryGENERAL_IDIOMAS.Where(spec)
                               select new SelectListItem
                               {
                                   Text = idioma.DESCRIPCION,
                                   Value = idioma.IDIOMA,
                                   Selected = idioma.IDIOMA == valorDefecto ? true : false,
                               }).ToList();

            }

            return Listidiomas;
        }

        public IEnumerable<SelectListItem> GetEmpresasUserListItems(string valorDefecto = "")
        {

            SAPHR_EmpresasSpecification spec = new SAPHR_EmpresasSpecification
            {
                Activo = true,
            };

            var empresasUsuario = ((UserModel)Util.GetItemFromMemory("userProfile")).Empresas;

            using (var unitOfWork = new UnitOfWork())
            {
                var LEmpresas = (from emp in unitOfWork.RepositorySAPHR_Empresas.Where(spec)
                             where empresasUsuario.Contains(emp.CodigoEmpresa)
                               select new SelectListItem
                               {
                                   Text = emp.Nombre,
                                   Value = emp.CodigoEmpresa.ToString(),
                                   Selected = emp.CodigoEmpresa.ToString() == valorDefecto ? true : false,
                               }).ToList();

                return LEmpresas;
            }
        }

        public IEnumerable<int> GetEmpresasUsuario()
        {
            var empresasUsuario = ((UserModel)Util.GetItemFromMemory("userProfile")).Empresas;
            return empresasUsuario;
        }


        public List<UsuarioSapPeticionAcceso> GetUsuarioSAP_PeticionAcceso(List<string> aLogin)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                ISpecification<SAPHR_UsuariosSAP> spec = new SAPHR_UsuariosSAPSpecification()
                {
                    LogonIN = aLogin,
                };

                var usuariosSAP = (from uSAP in unitOfWork.RepositorySAPHR_UsuariosSAP.Where(spec)
                                   where uSAP.IdUniOrganizativa != "50003566" && uSAP.IdDivision != "FE01" //Jubilados
                                   select new UsuarioSapPeticionAcceso
                                   {
                                       Nombre = uSAP.Nombre + " " + uSAP.Apellido1 + " " + uSAP.Apellido2,
                                       Email = uSAP.Email,
                                   }).ToList();

                return usuariosSAP;

            }
        }

    }
}