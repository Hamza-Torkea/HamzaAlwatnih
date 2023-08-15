using alwatnia.Helper;
using alwatnia.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class MediaController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Media.Where(w => w.Type.Equals("images")).OrderByDescending(o => o.Id).ToList());
            }
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Media.Find((object)id.Value);
                dataModel.Media.Remove(entity);
                dataModel.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult EditVideo(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var dataModel = new DataModel();
            var entity = dataModel.Media.Find(id.Value);
            return View(entity);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Edit(Medium model)
        {
            var dataModel = new DataModel();
            dataModel.Entry(model).State = EntityState.Modified;
            dataModel.SaveChanges();
            return getMessage(MStatus.success, "", "Videos", "Media");
        }

        public ActionResult Videos()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Media.Where(w => w.Type.Equals("video")).OrderBy(o => o.OrderNo).ThenBy(e => e.Id).ToList());
            }
        }

        public ActionResult GetMediaDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            using (var dataModel = new DataModel())
            {
                return View(dataModel.Media.Find((object)id.Value));
            }
        }

        //public ActionResult GetVideoDetails(int? id)
        //{
        //    if (!id.HasValue)
        //        return (ActionResult)this.RedirectToAction("Index");
        //    using (DataModel dataModel = new DataModel())
        //        return (ActionResult)this.View((object)dataModel.Media.Find((object)id.Value));
        //}

        [HttpPost]
        public void UploadFiless(HttpPostedFileBase[] file)
        {
            var stringList = Functions.SaveMultiFile(file, "~/Recources/Albums");
            using (var dataModel = new DataModel())
            {
                foreach (var str in stringList)
                {
                    var entity = new Medium
                    {
                        Type = "images",
                        Link = str,
                        cdate = DateTime.Now
                    };
                    dataModel.Media.Add(entity);
                }
                dataModel.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult Create(Medium model)
        {
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                var entity = new Medium
                {
                    Type = "video",
                    Link = model.Link,
                    cdate = DateTime.Now,
                    Title = model.Title,
                    ETitle = model.ETitle,
                    OrderNo = model.OrderNo
                };
                dataModel.Media.Add(entity);
                dataModel.SaveChanges();
                return RedirectToAction("videos");
            }
        }
    }
}