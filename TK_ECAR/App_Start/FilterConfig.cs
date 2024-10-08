using System.Web;
using System.Web.Mvc;
using TK_ECAR.Filters;

namespace TK_ECAR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleCustomException());
            filters.Add(new LocalizationAttribute());
        }
    }
}
