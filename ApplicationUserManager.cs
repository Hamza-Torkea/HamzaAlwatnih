using System;
using alwatnia.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace alwatnia
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var applicationUserManager1 =
                new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            var applicationUserManager2 = applicationUserManager1;
            var userValidator1 = new UserValidator<ApplicationUser>(applicationUserManager1);

            userValidator1.AllowOnlyAlphanumericUserNames = false;
            userValidator1.RequireUniqueEmail = true;
            var userValidator2 = userValidator1;
            applicationUserManager2.UserValidator = userValidator2;
            applicationUserManager1.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 2,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            applicationUserManager1.UserLockoutEnabledByDefault = true;
            applicationUserManager1.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5.0);
            applicationUserManager1.MaxFailedAccessAttemptsBeforeLockout = 5;
            var applicationUserManager3 = applicationUserManager1;
            var twoFactorProvider1 = "Phone Code";

            var numberTokenProvider1 = new PhoneNumberTokenProvider<ApplicationUser>();
            numberTokenProvider1.MessageFormat = "Your security code is {0}";
            var numberTokenProvider2 = numberTokenProvider1;
            applicationUserManager3.RegisterTwoFactorProvider(twoFactorProvider1, numberTokenProvider2);
            var applicationUserManager4 = applicationUserManager1;
            var twoFactorProvider2 = "Email Code";
            var emailTokenProvider1 = new EmailTokenProvider<ApplicationUser>();
            emailTokenProvider1.Subject = "Security Code";
            emailTokenProvider1.BodyFormat = "Your security code is {0}";
            var emailTokenProvider2 = emailTokenProvider1;
            applicationUserManager4.RegisterTwoFactorProvider(twoFactorProvider2, emailTokenProvider2);
            applicationUserManager1.EmailService = new EmailService();
            applicationUserManager1.SmsService = new SmsService();
            var protectionProvider = options.DataProtectionProvider;
            if (protectionProvider != null)
                applicationUserManager1.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(protectionProvider.Create("ASP.NET Identity"));
            return applicationUserManager1;
        }
    }
}