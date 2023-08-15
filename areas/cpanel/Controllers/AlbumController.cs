using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using alwatnia.Helper;
using alwatnia.Models;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class AlbumController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];

                var list = dataModel.Albums.OrderByDescending(x => x.Id).ToList();
                if (!string.IsNullOrEmpty(titleName))
                    list = list.Where(w => w.ArabicTitle.Contains(titleName)).ToList();
                return View(list);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetAlbumDetails(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Images.OrderByDescending(x => x.Id)
                    .FirstOrDefault(t => t.Id == id));
            }
        }

        public ActionResult Delete(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var images = dataModel.Images;
                Expression<Func<Image, bool>> predicate = w => w.AlbumId == id;
                foreach (var image in images.Where(predicate))
                    dataModel.Images.Remove(image);
                var album = dataModel.Albums.Find((object) id);
                dataModel.Albums.Remove(album);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Album");
            }
        }
        public ActionResult DeleteImage(int? id, int? albumId)
        {
            using (var dataModel = new DataModel())
            {
                var image = dataModel.Images.Find(id);
                dataModel.Images.Remove(image);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Uploading", "Album", albumId);
            }
        }

        [HttpPost]
        public ActionResult Create(Album model)
        {
            using (var dataModel = new DataModel())
            {
                model.cdate = DateTime.UtcNow;
                var album = dataModel.Albums.Add(model);
                dataModel.SaveChanges();
                return RedirectToAction("Uploading", new
                {
                    album.Id
                });
            }
        }

        public ActionResult Edit(int? id)
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Albums.Find((object) id));
            }
        }

        [HttpPost]
        public ActionResult Edit(Album model)
        {
            using (var dataModel = new DataModel())
            {
                dataModel.Entry(model).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Album");
            }
        }

        public ActionResult Uploading(int? id)
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Albums.Include(t => t.Image)
                    .FirstOrDefault(t => t.Id == id));
            }
        }

        [HttpPost]
        public ActionResult Uploading(HttpPostedFileBase[] file, int? id)
        {
            var stringList = Functions.SaveMultiFile(file);
            using (var dataModel = new DataModel())
            {
                foreach (var str in stringList)
                {
                    var image = new Image
                    {
                        photo = str,
                        AlbumId = id
                    };
                    dataModel.Images.Add(image);
                }
                dataModel.SaveChanges();
            }
            return getMessage(MStatus.success, "", "Index", "Album");
        }
    }
}