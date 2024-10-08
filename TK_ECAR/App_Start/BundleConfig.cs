using System.Configuration;
using System.Web;
using System.Web.Optimization;

namespace TK_ECAR
{
    public class BundleConfig
    {

        private static readonly string sBaseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private static readonly string sBaseStyles = ConfigurationManager.AppSettings["baseStyles"];
        private static readonly string sBaseScripts = ConfigurationManager.AppSettings["baseScripts"];

        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            // Get resources from TK Server
            bundles.UseCdn = true;   //enable CDN support            

            //add TK CDN links
            var jqueryCdnPath = sBaseUrl + sBaseScripts + "/jquery-1.12.0.js";
            var modernizrCdnPath = sBaseUrl + sBaseScripts + "/modernizr-2.8.3.js";
            var globalizeCdnPath = sBaseUrl + sBaseScripts + "/globalize.js";
            var globalizeCultureCdnPath = sBaseUrl + sBaseScripts + "/globalize.culture.es-ES.js";
            var bootstrapCdnPath = sBaseUrl + sBaseScripts + "/bootstrap.js";
            var respondCdnPath = sBaseUrl + sBaseScripts + "/respond.js";
            var momentCdnPath = sBaseUrl + sBaseScripts + "/moment.js";
            var jQueryDataTablesCdnPath = sBaseUrl + sBaseScripts + "/DataTables/jquery.dataTables.js";
            var responsiveDataTablesCdnPath = sBaseUrl + sBaseScripts + "/DataTables/dataTables.responsive.js";
            var dropdownJsCdnPath = sBaseUrl + sBaseScripts + "/dropdown.js";
            var datepickerJsCdnPath = sBaseUrl + sBaseScripts + "/bootstrap-datepicker.js";
            var datepickerLocaleESJsCdnPath = sBaseUrl + sBaseScripts + "/Datepicker/locales/bootstrap-datepicker.es.js";
            
            var choosenJqJsCdnPath = sBaseUrl + sBaseScripts + "/chosen.jquery.js";
            var choosenAjaxJsCdnPath = sBaseUrl + sBaseScripts + "/ajax-chosen.js";
            var jqueryUnobtrusiveAjaxJsCdnPath = sBaseUrl + sBaseScripts + "/jquery.unobtrusive-ajax.js";
            var jQueryValidateCdnPath = sBaseUrl + sBaseScripts + "/jquery.validate.js";
            var jQueryValidateGlobalCdnPath = sBaseUrl + sBaseScripts + "/jquery.validate.globalize.js";
            var jQueryValidateUnotrusiveCdnPath = sBaseUrl + sBaseScripts + "/jquery.validate.unobtrusive.js";
            var toastrCdnPath = sBaseUrl + sBaseScripts + "/toastr.js";
            var bootstrapTreeviewCdnPath = sBaseUrl + sBaseScripts + "/bootstrap-treeview.js";
            var bootstrapDialogCdnPath = sBaseUrl + sBaseScripts + "/bootstrap-dialog.js";
            var bootstrapDialogCSSCdnPath = sBaseUrl + sBaseStyles + "/bootstrap-dialog.css";

            var bootstrapCSSCdnPath = sBaseUrl + sBaseStyles + "/tk_bootstrap.css";
            var dropdownsCdnPath = sBaseUrl + sBaseStyles + "/dropdowns.css";
            var tke_common_whiteCdnPath = sBaseUrl + sBaseStyles + "/tke_common_white.css";
            var tke_common_blueCdnPath = sBaseUrl + sBaseStyles + "/tke_common_blue.css";
            var datepickerCdnPath = sBaseUrl + sBaseStyles + "/bootstrap-datepicker.css";
            var fontawesomeCdnPath = sBaseUrl + sBaseStyles + "/font-awesome.css";
            var tK_jquery_dataTablesCdnPath = sBaseUrl + "/Content/DataTables/css/tK_jquery.dataTables.css";
            var responsive_dataTablesCdnPath = sBaseUrl + "/Content/DataTables/css/responsive.dataTables.css";
            var choosenCSSCdnPath = sBaseUrl + sBaseStyles + "/chosen.css";
            var toastrCSSCdnPath = sBaseUrl + sBaseStyles + "/toastr.css";
            var bootstrapTreeviewCSSCdnPath = sBaseUrl + sBaseStyles + "/bootstrap-treeview.css";

            /* Datatables Buttons*/
            var jQueryDataTablesButtonJsCdnPath = sBaseUrl + sBaseScripts + "/DataTables/dataTables.buttons.js";
            var jQueryDataTablesButtonCSSCdnPath = sBaseUrl + "/Content/DataTables/css/buttons.dataTables.css";

