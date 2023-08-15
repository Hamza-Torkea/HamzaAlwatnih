using alwatnia.Helper;
using alwatnia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CPBaseController : Controller
    {
        public enum MStatus
        {
            success,
            danger
        }

        public ActionResult getMessage(Enum status, string meesage, string action, string controller, int? parameter = null)
        {
            TempData["Message"] = string.IsNullOrEmpty(meesage) ? "تمت العملية بنجاح" : meesage;
            TempData["Status"] = status;
            return !parameter.HasValue ? RedirectToAction(action, controller)
                : RedirectToAction(action, controller, new { id = parameter });
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] file)
        {
            var stringList = Functions.SaveMultiFile(file, "~/Recources/Uploads");
            return Json(new imageFormat
            {
                location = Request.Url.Scheme + "://" + Request.Url.Host + "/Recources/Uploads/" + stringList[0]
            }, JsonRequestBehavior.AllowGet);
        }

        public List<Company> getCompanies()
        {
            using (var dataModel = new DataModel())
            {
                return dataModel.Companies.ToList();
            }
        }
        public List<ProductCat> GetProductCats()
        {
            using (var dataModel = new DataModel())
            {
                return dataModel.ProductCats.ToList();
            }
        }
    }

    public class imageFormat
    {
        public string location { get; set; }
    }
}