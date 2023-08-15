using alwatnia.Areas.Cpanel.Controllers;
using alwatnia.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace alwatnia.areas.cpanel.Controllers
{
    public class BranchesController : CPBaseController
    {
        public ActionResult Index(int companyId)
        {
            using (var context = new DataModel())
            {
                ViewBag.CompanyId = companyId;
                return View(context.Branches.Where(e => e.CompanyId == companyId).ToList());
            }
        }

        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Branch branch)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(branch);

                using (var context = new DataModel())
                {
                    context.Branches.Add(branch);
                    context.SaveChanges();

                    return RedirectToAction("Index", new { companyId = branch.CompanyId });
                }
            }
            catch
            {
                return View(branch);
            }
        }

        public ActionResult Edit(int id)
        {
            using (var context = new DataModel())
            {
                var branch = context.Branches.SingleOrDefault(s => s.Id == id);
                if (branch == null)
                    return HttpNotFound();

                return View(branch);
            }
        }

        [HttpPost]
        public ActionResult Edit(Branch branch)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(branch);

                using (var context = new DataModel())
                {
                    context.Entry(branch).State = EntityState.Modified;
                    context.SaveChanges();

                    return RedirectToAction("Index", new { companyId = branch.CompanyId });
                }
            }
            catch
            {
                return View(branch);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var context = new DataModel())
                {
                    var branch = context.Branches.SingleOrDefault(s => s.Id == id);
                    if (branch == null)
                        return HttpNotFound();

                    var companyId = branch.CompanyId;

                    context.Branches.Remove(branch);
                    context.SaveChanges();

                    return RedirectToAction("Index", new { companyId = companyId });
                }
            }
            catch
            {
                return HttpNotFound();
            }
        }
    }
}
