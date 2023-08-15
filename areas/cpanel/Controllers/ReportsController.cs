using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using alwatnia.Models;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class ReportsController : CPBaseController
    {
        public ActionResult Index(string title, int? company, int? lang)
        {
            ViewBag.company = getCompanies();

            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var companyId = Request.QueryString["companyId"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.Report
                    .Include(t => t.Company)
                    .OrderByDescending(t => t.Id).ToList();

                if (!string.IsNullOrEmpty(titleName))
                    list = list.Where(w => w.Title.Contains(titleName)).ToList();

                if (!string.IsNullOrEmpty(companyId))
                    list = list.Where(w => int.Parse(companyId) == w.company_id).ToList();

                if (!string.IsNullOrEmpty(langId))
                    list = list.Where(w => int.Parse(langId) == w.Lang).ToList();

                return View(list);
            }
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.company = getCompanies();
            using (var dataModel = new DataModel())
            {
                if (!id.HasValue)
                    return RedirectToAction("index");
                return View(dataModel.Report.Find(id.Value));
            }
        }

        [HttpPost]
        public ActionResult Edit(Reports model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();
            if (file != null)
            {
                var str = Guid.NewGuid().ToString().Substring(0, 20) + new FileInfo(file.FileName).Extension;
                file.SaveAs(Server.MapPath("~/Recources/Uploads/" + str));
                model.link = str;
            }
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Entry(model).State = EntityState.Modified;

                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Reports");
            }
        }

        public ActionResult Create()
        {
            ViewBag.company = getCompanies();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Reports model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();

            if (file == null)
                return View(model);
            var str = Guid.NewGuid().ToString().Substring(0, 20) + new FileInfo(file.FileName).Extension;
            file.SaveAs(Server.MapPath("~/Recources/Uploads/" + str));
            model.link = str;
            model.cdate = DateTime.Now;
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Report.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Reports");
            }
        }

        public ActionResult delete(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");
            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Report.Find((object)id.Value);
                dataModel.Report.Remove(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Products");
            }
        }
    }
}