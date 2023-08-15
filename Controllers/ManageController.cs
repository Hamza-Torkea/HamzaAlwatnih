using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using alwatnia.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace alwatnia.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private const string XsrfKey = "XsrfId";
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            string str;
            if ((message.GetValueOrDefault() != ManageMessageId.ChangePasswordSuccess
                    ? 0
                    : (message.HasValue ? 1 : 0)) == 0)
                if ((message.GetValueOrDefault() != ManageMessageId.SetPasswordSuccess
                        ? 0
                        : (message.HasValue ? 1 : 0)) == 0)
                    if ((message.GetValueOrDefault() != ManageMessageId.SetTwoFactorSuccess
                            ? 0
                            : (message.HasValue ? 1 : 0)) == 0)
                        if ((message.GetValueOrDefault() != ManageMessageId.Error ? 0 : (message.HasValue ? 1 : 0)) ==
                            0)
                            if ((message.GetValueOrDefault() != ManageMessageId.AddPhoneSuccess
                                    ? 0
                                    : (message.HasValue ? 1 : 0)) == 0)
                                str = (message.GetValueOrDefault() != ManageMessageId.RemovePhoneSuccess
                                          ? 0
                                          : (message.HasValue ? 1 : 0)) != 0
                                    ? "Your phone number was removed."
                                    : "";
                            else
                                str = "Your phone number was added.";
                        else
                            str = "An error has occurred.";
                    else
                        str = "Your two-factor authentication provider has been set.";
                else
                    str = "Your password has been set.";
            else
                str = "Your password has been changed.";
            ViewBag.StatusMessage = str;

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = (await UserManager.GetTwoFactorEnabledAsync(userId) ? 1 : 0) != 0,
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = (await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId) ? 1 : 0) != 0
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(loginProvider, providerKey));
            ManageMessageId? message;
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                    await SignInManager.SignInAsync(user, false, false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new
            {
                Message = message
            });
        }

        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var identityMessage = new IdentityMessage();
                identityMessage.Destination = model.Number;
                identityMessage.Body = "Your security code is: " + code;
                var message = identityMessage;
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new
            {
                PhoneNumber = model.Number
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            var identityResult = await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
                await SignInManager.SignInAsync(user, false, false);
            return RedirectToAction("Index", "Manage");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            var identityResult = await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
                await SignInManager.SignInAsync(user, false, false);
            return RedirectToAction("Index", "Manage");
        }

        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            ViewResult viewResult;
            if (phoneNumber != null)
                viewResult = View(new VerifyPhoneNumberViewModel
                {
                    PhoneNumber = phoneNumber
                });
            else
                viewResult = View("Error");
            return viewResult;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result =
                await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                    await SignInManager.SignInAsync(user, false, false);
                return RedirectToAction("Index", new
                {
                    Message = ManageMessageId.AddPhoneSuccess
                });
            }
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
                return RedirectToAction("Index", new
                {
                    Message = ManageMessageId.Error
                });
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
                await SignInManager.SignInAsync(user, false, false);
            return RedirectToAction("Index", new
            {
                Message = ManageMessageId.RemovePhoneSuccess
            });
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result =
                await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                    await SignInManager.SignInAsync(user, false, false);
                return RedirectToAction("Index", new
                {
                    Message = ManageMessageId.ChangePasswordSuccess
                });
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                        await SignInManager.SignInAsync(user, false, false);
                    return RedirectToAction("Index", new
                    {
                        Message = ManageMessageId.SetPasswordSuccess
                    });
                }
                AddErrors(result);
            }
            return View(model);
        }

        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            string str;
            if ((message.GetValueOrDefault() != ManageMessageId.RemoveLoginSuccess ? 0 : (message.HasValue ? 1 : 0)) ==
                0)
                str = (message.GetValueOrDefault() != ManageMessageId.Error ? 0 : (message.HasValue ? 1 : 0)) != 0
                    ? "An error has occurred."
                    : "";
            else
                str = "The external login was removed.";

            ViewBag.StatusMessage = str;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
                return View("Error");


            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());

            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes()
                .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();

            ViewBag.ShowRemoveButton = user
                                           .PasswordHash != null || userLogins.Count > 1;

            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"),
                User.Identity.GetUserId());
        }

        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync("XsrfId", User.Identity.GetUserId());
            if (loginInfo == null)
                return RedirectToAction("ManageLogins", new
                {
                    Message = ManageMessageId.Error
                });
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded
                ? RedirectToAction("ManageLogins")
                : RedirectToAction("ManageLogins", new
                {
                    Message = ManageMessageId.Error
                });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        private bool HasPassword()
        {
            var byId = UserManager
                .FindById(User.Identity
                    .GetUserId());
            if (byId != null)
                return byId.PasswordHash != null;
            return false;
        }

        private bool HasPhoneNumber()
        {
            var byId = UserManager
                .FindById(User.Identity
                    .GetUserId());

            if (byId != null)
                return byId.PhoneNumber != null;
            return false;
        }
    }
}