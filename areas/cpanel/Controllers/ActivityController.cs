using alwatnia.Helper;
using alwatnia.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class ActivityController : CPBaseController
    {
        public ActionResult Index()
        {
            var langCookie = new HttpCookie(
                    "lang",
                    "en"
                )
            { HttpOnly = true };
            Response.AppendCookie(langCookie);

            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.Activities
                    .Include(t => t.Comments)
                    .OrderByDescending(o => o.Id).ToList();

                if (!string.IsNullOrEmpty(titleName))
                {
                    list = list.Where(w => w.Title.Contains(titleName)).ToList();
                }

                if (!string.IsNullOrEmpty(langId))
                {
                    list = list.Where(w => w.Lang == int.Parse(langId)).ToList();
                }

                return View(list);
            }
        }

        public ActionResult Create()
        {
            var langCookie = new HttpCookie(
                    "lang",
                    "en"
                )
            { HttpOnly = true };
            Response.AppendCookie(langCookie);

            return View();
        }

        [HttpPost]
        public ActionResult Create(Activity model, HttpPostedFileBase file)
        {
            var tuple = Functions.ValidateImage(file, true);
            if (!tuple.Item1)
            {
                TempData["Message"] = tuple.Item2;
                TempData["Status"] = "danger";
                return View(model);
            }
            var str = Functions.SaveTempFile(file, "~/Recources/Activities");
            model.Photo = str;
            model.Cdate = DateTime.Now;
            //  model.StartDate = DateTime.UtcNow;

            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {

                //  model.Cdate = DateTime.UtcNow;


                var entity = new Activity
                {
                    Title = model.Title,
                    StartDate = model.StartDate,
                    Details = model.Details,
                    Cdate = DateTime.UtcNow,
                    Photo = str,
                    Lang = model.Lang
                };


                dataModel.Activities.Add(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Activity");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("index");
            }

            using (var dataModel = new DataModel())
            {
                return View(dataModel.Activities.Find((object)id.Value));
            }
        }

        [HttpPost]
        public ActionResult Edit(Activity model, HttpPostedFileBase file)
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
                    var str = Functions.SaveTempFile(file, "~/Recources/Activities");
                    model.Photo = str;
                }
                if (!model.Lang.HasValue)
                {
                    model.Lang = 1;
                }



                dataModel.Entry(model).State = EntityState.Modified;

                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Activity");
            }
        }

        public ActionResult delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("index");
            }

            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Activities.Find((object)id.Value);
                dataModel.Activities.Remove(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Activity");
            }
        }

        public ActionResult Comments(int? id, string comment_auther, string title)
        {
            using (var dataModel = new DataModel())
            {
                var list = dataModel.Comments.Where(w => w.act_id.HasValue).Include(w => w.Activity).ToList();
                if (id.HasValue)
                {
                    list = list.Where(w =>
                    {
                        var actId = w.act_id;
                        var nullable = id;
                        return actId.GetValueOrDefault() == nullable.GetValueOrDefault() &&
                               actId.HasValue == nullable.HasValue;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(comment_auther))
                {
                    list = list.Where(w => w.comment_auther.Contains(comment_auther)).ToList();
                }

                if (!string.IsNullOrEmpty(title))
                {
                    list = list.Where(w => w.Activity.Title.Contains(title)).ToList();
                }

                return View(list);
            }
        }
    }
}