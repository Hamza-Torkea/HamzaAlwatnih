using System.Web.Mvc;

namespace alwatnia.Controllers
{
    public class MessageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult TempMessage()
        {
            return PartialView();
        }
    }
}