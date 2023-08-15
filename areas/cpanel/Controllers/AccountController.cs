using alwatnia.areas.cpanel.Models;
using Microsoft.AspNet.Identity.Owin;
using Resources;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace alwatnia.areas.cpanel.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", Resource.ChangePasswordError);
                return View(model);
            }

            TempData["Message"] = "تم تغيير كلمة المرور بنجاح";

            return RedirectToAction(nameof(ChangePassword));
        }
    }
}