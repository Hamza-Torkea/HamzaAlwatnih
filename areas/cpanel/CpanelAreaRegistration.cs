using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel
{
    public class CpanelAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Cpanel";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Cpanel_default",
                "Cpanel/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "alwatnia.Areas.Cpanel.Controllers" }
            );

            //context.MapRoute(
            //    "Cpanel_default",
            //    "Cpanel/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}