using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace alwatnia
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return (Task)Task.FromResult<int>(0);
        }
    }
}
