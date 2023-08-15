using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using alwatnia.Models;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class ConfigController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Configrations.ToList());
            }
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            using (var dataModel = new DataModel())
            {
                foreach (var allKey in form.AllKeys)
                {
                    var key = allKey;
                    var configration1 = dataModel.Configrations.FirstOrDefault(w => w.Config_name.Equals(key));
                    if (configration1 != null)
                    {
                        configration1.Config_details = Request.Form[key];
                        dataModel.Entry(configration1).State = (EntityState) 16;
                    }
                    else
                    {
                        var configration2 = new Configration
                        {
                            Config_name = key,
                            Config_details = Request.Form[key]
                        };
                        dataModel.Configrations.Add(configration2);
                    }
                }
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", nameof(Index), "Config");
            }
        }
    }
}