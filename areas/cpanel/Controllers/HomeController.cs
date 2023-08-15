using alwatnia.Models;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class HomeController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                ViewBag.News = dataModel.News.Count();

                ViewBag.products = dataModel.Products.Count();
                ViewBag.Activity = dataModel.Activities.Count();
                ViewBag.Jobs = dataModel.Jobs.Count();
                ViewBag.comments = dataModel.Comments
                    .OrderByDescending(o => o.Id)
                    .Where(w => !w.job_id.HasValue).ToList();

                ViewBag.Job = dataModel.Comments.Include("Job")
                    .OrderByDescending(o => o.Id)
                    .Where(w => w.job_id.HasValue).ToList();

                return View();
            }
        }

        public ActionResult delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("index");
            }

            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Comments.Find((object)id.Value);
                dataModel.Comments.Remove(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Home");
            }
        }

        public ActionResult Contact()
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["name"];
                var langId = Request.QueryString["email"];

                var list = dataModel.Contacts
                    .Where(t => t.Type == ContactType.Main)
                   .OrderByDescending(o => o.Id).ToList();

                if (!string.IsNullOrEmpty(titleName))
                {
                    list = list.Where(w => w.Name.Contains(titleName)).ToList();
                }

                if (!string.IsNullOrEmpty(langId))
                {
                    list = list.Where(w => w.Email.Contains(langId)).ToList();
                }

                return View(list);

            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveContact(int id)
        {
            using (var dataModel = new DataModel())
            {
                var contact = await dataModel.Contacts
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (contact != null)
                {
                    dataModel.Contacts.Remove(contact);
                    await dataModel.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Contact));
            }
        }

        public ActionResult Mailing()
        {
            using (var dataModel = new DataModel())
            {
                ViewBag.cats = dataModel.MailCat.ToList();
                return View(dataModel.MailingLists
                    .OrderByDescending(o => o.Id)
                    .ToList());
            }
        }

        [HttpPost]
        public ActionResult addToCat(int[] cbEmail, string cat)
        {
            if (cbEmail == null)
            {
                ViewBag.message = "1";
                return getMessage(MStatus.danger, "الرجاء اختيار ايميل واحد على الاقل", "Mailing", "Home");
            }
            else
            {
                using (var dataModel = new DataModel())
                {
                    foreach (var num in cbEmail)
                    {
                        var entity = dataModel.MailingLists.Find((object)num);
                        entity.cat1 = cat;
                        dataModel.Entry(entity).State = EntityState.Modified;
                        dataModel.SaveChanges();
                    }
                    return getMessage(MStatus.success, "", "Mailing", "Home");
                }

            }

        }

        public ActionResult DeleteMailing(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var entity = dataModel.MailingLists.Find((object)id);
                dataModel.MailingLists.Remove(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Mailing", "Home");
            }
        }

        public ActionResult done(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var entity = dataModel.Contacts.Find(id);
                entity.IsRead = true;
                dataModel.Entry(entity).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Contact", "Home");
            }
        }

        public ActionResult addCat(string name)
        {
            using (var dataModel = new DataModel())
            {
                var entity = new MailCats
                {
                    name = name
                };
                dataModel.MailCat.Add(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Mailing", "Home");
            }
        }

        public ActionResult change_password()
        {
            return View();
        }

        public ActionResult addEmail(string name)
        {
            using (var dataModel = new DataModel())
            {
                var entity = new MailingList
                {
                    Email = name
                };
                dataModel.MailingLists.Add(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Mailing", "Home");
            }
        }

        public ActionResult Config()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Configrations.ToList());
            }
        }

        public async Task<ActionResult> GenerateAccessToken()
        {
            var scope = "manage_pages,publish_pages";
            using (var dataModel = new DataModel())
            {
                var appId = await dataModel.Configrations
                    .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_app_id"));
                var appSecret = await dataModel.Configrations
                    .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_app_secret"));
                if (appId == null || appSecret == null)
                {
                    return RedirectToAction(nameof(Config));
                }

                if (Request["code"] == null)
                {
                    // Response.Redirect();
                    return Redirect(
                        $"https://graph.facebook.com/oauth/authorize?client_id={appId.Config_details}&redirect_uri={Request.Url.AbsoluteUri}&scope={scope}");
                }

                string url =
                    $"https://graph.facebook.com/oauth/access_token?client_id={appId.Config_details}&redirect_uri={Request.Url.AbsoluteUri}&scope={scope}&code={Request["code"].ToString()}&client_secret={appSecret.Config_details}";

                var request = WebRequest.Create(url) as HttpWebRequest;

                using (var response = request.GetResponse() as HttpWebResponse)
                {

                    var reader = new StreamReader(response.GetResponseStream());
                    var vals = reader.ReadToEnd();
                    var jsonObj = JObject.Parse(vals);
                    var token = (string)jsonObj["access_token"];

                    var pageConfig = await dataModel.Configrations
                        .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_page_url"));
                    var pageId = await dataModel.Configrations
                        .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_page_url"));
                    if (pageConfig == null)
                    {
                        var getPageId =
                            $"https://graph.facebook.com/v2.6/?id={pageConfig.Config_details}&access_token={token}";
                        var req = WebRequest.Create(getPageId) as HttpWebRequest;
                        using (var res = req.GetResponse() as HttpWebResponse)
                        {
                            reader = new StreamReader(res.GetResponseStream());
                            vals = reader.ReadToEnd();
                            jsonObj = JObject.Parse(vals);
                            var id = (string)jsonObj["id"];
                            pageId = new Configration
                            {
                                Config_name = "facebook_page_id",
                                Config_details = id
                            };
                            dataModel.Configrations.Add(pageId);
                        }
                    }

                    var client = new FacebookClient(token);
                    var pageToken = client.Get($"/{pageId?.Config_details}?fields=access_token&access_token={token}");

                    var fbAccessToken = new Configration
                    {
                        Config_name = "facebook_access_token",
                        Config_details = (string)((JsonObject)pageToken)["access_token"]
                    };

                    dataModel.Configrations.Add(fbAccessToken);
                    await dataModel.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Config));
            }
        }

        [HttpPost]
        public ActionResult Config(FormCollection form)
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
                        dataModel.Entry(configration1).State = (EntityState)16;
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
                return getMessage(MStatus.success, "", "Config", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> change_password(ChangePasswordViewModel model)
        {
            using (var applicationDbContext = new ApplicationDbContext())
            {
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDbContext));

                var user = await userManager.FindByIdAsync(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var isPasswordValid = await userManager.CheckPasswordAsync(user, model.OldPassword);

                if (!isPasswordValid)
                {
                    return getMessage(MStatus.danger, "كلمة المرور الحالية غير صحيحة", "change_password", "Home");
                }

                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    return getMessage(MStatus.danger, "كلمة المرور الجديد غير مطابقة", "change_password", "Home");
                }

                var changed = await userManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);

                return changed.Succeeded
                    ? getMessage(MStatus.success, "تمت العملية بنجاح", "change_password", "Home")
                    : getMessage(MStatus.danger, "حصل خطأ في تغيير كلمة المرور", "change_password", "Home");
            }
        }
    }
}