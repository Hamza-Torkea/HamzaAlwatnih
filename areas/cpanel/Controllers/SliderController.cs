using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using alwatnia.Areas.Cpanel.Models;
using alwatnia.Helper;
using alwatnia.Models;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class SliderController : CPBaseController
    {
        public ActionResult Index(string title, int? lang)
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.Sliders.OrderByDescending(o => o.Id).ToList();

                if (!string.IsNullOrEmpty(titleName))
                    list = list.Where(w => w.Title.Contains(title)).OrderByDescending(o => o.Id).ToList();

                if (!string.IsNullOrEmpty(langId))
                    list = list.Where(w => w.Lang == int.Parse(langId)).OrderByDescending(o => o.Id).ToList();

                return View(list);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Slider model, HttpPostedFileBase file)
        {
            if (!Functions.ValidateImage(file, true).Item1)
            {
                TempData["Message"] = "حجم الصورة يجب أن يكون على الأقل 600*600";
                TempData["Status"] = "danger";
                return View(model);
            }
            var str = Functions.SaveTempFile(file, "~/Recources/Slider");
            model.Photo = str;
           
            model.Cdate = DateTime.Now;
            using (var dataModel = new DataModel())
            {
                if (!model.Lang.HasValue)
                {
                    model.Lang = 1;
                }
                
                dataModel.Sliders.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Slider");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Sliders.Find((object) id.Value));
            }
        }

        [HttpPost]
        public ActionResult Edit(Slider model, HttpPostedFileBase file)
        {
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
                    var str = Functions.SaveTempFile(file, "~/Recources/Slider");
                    model.Photo = str;
                }
                if (!model.Lang.HasValue)
                {
                    model.Lang = 1;
                }
                dataModel.Entry(model).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Slider");
            }
        }

        public ActionResult delete(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");
            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Sliders.Find((object) id.Value);
                dataModel.Sliders.Remove(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Slider");
            }
        }
    }
}