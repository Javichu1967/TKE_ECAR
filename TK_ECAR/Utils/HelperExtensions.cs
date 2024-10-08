using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Helpers;
using System.Web.Mvc.Html;
using TK_ECAR.Framework;
using System.ComponentModel;
using System.Linq.Expressions;
using TK_ECAR.Application_Services;
using TK_ECAR.Content.resources;
using TK_ECAR.Controllers;
using TK_ECAR.Models;

namespace HelperExtensionsNameSpace
{
    public static class HelperExtensions
    {
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAtrributes)
        {
            var repID = Guid.NewGuid().ToString();
            
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAtrributes);

            return MvcHtmlString.Create(lnk.ToString().Replace(repID,linkText));
        }

        
        public static string ContentAbsUrl(this UrlHelper url, string relativeContentPath)
        {
            Uri contextUri = HttpContext.Current.Request.Url;

           
            var baseUri = string.Format("{0}://{1}{2}", contextUri.Scheme,
                contextUri.Host, contextUri.Port == 80 ? string.Empty : ":" + contextUri.Port);

            return string.Format("{0}{1}", baseUri, VirtualPathUtility.ToAbsolute(relativeContentPath));
        }

        public static MvcHtmlString IsActive(this HtmlHelper htmlHelper, string action, string controller, string activeClass, string inActiveClass = "")
        {

            var routeData = htmlHelper.ViewContext.RouteData;
                         
            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController && action == routeAction);

            return new MvcHtmlString(returnActive ? activeClass : inActiveClass);

           
        }

        public static MvcHtmlString DropDownListMotivoSolicitudSOLRED(this HtmlHelper helper, string name, object htmlAttributes, int? idvaloracion)
        {
            var values = new AlertasService().AllMotivoSolicitudSOLRED()
            //var values = Enum.GetValues(typeof(EnumTipoSolicitudSOLRED)).Cast<EnumTipoSolicitudSOLRED>()
                       .Select(enuValoracion => new SelectListItem
                       {
                           Selected = idvaloracion == (int)enuValoracion.ID ? true : false,
                           //Text = EnumUtils<EnumTipoSolicitudSOLRED>.GetDescription(enuValoracion),
                           Text = enuValoracion.DESCRPICION,
                           Value = (enuValoracion.ID).ToString()
                       });
            
            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);


        }
        public static MvcHtmlString DropDownListTipoClasificacion(this HtmlHelper helper, string name, object htmlAttributes, int? idvaloracion)
        {
            var values = new AlertasService().AllTiposClasficacion()          
                       .Select(enuValoracion => new SelectListItem
                       {
                           Selected = idvaloracion == (int)enuValoracion.ID ? true : false,                           
                           Text = enuValoracion.DESCRPICION,
                           Value = (enuValoracion.ID).ToString()
                       });

            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);


        }
        public static MvcHtmlString DropDownListTiposAlertas(this HtmlHelper helper, string name, object htmlAttributes, int? idvaloracion)
        {

            var values = new PreguntasService().AllTipoAlertas()
                       .Select(enuValoracion => new SelectListItem
                       {
                           Selected = idvaloracion == (int)enuValoracion.idCategoria ? true : false,
                           Text = enuValoracion.Descripcion,
                           Value = ((int)enuValoracion.idCategoria).ToString()
                       });

            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);
        }

        public static MvcHtmlString DropDownListOrdenacionCategoriasPreguntas(this HtmlHelper helper, string name, object htmlAttributes, int? idvaloracion, EnumAccionEntity accion)
        {
            var values = new List<SelectListItem>();
            var ordenacion = new CategoriasPreguntasService().OrdenacionCategoriasPreguntas(accion);

            for (int i = 0; i <= ordenacion.Count() - 1; i++)
            {
                values.Add(new SelectListItem
                {
                    Selected = ordenacion[i].ToUpper() == "ULTIMO" ? true : false,
                    Text = ordenacion[i],
                    Value = (i + 1).ToString()
                });
            }

            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);
        }


        public static MvcHtmlString DropDownListEmpresas(this HtmlHelper helper, string name, object htmlAttributes, int? idempresa, EnumAccionEntity accion)
        {
            var values = new List<SelectListItem>();
            string valorSeleccionado = "";
            if (idempresa != null)
            {
                valorSeleccionado = idempresa.ToString();
            }
            values = new UsersService().GetEmpresasUserListItems(valorSeleccionado).ToList();

            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);
        }

        public static MvcHtmlString DropDownListTiposCategorias(this HtmlHelper helper, string name, object htmlAttributes, int? idvaloracion)
        {

            var values = new CategoriasService().AllCategorias()
                       .Select(enuValoracion => new SelectListItem
                       {
                           Selected = idvaloracion == (int)enuValoracion.ID_Categoria ? true : false,
                           Text = enuValoracion.Nombre,
                           Value = ((int)enuValoracion.ID_Categoria).ToString()
                       });

            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);
        }


        public static MvcHtmlString DropDownListTiposCategoriasPreguntas(this HtmlHelper helper, string name, object htmlAttributes, int? idvaloracion)
        {

            var values = new CategoriasPreguntasService().AllCategoriasPreguntas()
                       .Select(enuValoracion => new SelectListItem
                       {
                           Selected = idvaloracion == (int)enuValoracion.ID_Categoria ? true : false,
                           Text = enuValoracion.Nombre,
                           Value = ((int)enuValoracion.ID_Categoria).ToString()
                       });

            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);
        }


        public static MvcHtmlString DropDownListOrdenacionCategorias(this HtmlHelper helper, string name, object htmlAttributes, int? idvaloracion, EnumAccionEntity accion)
        {
            var values = new List<SelectListItem>();
            var ordenacion = new CategoriasService().OrdenacionCategorias(accion);

            for (int i = 0; i <= ordenacion.Count() - 1; i++)
            {
                values.Add(new SelectListItem
                {
                    Selected = ordenacion[i].ToUpper() == "ULTIMO" ? true : false,
                    Text = ordenacion[i],
                    Value = (i + 1).ToString()
                });
            }

            return SelectExtensions.DropDownList(helper, name, values, TK_ECAR_Resource.LSeleccionar, htmlAttributes);
        }

        public static MvcHtmlString ListBoxAgrupaciones(this HtmlHelper helper, string name, object htmlAttributes, string idagrupacion)
        {

            var values = new FilterService().GetAgrupacionChosen()
                       .Select(item => new SelectListItem
                       {
                           Selected = idagrupacion == item.value ? true : false,
                           Text = item.text,
                           Value = item.value
                       });

           
            return SelectExtensions.ListBox(helper, name, values,  htmlAttributes);
        }

        public static MvcHtmlString ListBoxEstadoVehiculo(this HtmlHelper helper, string name, object htmlAttributes, string idestadovehiculo)
        {

            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem
            {
                Selected = true,
                Text = TK_ECAR.Content.resources.TK_ECAR_Resource.SelectTodosLosVehiculos,
                Value = EnumAltasBajasAmbas.Todas.ToString()
            });
            values.Add(new SelectListItem
            {
                Selected = false,
                Text = TK_ECAR.Content.resources.TK_ECAR_Resource.SelectVehiculosAlta,
                Value = EnumAltasBajasAmbas.Altas.ToString()
            });
            values.Add(new SelectListItem
            {
                Selected = false,
                Text = TK_ECAR.Content.resources.TK_ECAR_Resource.SelectVehiculosBaja,
                Value = EnumAltasBajasAmbas.Bajas.ToString()
            });

            return SelectExtensions.DropDownList(helper, name, values, htmlAttributes);
        }

        public static MvcHtmlString DropDownListIdiomas(this HtmlHelper helper, string name, object htmlAttributes, string idiomaSeleccionado)
        {

            var values = new UsersService().GetIdiomas(idiomaSeleccionado);


            return SelectExtensions.DropDownList(helper, name, values, htmlAttributes);
        }

        public static MvcHtmlString DropDownListMarcasVehiculo(this HtmlHelper helper, string name, object htmlAttributes, int? idmarca = null)
        {

            var values = new MtoTiposGeneralesService().GetMarcasSelectListItem(idmarca);

            return SelectExtensions.DropDownList(helper, name, values, htmlAttributes);
        }


        public static MvcHtmlString RequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metaData.DisplayName ?? metaData.PropertyName ?? htmlFieldName.Split('.').Last();

            //if (metaData.IsRequired)
                labelText += "<span class=\"required\">*</span>";

            if (String.IsNullOrEmpty(labelText))
                return MvcHtmlString.Empty;

            var label = new TagBuilder("label");
            label.Attributes.Add("for", helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(htmlAttributes))
            {
                label.MergeAttribute(prop.Name.Replace('_', '-'), prop.GetValue(htmlAttributes).ToString(), true);
            }

            label.InnerHtml = labelText;
            return MvcHtmlString.Create(label.ToString());
        }


        public static string GetTreeViewJsonFormat(this string str)
        { return "[" + str + "]"; 
        }
        public static string MergeJsonString(this string str, string strTwo)
        {
            if (String.IsNullOrEmpty(str))
            {
                return strTwo;
            }
            else
            {
                return str + "," + strTwo;
            }
        }

        public static bool OptionMenuIsVisible(this HtmlHelper htmlHelper, EnumOpcionesMenu opcion)
        {
            var controller = htmlHelper.ViewContext.Controller as BaseController;
            bool retunValue = false;
            if (controller != null)
            {
                retunValue = controller.OptionMenuVisible(opcion);
            }
            return retunValue;
        }

        //public static List<MenuModel> UserMenu(this HtmlHelper htmlHelper)
        //{
        //    var controller = htmlHelper.ViewContext.Controller as BaseController;
        //    List<MenuModel> retunValue = new List<MenuModel>();
        //    if (controller != null)
        //    {
        //        retunValue = controller.GetUserMenu().ToList();
        //    }
        //    return retunValue;
        //}

        //public static string MyResource<T>(this HtmlHelper html, object key)
        //{
        //    return new System.Resources.ResourceManager(typeof(T)).GetString(key.ToString());
        //}

    }

}