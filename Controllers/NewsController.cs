using System.Web.Mvc;
using alwatnia.Models;

namespace alwatnia.Controllers
{
    public class NewsController : BaseController
    {
        public ActionResult Index(int? id)
        {
            using (var dataModel = new DataModel())
            {
                if (!id.HasValue)
                    return RedirectToAction("News", "Home");
                return View(dataModel.News.Find((object) id.Value));
            }
        }
    }
}