using System.Web.Mvc;

namespace alwatnia
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add((object)new HandleErrorAttribute());
        }
    }
}
