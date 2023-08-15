using alwatnia.Helper;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace alwatnia.Controllers
{
    public class ThumbController : Controller
    {
        public ActionResult Index(string type, string name, int width, int height)
        {
            // return null;
            var path = "~/Recources/Uploads/default.jpg";
            if (!string.IsNullOrEmpty(name))
            {
                path = "~/Recources/" + type + "/" + name;
                if (!System.IO.File.Exists(Server.MapPath(path)))
                {
                    path = "~/Recources/Uploads/default.jpg";
                }
            }
            return this.Image(ImageHelper.GetCroppedImage(
                System.IO.File.ReadAllBytes(Server.MapPath(path)),
                new Size(width, height), 0, 0, ImageFormat.Jpeg, null), "image/jpeg");
        }
    }
}