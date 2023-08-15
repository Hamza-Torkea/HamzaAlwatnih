using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using alwatnia.Helper;
using alwatnia.Models;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class PagesController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var companyId = Request.QueryString["companyId"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.Pages
                    .OrderByDescending(o => o.Id).ToList();
                if (!string.IsNullOrEmpty(titleName))
                    list = list.Where(w => w.title.Contains(titleName)).OrderByDescending(o => o.Id).ToList();

                if (!string.IsNullOrEmpty(companyId))
                    list = list.Where(w => w.company_id == int.Parse(companyId)).OrderByDescending(o => o.Id).ToList();

                if (!string.IsNullOrEmpty(langId))
                    list = list.Where(w => w.lang == int.Parse(langId)).OrderByDescending(o => o.Id).ToList();

                return View(list);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Uploading(int? id)
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Pages.Include(t => t.Image)
                    .FirstOrDefault(t => t.Id == id));
            }
        }

        public ActionResult DeleteImage(int? id, int? pageId)
        {
            using (var dataModel = new DataModel())
            {
                var image = dataModel.Images.Find(id);
                dataModel.Images.Remove(image);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Uploading", "Pages", pageId);
            }
        }

        public ActionResult GetAlbumDetails(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Images
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
                        PageId = id
                    };
                    dataModel.Images.Add(image);
                }
                dataModel.SaveChanges();
            }
            return getMessage(MStatus.success, "", "Index", "Pages");
        }

        [HttpPost]
        public ActionResult Create(Page model, HttpPostedFileBase file)
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
                    var str = Functions.SaveTempFile(file, "~/Recources/Uploads");
                    model.photo = str;
                }
                if (!model.lang.HasValue)
                {
                    model.lang = 1;
                }
                dataModel.Pages.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Pages");
            }
        }

        public ActionResult Edit(int? id)
        {
            using (var dataModel = new DataModel())
            {
                if (!id.HasValue)
                    return RedirectToAction("Index");
                return View(dataModel.Pages.Find((object) id.Value));
            }
        }

        [HttpPost]
        public ActionResult Edit(Page model, HttpPostedFileBase file)
        {

            string imagefile = "";

            using (var dataModel = new DataModel())
            {

                var list = dataModel.Pages.Where(w => w.Id == (model.Id)).ToList();

                foreach (var image in list)
                    imagefile = image.photo;

            }

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
                    var str = Functions.SaveTempFile(file, "~/Recources/Uploads");
                    model.photo = str;
                }
                else
                {

                    model.photo = imagefile; 


                }
                if (!model.lang.HasValue)
                {
                    model.lang = 1;
                }
                dataModel.Entry(model).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Pages");
            }
        }
    }
}