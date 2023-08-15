using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using alwatnia.Helper;
using alwatnia.Models;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class ProductCatController : CPBaseController
    {
        public ActionResult Index()
        {
            ViewBag.company = getCompanies(); 
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var companyId = Request.QueryString["companyId"];

                var list = dataModel.ProductCats.OrderByDescending(o => o.Id).ToList();
                if (!string.IsNullOrEmpty(titleName))
                    list = list.Where(w => w.ArabicTitle.Contains(titleName) 
                    || w.EnglishTitle.Contains(titleName)).ToList();

                if (!string.IsNullOrEmpty(companyId))
                    list = list.Where(w => w.CompanyId == int.Parse(companyId)).OrderByDescending(o => o.Id).ToList();

                return View(list);
            }
        }

        public ActionResult Create()
        {
            ViewBag.company = getCompanies();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCat model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();
            var tuple = Functions.ValidateImage(file, true);
            if (!tuple.Item1)
            {
                TempData["Message"] = tuple.Item2;
                TempData["Status"] = "danger";
                return View(model);
            }
            var str = Functions.SaveTempFile(file, "~/Recources/ProductCat");
            model.Photo = str;
            model.CreatedOn = DateTime.Now;
            using (var dataModel = new DataModel())
            {
                dataModel.ProductCats.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "ProductCat");
            }
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.company = getCompanies();
            if (!id.HasValue)
                return RedirectToAction("index");
            using (var dataModel = new DataModel())
            {
                return View(dataModel.ProductCats.Find(id.Value));
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCat model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();
            using (var dataModel = new DataModel())
            {
                if (file != null)
                {
                    var tuple = Functions.ValidateImage(file, true);
                    if (!tuple.Item1)
                    {
                        TempData["Message"] = tuple.Item2;
                        TempData["Status"] = "danger";
                        return View(model);
                    }
                    var str = Functions.SaveTempFile(file, "~/Recources/ProductCat");
                    model.Photo = str;
                }
                model.CreatedOn = DateTime.Now;
                dataModel.Entry(model).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "ProductCat");
            }
        }

        public ActionResult delete(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");
            using (var dataModel = new DataModel())
            {
                var prodeuctCat = dataModel.ProductCats
                    .Include(t => t.Products)
                    .FirstOrDefault(t => id.Value == t.Id);
                if (prodeuctCat.Products?.Count > 0)
                {
                    TempData["Message"] = "التصنيف يحتوي على منتجات .. يجب حذف كافة منتجات التصنيف ومن ثم حذف التصنيف";
                    TempData["Status"] = "danger";
                    return getMessage(MStatus.danger, "", "Index", "ProductCat");
                }
                dataModel.ProductCats.Remove(prodeuctCat);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "ProductCat");
            }
        }
    }
}