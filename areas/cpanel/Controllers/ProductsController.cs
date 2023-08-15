using alwatnia.Helper;
using alwatnia.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class ProductsController : CPBaseController
    {
        public ActionResult Index()
        {
            ViewBag.productCats = GetProductCats();
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var CatId = Request.QueryString["companyId"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.Products
                    .Include("Comments").OrderByDescending(o => o.Id).ToList();
                if (!string.IsNullOrEmpty(titleName))
                    list = list.Where(w => w.Title.Contains(titleName)).ToList();

                if (!string.IsNullOrEmpty(CatId))
                    list = list.Where(w => w.ProductCatId == int.Parse(CatId)).OrderByDescending(o => o.Id).ToList();

                if (!string.IsNullOrEmpty(langId))
                    list = list.Where(w => w.Lang == int.Parse(langId)).OrderByDescending(o => o.Id).ToList();

                return View(list);
            }
        }

        public ActionResult Create()
        {
            ViewBag.productCats = GetProductCats();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product model, HttpPostedFileBase file)
        {
            var productCats = GetProductCats();
            ViewBag.productCats = productCats;
            var tuple = Functions.ValidateImage(file, true);
            if (!tuple.Item1)
            {
                TempData["Message"] = tuple.Item2;
                TempData["Status"] = "danger";
                return View(model);
            }
            var str = Functions.SaveTempFile(file, "~/Recources/Products");
            model.Photo = str;
            model.Cdate = DateTime.Now;
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }

            model.company_Id = productCats.FirstOrDefault(s => s.Id == model.ProductCatId)
                ?.CompanyId;

            using (var dataModel = new DataModel())
            {
                dataModel.Products.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Products");
            }
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.productCats = GetProductCats();
            if (!id.HasValue)
                return RedirectToAction("index");
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Products.Find((object)id.Value));
            }
        }

        [HttpPost]
        public ActionResult Edit(Product model, HttpPostedFileBase file)
        {
            var productCats = GetProductCats();
            ViewBag.productCats = productCats;
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
                    var str = Functions.SaveTempFile(file, "~/Recources/Products");
                    model.Photo = str;
                }
                if (!model.Lang.HasValue)
                {
                    model.Lang = 1;
                }

                model.company_Id = productCats.FirstOrDefault(s => s.Id == model.ProductCatId)
                    ?.CompanyId;

                dataModel.Entry(model).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Products");
            }
        }

        public ActionResult delete(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");
            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Products
                    .Include(e => e.Comments)
                    .Include(e => e.RequestedProduct)
                    .Single(s => s.Id == id.Value);

                if (entity.Comments.Any())
                {
                    dataModel.Comments.RemoveRange(entity.Comments);
                }

                if (entity.RequestedProduct.Any())
                {
                    dataModel.RequestedProduct.RemoveRange(entity.RequestedProduct);
                }

                dataModel.Products.Remove(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Products");
            }
        }

        public ActionResult Comments(int? id, string comment_auther, string title)
        {
            using (var dataModel = new DataModel())
            {
                var list = dataModel.Comments.Where(w => w.news_id.HasValue).Include(w => w.Product).ToList();
                if (id.HasValue)
                    list = list.Where(w =>
                    {
                        var productId = w.product_id;
                        var nullable = id;
                        return productId.GetValueOrDefault() == nullable.GetValueOrDefault() &&
                               productId.HasValue == nullable.HasValue;
                    }).ToList();
                if (!string.IsNullOrEmpty(comment_auther))
                    list = list.Where(w => w.comment_auther.Contains(comment_auther)).ToList();
                if (!string.IsNullOrEmpty(title))
                    list = list.Where(w => w.Product.Title.Contains(title)).ToList();
                return View(list);
            }
        }
    }
}