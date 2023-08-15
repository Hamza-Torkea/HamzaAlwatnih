using System.Web.Mvc;

namespace alwatnia.Areas.Unit
{
    public class UnitAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Unit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Unit_default",
                "Unit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}