            /* EXport to excel*/
            var jQueryDataTablesjszipCdnPath = sBaseUrl + sBaseScripts + "/DataTables/jszip.min.js";
            var jQueryDataTablesbuttonsHtml5CdnPath = sBaseUrl + sBaseScripts + "/DataTables/buttons.html5.min.js";
            var jQueryDataTablesbuttonsFlashCdnPath = sBaseUrl + sBaseScripts + "/DataTables/buttons.flash.js";
            var jQueryDataTablesbuttonsPrintCdnPath = sBaseUrl + sBaseScripts + "/DataTables/buttons.print.js";
            var jQueryDataTablesvfsFontsCdnPath = sBaseUrl + sBaseScripts + "/DataTables/vfs_fonts.js";

            /* Datatables Sorting Dates*/
            var DateTimeTableSortJsCdnPath = sBaseUrl + sBaseScripts + "/DataTables/datetime-moment.js";

            

            bundles.Add(new ScriptBundle("~/bundles/jquery", jqueryCdnPath));

            bundles.Add(new ScriptBundle("~/bundles/globalize", globalizeCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/globalizeculture", globalizeCultureCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/modernizr", modernizrCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap", bootstrapCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/respond", respondCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapDialog", bootstrapDialogCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/datatable", jQueryDataTablesCdnPath));

            bundles.Add(new ScriptBundle("~/bundles/DataTablesButtonJs", jQueryDataTablesButtonJsCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/DataTablesjszipJs", jQueryDataTablesjszipCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/DataTablesbuttonsHtml5Js", jQueryDataTablesbuttonsHtml5CdnPath));
            bundles.Add(new ScriptBundle("~/bundles/DateTimeTableSortJs", DateTimeTableSortJsCdnPath));

            bundles.Add(new ScriptBundle("~/bundles/DataTablesbuttonsPrintJs", jQueryDataTablesbuttonsPrintCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/DataTablesbuttonsFlashJs", jQueryDataTablesbuttonsFlashCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/DataTablesVfsFontsJs", jQueryDataTablesvfsFontsCdnPath));


            bundles.Add(new ScriptBundle("~/bundles/dtresponsive", responsiveDataTablesCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/dropdown", dropdownJsCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/datepicker", datepickerJsCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/datepickerLocaleES", datepickerLocaleESJsCdnPath));

            

            bundles.Add(new ScriptBundle("~/bundles/moment", momentCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/choosenJqJs", choosenJqJsCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/choosenAjaxJs", choosenAjaxJsCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/toastrJs", toastrCdnPath));
            bundles.Add(new ScriptBundle("~/bootstrapTreeviewCdnPath", bootstrapTreeviewCdnPath));


            bundles.Add(new ScriptBundle("~/bundles/jqueryUnobtrusiveAjax", jqueryUnobtrusiveAjaxJsCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/jQueryValidate", jQueryValidateCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/jQueryValidateGlobal", jQueryValidateGlobalCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/jQueryValidateUnotrusive", jQueryValidateUnotrusiveCdnPath));

            // STYLES

            bundles.Add(new StyleBundle("~/Content/cssbootstrap", bootstrapCSSCdnPath));
            bundles.Add(new StyleBundle("~/Content/cssdropdowns", dropdownsCdnPath));
            bundles.Add(new StyleBundle("~/Content/cssExtra", tke_common_blueCdnPath));
            bundles.Add(new StyleBundle("~/Content/cssIntra", tke_common_whiteCdnPath));

            bundles.Add(new StyleBundle("~/Content/dataTablecss", tK_jquery_dataTablesCdnPath));
            bundles.Add(new StyleBundle("~/Content/dataTablecssResponsive", responsive_dataTablesCdnPath));
            bundles.Add(new StyleBundle("~/Content/DataTablesButtonCSS", jQueryDataTablesButtonCSSCdnPath));

            bundles.Add(new StyleBundle("~/Content/datepicker", datepickerCdnPath));
            bundles.Add(new StyleBundle("~/Content/fontawesome", fontawesomeCdnPath));
            bundles.Add(new StyleBundle("~/Content/choosenCSS", choosenCSSCdnPath));
            bundles.Add(new StyleBundle("~/Content/toastrCSS", toastrCSSCdnPath));
            bundles.Add(new StyleBundle("~/bootstrapTreeviewCSSCdnPath", bootstrapTreeviewCSSCdnPath));

            bundles.Add(new StyleBundle("~/bootstrapDialogCSSCdnPath", bootstrapDialogCSSCdnPath));


            // ADD LOCAL RESOURCES -- NOT RECOMMENDED

            bundles.Add(new ScriptBundle("~/bundles/customannotation").Include(
                      "~/Content/scripts/Application/customannotation.js"));
            

            bundles.Add(new StyleBundle("~/Content/cssLocal").Include(
                          "~/Content/styles/Site.css"
                          ));

        }


 
    }
}
