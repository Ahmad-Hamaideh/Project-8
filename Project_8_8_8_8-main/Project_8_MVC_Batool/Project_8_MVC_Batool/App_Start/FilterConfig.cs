using System.Web;
using System.Web.Mvc;

namespace Project_8_MVC_Batool
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
