using alwatnia.Helper;
using alwatnia.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class CompanyController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.Companies.OrderByDescending(o => o.Id).ToList();

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
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Company model, HttpPostedFileBase file, HttpPostedFileBase file2, string password)
        {
            var imgValidation = Functions.ValidateImage(file, true);
            var imgValidation2 = Functions.ValidateImage(file2, true);

            if (!imgValidation.Item1 || !imgValidation2.Item1)
            {
                TempData["Message"] = imgValidation.Item2;
                TempData["Status"] = "danger";
                return View(model);
            }
            var res = Functions.SaveTempFile(file, "~/Recources/Uploads");
            var res2 = Functions.SaveTempFile(file2, "~/Recources/Uploads");

            model.Photo = res;
            model.image2 = res2;

            model.Cdate = DateTime.Now;

            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var applicationDbContext = new ApplicationDbContext())
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var user = applicationUser;

                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDbContext));

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user.Id, "Unit");

                model.other_Id = user.Id;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Companies.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Company");
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
                return View(dataModel.Companies.Find(id));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Company model, HttpPostedFileBase file,
            HttpPostedFileBase file2, string newpassword)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Id == 6 || model.Id == 4)
            {
                model.ShowProductsInstedOfCategories = true;
            }
            if (file != null)
            {
                var tuple = Functions.ValidateImage(file, true);
                if (!tuple.Item1)
                {
                    TempData["Message"] = tuple.Item2;
                    TempData["Status"] = "danger";
                    return View(model);
                }
                model.Photo = Functions.SaveTempFile(file, "~/Recources/Uploads");
            }
            if (file2 != null)
            {
                var tuple2 = Functions.ValidateImage(file2, true);

                if (!tuple2.Item1)
                {
                    TempData["Message"] = tuple2.Item2;
                    TempData["Status"] = "danger";
                    return View(model);
                }
                model.image2 = Functions.SaveTempFile(file2, "~/Recources/Uploads");
            }

            using (var dataModel = new DataModel())
            {
                if (!model.Lang.HasValue)
                {
                    model.Lang = 1;
                }

                dataModel.SaveChanges();
            }

            using (var applicationDbContext = new ApplicationDbContext())
            {
                try
                {
                    var users = applicationDbContext.Users
                        .Where(w => w.Email.Equals(model.Email))
                        .ToList();
                    if (users.Any())
                    {
                        foreach (var staff in users)
                        {
                            applicationDbContext.Users.Remove(staff);
                        }

                        applicationDbContext.SaveChanges();
                    }
                    if (users.Any())
                    {
                        foreach (var exctive in users)
                        {
                            applicationDbContext.Users.Remove(exctive);
                        }

                        applicationDbContext.SaveChanges();
                    }
                    var applicationUser = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var userManager = new UserManager<ApplicationUser>(
                        new UserStore<ApplicationUser>(applicationDbContext)
                    );

                    await userManager.CreateAsync(applicationUser, newpassword);
                    applicationDbContext.SaveChanges();

                    await userManager.AddToRoleAsync(applicationUser.Id, "Unit");
                    model.other_Id = applicationUser.Id;
                    applicationDbContext.SaveChanges();

                }
                catch (Exception)
                {

                }
            }


            try
            {
                using (var dataModel = new DataModel())
                {
                    dataModel.Entry(model).State = EntityState.Modified;
                    dataModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return getMessage(MStatus.success, "", "Index", "Company");
        }

        public ActionResult Staff(string title, int? company, int? lang)
        {
            ViewBag.company = getCompanies();

            using (var dataModel = new DataModel())
            {
                var list = dataModel.Staffs
                    .Include(t => t.Company)
                    .OrderByDescending(t => t.Id)
                    .ToList();

                if (!string.IsNullOrEmpty(title))
                {
                    list = list.Where(w => w.Title.Contains(title)).ToList();
                }

                if (lang.HasValue)
                {
                    list = list.Where(w => w.Lang == lang).ToList();
                }

                if (company.HasValue)
                {
                    list = list.Where(w =>
                    {
                        int num;
                        if (w.company_id.HasValue)
                        {
                            var companyId = w.company_id;
                            var nullable = company;
                            num = companyId.GetValueOrDefault() != nullable.GetValueOrDefault()
                                ? 0
                                : (companyId.HasValue == nullable.HasValue ? 1 : 0);
                        }
                        else
                        {
                            num = 0;
                        }
                        return num != 0;
                    }).ToList();
                }

                return View(list.OrderByDescending(o => o.Id));
            }
        }

        public ActionResult addStaff()
        {
            ViewBag.company = getCompanies();
            return View();
        }

        [HttpPost]
        public ActionResult addStaff(alwatnia.Models.Staff model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();

            var tuple = Functions.ValidateImage(file, true);
            if (!tuple.Item1)
            {
                TempData["Message"] = tuple.Item2;
                TempData["Status"] = "danger";
                return View(model);
            }
            var str = Functions.SaveTempFile(file, "~/Recources/Staff");
            model.Photo = str;
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Staffs.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Staff", "Company");
            }
        }

        [HttpPost]
        public ActionResult EditStaff(alwatnia.Models.Staff model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();

            if (file != null)
            {
                var tuple = Functions.ValidateImage(file, true);
                if (!tuple.Item1)
                {
                    TempData["Message"] = tuple.Item2;
                    TempData["Status"] = "danger";
                    return View(model);
                }
                var str = Functions.SaveTempFile(file, "~/Recources/Staff");
                model.Photo = str;
            }
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Entry(model).State = (EntityState)16;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Staff", "Company");
            }
        }

        public ActionResult EditStaff(int? id)
        {
            ViewBag.company = getCompanies();

            using (var dataModel = new DataModel())
            {
                return View(dataModel.Staffs.Find(id));
            }
        }

        public ActionResult DeleteStaff(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var staff = dataModel.Staffs.Find(id);
                dataModel.Staffs.Remove(staff);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Staff", "Company");
            }
        }
        public ActionResult ExecutiveManagement(string title, int? company, int? lang)
        {
            ViewBag.company = getCompanies();

            using (var dataModel = new DataModel())
            {
                var list = dataModel.Exctives
                    .Include(t => t.Company)
                    .OrderByDescending(t => t.Id)
                    .ToList();

                if (!string.IsNullOrEmpty(title))
                {
                    list = list.Where(w => w.Title.Contains(title)).ToList();
                }

                if (lang.HasValue)
                {
                    list = list.Where(w => w.Lang == lang).ToList();
                }

                if (company.HasValue)
                {
                    list = list.Where(w =>
                    {
                        int num;
                        if (w.CompanyId.HasValue)
                        {
                            var companyId = w.CompanyId;
                            var nullable = company;
                            num = companyId.GetValueOrDefault() != nullable.GetValueOrDefault()
                                ? 0
                                : (companyId.HasValue == nullable.HasValue ? 1 : 0);
                        }
                        else
                        {
                            num = 0;
                        }
                        return num != 0;
                    }).ToList();
                }

                return View(list.OrderByDescending(o => o.Id));
            }
        }

        public ActionResult addExecutiveManagement()
        {
            ViewBag.company = getCompanies();
            return View();
        }

        [HttpPost]
        public ActionResult addExecutiveManagement(alwatnia.Models.Exctive model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();

            var tuple = Functions.ValidateImage(file, true);
            if (!tuple.Item1)
            {
                TempData["Message"] = tuple.Item2;
                TempData["Status"] = "danger";
                return View(model);
            }
            var str = Functions.SaveTempFile(file, "~/Recources/Staff");
            model.Photo = str;
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Exctives.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "ExecutiveManagement", "Company");
            }
        }

        [HttpPost]
        public ActionResult EditExecutiveManagement(alwatnia.Models.Exctive model, HttpPostedFileBase file)
        {
            ViewBag.company = getCompanies();

            if (file != null)
            {
                var tuple = Functions.ValidateImage(file, true);
                if (!tuple.Item1)
                {
                    TempData["Message"] = tuple.Item2;
                    TempData["Status"] = "danger";
                    return View(model);
                }
                var str = Functions.SaveTempFile(file, "~/Recources/Staff");
                model.Photo = str;
            }
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Entry(model).State = (EntityState)16;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "ExecutiveManagement", "Company");
            }
        }

        public ActionResult EditExecutiveManagement(int? id)
        {
            ViewBag.company = getCompanies();

            using (var dataModel = new DataModel())
            {
                return View(dataModel.Exctives.Find(id));
            }
        }

        public ActionResult DeleteExecutiveManagement(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var executive = dataModel.Exctives.Find(id);
                dataModel.Exctives.Remove(executive);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "ExecutiveManagement", "Company");
            }
        }

        public ActionResult Delete(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var queryable1 = dataModel.Staffs.Where(w => w.company_id == id);
                var queryable2 = dataModel.Products.Where(w => w.company_Id == id);
                //var queryable3 = dataModel.News.Where(w => w.company_Id == id);
                var queryable4 = dataModel.Images.Where(w => w.companyId == id);
                var queryable5 = dataModel.Exctives.Where(w => w.CompanyId == id);

                foreach (var staff in queryable1)
                {
                    dataModel.Staffs.Remove(staff);
                }

                foreach (var product in queryable2)
                {
                    var i = product;
                    var comments = dataModel.Comments;
                    Expression<Func<Comment, bool>> predicate = w => w.product_id == i.Id;
                    foreach (var comment in comments.Where(predicate))
                    {
                        dataModel.Comments.Remove(comment);
                    }

                    dataModel.Products.Remove(i);
                }
                //foreach (var news in queryable3)
                //dataModel.News.Remove(news);
                foreach (var image in queryable4)
                {
                    dataModel.Images.Remove(image);
                }
                foreach (var exctives in queryable5)
                {
                    dataModel.Exctives.Remove(exctives);
                }
                var company = dataModel.Companies.Find((object)id);
                dataModel.Companies.Remove(company);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Company");
            }
        }

        public ActionResult Uploading(int? id)
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Companies.Include(t => t.Image)
                    .FirstOrDefault(t => t.Id == id));
            }
        }

        public ActionResult DeleteImage(int? id, int? companyId)
        {
            using (var dataModel = new DataModel())
            {
                var image = dataModel.Images.Find(id);
                dataModel.Images.Remove(image);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Uploading", "Company", companyId);
            }
        }

        public ActionResult GetAlbumDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

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
                        companyId = id
                    };
                    dataModel.Images.Add(image);
                }
                dataModel.SaveChanges();
            }
            return getMessage(MStatus.success, "", "Index", "Company");
        }
    }
}