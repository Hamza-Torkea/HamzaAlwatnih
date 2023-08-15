using System.Collections.Generic;

namespace alwatnia.Areas.Cpanel.ViewModels
{
    public partial class NewsController
    {
        public class SendNewsViewModel
        {
            public int NewsId { get; set; }
            public List<string> Emails { get; set; }
            public bool SendToTwitter { get; set; } = false;
            public bool SendToFacebook { get; set; } = false;
        }

    }
}