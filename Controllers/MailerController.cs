using ActionMailer.Net.Mvc;

namespace alwatnia.Controllers
{
    public class MailerController : MailerBase
    {
        public MailerController()
            : base(null, null)
        {
            From = "wfirec@wataniaind.com";
        }
    }
}