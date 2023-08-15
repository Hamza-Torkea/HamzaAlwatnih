using System.Web.Mvc;

namespace alwatnia.Areas.Helper
{
    public class HelperAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Helper";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Helper_default",
                "Helper/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}