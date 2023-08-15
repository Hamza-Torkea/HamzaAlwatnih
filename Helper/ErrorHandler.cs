using System.Web.Mvc;

namespace alwatnia.Helper
{
    public class ErrorHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            if (filterContext.HttpContext.Session != null) filterContext.HttpContext.Session.RemoveAll();
            // set this to true so that IIS 7 does not use its own error pages
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}