using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using alwatnia.Helper;
using alwatnia.Models;

namespace alwatnia.Controllers
{
    public class BaseController : Controller
    {
        public enum MStatus
        {
            success,
            danger
        }

        public BaseController()
		{
			var lang = Functions.GetLanguage();

			using (var dataModel = new DataModel())
            {
                var cc = dataModel.Companies.ToList();
                ViewBag.company = cc;
                ViewBag.CompanyWithoutMain = cc.Where(t => t.Id != 1).ToList();
                ViewBag.conf = dataModel.Configrations.ToList();
            }
        }

        public ActionResult getMessage(Enum status, string meesage, string action, string controller, int? Id = null)
		{

			TempData["Message"] = string.IsNullOrEmpty(meesage) ? Functions.IsEnglish() ? "Sent successfully":"تمت العملية بنجاح" : meesage;
            TempData["Status"] = status;
		    return Id.HasValue ? RedirectToAction(action, controller, new {id = Id}) : 
                RedirectToAction(action, controller,new RouteValueDictionary{});
		}
    }
}