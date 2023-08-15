using System;
using System.Security.Claims;
using System.Threading.Tasks;
using alwatnia.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace alwatnia
{
    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext(
                new Func<IdentityFactoryOptions<ApplicationUserManager>, IOwinContext, ApplicationUserManager>(
                    ApplicationUserManager.Create));
            app.CreatePerOwinContext(
                new Func<IdentityFactoryOptions<ApplicationSignInManager>, IOwinContext, ApplicationSignInManager>(
                    ApplicationSignInManager.Create));
            var app1 = app;
            var authenticationOptions = new CookieAuthenticationOptions();
            authenticationOptions.AuthenticationType = "ApplicationCookie";
            authenticationOptions.LoginPath = new PathString("/Account/Login");
            authenticationOptions.Provider = new CookieAuthenticationProvider
            {
                OnValidateIdentity = SecurityStampValidator.OnValidateIdentity(TimeSpan.FromMinutes(30.0),
                    (Func<ApplicationUserManager, ApplicationUser, Task<ClaimsIdentity>>) ((manager, user) =>
                        user.GenerateUserIdentityAsync(manager)))
            };
            var options = authenticationOptions;
            app1.UseCookieAuthentication(options);
            app.UseExternalSignInCookie("ExternalCookie");
            app.UseTwoFactorSignInCookie("TwoFactorCookie", TimeSpan.FromMinutes(5.0));
            app.UseTwoFactorRememberBrowserCookie("TwoFactorRememberBrowser");
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}