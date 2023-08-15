using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace alwatnia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, "ApplicationCookie");
            return userIdentity;
        }
    }
}
