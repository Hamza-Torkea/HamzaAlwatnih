using alwatnia.Areas.Cpanel.Controllers;
using alwatnia.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace alwatnia.Areas.Unit.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Unit")]
    public class UnitPageController : Controller
    {
        public ActionResult Index(string name, string email, string mobile)
        {
            using (var dataModel = new DataModel())
            {
                var uid = User.Identity.GetUserName();
                var id = dataModel.Companies
                    .Include(t => t.RequestedProduct.Select(y => y.UsersProduct))
                    .FirstOrDefault(w => w.Email.Equals(uid));
                ViewBag.Company = id;

                var source = id?.RequestedProduct.Select(y => y.UsersProduct).DistinctBy(p => p.Id)
                    .ToList();

                if (!string.IsNullOrEmpty(name))
                {
                    source = source?.Where(w => w.fullname.Contains(name)).ToList();
                }

                if (!string.IsNullOrEmpty(email))
                {
                    source = source?.Where(w => w.email.Contains(email)).ToList();
                }

                if (!string.IsNullOrEmpty(mobile))
                {
                    source = source?.Where(w => w.mobile.Contains(mobile)).ToList();
                }

                source = source?.OrderByDescending(o => o.Id).ToList();

                return View(source);

            }

        }

        public ActionResult RemoveProduct(int id)
        {
            using (var dataModel = new DataModel())
            {
                var usersProducts = dataModel.UsersProducts
                    .Include(e => e.RequestedProduct)
                    .Single(e => e.Id == id);

                dataModel.RequestedProduct.RemoveRange(usersProducts.RequestedProduct);
                dataModel.UsersProducts.Remove(usersProducts);
                dataModel.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Contact()
        {
            using (var dataModel = new DataModel())
            {
                var uid = User.Identity.GetUserName();
                var id = dataModel.Companies
                    .FirstOrDefault(w => w.Email.Equals(uid));

                ViewBag.Company = id;

                return View(dataModel.Contacts
                    .Where(t => t.CompanyId == id.Id)
                    .OrderByDescending(o => o.Id).ToList());
            }
        }

        public ActionResult RemoveContact(int id)
        {
            using (var dataModel = new DataModel())
            {
                var contact = dataModel.Contacts
                    .Single(e => e.Id == id);
                dataModel.Contacts.Remove(contact);
                dataModel.SaveChanges();

                return RedirectToAction(nameof(Contact));
            }
        }

        public ActionResult Details(int? id)
        {
            var dataModel = new DataModel();
            var uid = User.Identity.GetUserName();
            var cid = dataModel.Companies
                .Include(t => t.RequestedProduct.Select(y => y.UsersProduct))
                .FirstOrDefault(w => w.Email.Equals(uid));

            return View(cid?.RequestedProduct.Where(t => t.UserProductId == id).ToList());
        }

        public ActionResult RemoveRequested(int id)
        {
            using (var dataModel = new DataModel())
            {
                var requestedProducts = dataModel.RequestedProduct
                    .Single(e => e.Id == id);
                dataModel.RequestedProduct.Remove(requestedProducts);
                dataModel.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
        }


        [System.Web.Http.HttpPost]
        public ActionResult done(int? id, int? status)
        {
            var dataModel = new DataModel();
            var requestedProduct = dataModel.RequestedProduct.SingleOrDefault(t => t.Id == id);
            if (requestedProduct != null)
            {
                requestedProduct.status = status;
                dataModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult getMessage(Enum status, string meesage, string action, string controller, int? parameter = null)
        {
            TempData["Message"] = string.IsNullOrEmpty(meesage) ? "تمت العملية بنجاح" : meesage;
            TempData["Status"] = status;
            return !parameter.HasValue ? RedirectToAction(action, controller)
                : RedirectToAction(action, controller, new { id = parameter });
        }

        public ActionResult Read(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Contacts.Find(id);
                entity.IsRead = true;
                dataModel.Entry(entity).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(CPBaseController.MStatus.success, "", "Contact", "UnitPage");
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult UserRequestDone(int? id, int? status)
        {
            var dataModel = new DataModel();
            var userProduct = dataModel.UsersProducts.SingleOrDefault(t => t.Id == id);
            if (userProduct != null)
            {
                userProduct.status = status;
                dataModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}