using alwatnia.Areas.Cpanel.Services;
using alwatnia.Helper;
using alwatnia.Models;
using reCAPTCHA.MVC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace alwatnia.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult ChangeCulture(string lang)
        {
            var langCookie = new HttpCookie("lang", lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);
            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("Products/"))
                {
                    return Redirect("~/");
                }

                if (Request.UrlReferrer.LocalPath.Contains("News/"))
                {
                    return RedirectToAction("News", "Home");
                }

                if (Request.UrlReferrer.LocalPath.Contains("Activities/"))
                {
                    return RedirectToAction("Activities", "Home");
                }

                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }

            return Redirect("~/");
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Index()
        {
            var lang = Functions.GetLanguage();

            using (var dataModel = new DataModel())
            {
                // ISSUE: reference to a compiler-generated field
                ViewBag.Slider = dataModel.Sliders.Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(5).ToList();

                var newsNumConfig = dataModel.Configrations
                    .FirstOrDefault(w => w.Config_name.Equals("news_number"));
                var newsNum = newsNumConfig != null ? int.Parse(newsNumConfig.Config_details) : 20;
                ViewBag.News = dataModel.News.Where(w => w.Lang == lang && !w.type.HasValue)
                    .OrderByDescending(o => o.Id)
                    .Take(newsNum)
                    .ToList();

                ViewBag.News2 = dataModel.News.Where(w => w.Lang == lang && !w.type.HasValue)
                    .OrderByDescending(o => o.Id)
                    .Take(5)
                    .ToList();

                ViewBag.video =
                    dataModel.Pages
                        .FirstOrDefault(w => w.tag.Equals("about") && w.lang == lang)
                        ?.video;

                ViewBag.Acts = dataModel.Activities
                    .Where(w => w.Lang == lang)
                    .OrderByDescending(o => o.Id)
                    .Take(3).ToList();
                var cc = dataModel.Companies.Include(e => e.Branches).ToList();
                ViewBag.Company = cc;
                ViewBag.CompanyWithoutMain = cc.Where(t => t.Id != 1).ToList();
                ViewBag.MainCompany = cc.FirstOrDefault(t => t.Id == 1);

                ViewBag.Pages = dataModel.Pages.Where(w => w.lang == lang && (
                                                               w.tag.Equals("home_about") ||
                                                               w.tag.Equals("home_vision") ||
                                                               w.tag.Equals("home_target") ||
                                                               w.tag.Equals("home_jobs")))
                    .ToList();

                ViewBag.exh = dataModel.Albums
                    .Where(w => w.type.HasValue && w.type.Value == 3)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                ViewBag.Reports = dataModel.Report
                    .Where(w => w.home.HasValue && w.home.Value && w.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(4)
                    .ToList();

                return View();
            }
        }

        public ActionResult Rajhi()
        {
            var lang = Functions.GetLanguage();
            var data = new Page();
            using (var dataModel = new DataModel())
            {
                data = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("managment"));

                ViewBag.photo = data?.photo;


                ViewBag.managment = data?.details;

                return View();
            }
        }

        public ActionResult features()
        {
            var lang = Functions.GetLanguage();

            using (var dataModel = new DataModel())
            {
                var feature = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals(nameof(features)));
                ViewBag.photo = feature?.photo;
                ViewBag.features = feature?.details;
                return View();
            }
        }

        public ActionResult JobContact()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.LeftNews = dataModel.News
                    .Where(t => t.Lang == lang &&
                                !t.type.HasValue)
                    .OrderByDescending(o => o.Id).Take(3)
                    .ToList();

                ViewBag.Products = dataModel.Products
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                ViewBag.Acts = dataModel.Activities
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                return View();
            }
        }

        public ActionResult FillApplication()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var feature = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("features"));

                ViewBag.video = feature?.video;
                ViewBag.features = feature?.details;

                ViewBag.App = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("jobapplication"))?.details;

                return View();
            }
        }

        public ActionResult traning()
        {
            var lang = Functions.GetLanguage();

            using (var dataModel = new DataModel())
            {
                var training = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals(nameof(traning)));
                ViewBag.video = training
                    ?.video;

                ViewBag.photo = training
                    ?.photo;

                ViewBag.traning = training
                    ?.details;

                return View();
            }
        }

        public ActionResult Career(int? id)
        {
            var lang = Functions.GetLanguage();

            using (var dataModel = new DataModel())
            {
                ViewBag.LeftNews = dataModel.News
                    .Where(t => t.Lang == lang &&
                                !t.type.HasValue)
                    .OrderByDescending(o => o.Id).Take(3)
                    .ToList();

                ViewBag.Products = dataModel.Products
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                ViewBag.Acts = dataModel.Activities
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                if (id.HasValue)
                {
                    return View("request_career", dataModel.Jobs.Find((object)id));
                }

                return View(dataModel.Jobs.Where(t => t.Lang == lang && t.IsOpen == true).OrderByDescending(o => o.Id).ToList());
            }
        }

        public ActionResult News(int? id, int? page, NewsType? type = null, string keyword = null, string culture = null)
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                if (id.HasValue)
                {
                    var singleNews = dataModel.News.Find(id.Value);
                    if (singleNews?.type == NewsType.Social)
                    {
                        ViewBag.AnotherNews = dataModel.News
                            .Where(t => t.Lang == lang && t.type == NewsType.Social && t.Id != id)
                            .OrderByDescending(o => o.Id).Take(3).ToList();
                    }
                    else
                    {
                        ViewBag.AnotherNews = dataModel.News
                            .Where(t => t.Lang == lang && !t.type.HasValue && t.Id != id)
                            .OrderByDescending(o => o.Id).Take(3).ToList();
                    }

                    if (!singleNews.ReadCounter.HasValue)
                    {
                        singleNews.ReadCounter = 1;
                        dataModel.SaveChanges();
                    }
                    else
                    {
                        singleNews.ReadCounter += 1;
                        dataModel.SaveChanges();
                    }

                    ViewBag.LeftNews = dataModel.News.Where(w => w.Id != id && !w.type.HasValue)
                        .Where(t => t.Lang == lang)
                        .OrderByDescending(o => o.Id).Skip(3).Take(2)
                        .ToList();

                    ViewBag.Products = dataModel.Products
                        .Where(t => t.Lang == lang)
                        .OrderByDescending(o => o.Id).Take(2)
                        .ToList();

                    ViewBag.Acts = dataModel.Activities
                        .Where(t => t.Lang == lang)
                        .OrderByDescending(o => o.Id).Take(2)
                        .ToList();

                    ViewBag.Next = dataModel.News.Where(t => t.Lang == lang && !t.type.HasValue)
                                       .FirstOrDefault(w => w.Id > id)?.Id ?? 0;
                    ViewBag.prev = dataModel.News.Where(t => t.Lang == lang && !t.type.HasValue)
                                       .FirstOrDefault(w => w.Id < id)?.Id ?? 0;

                    if (!string.IsNullOrWhiteSpace(culture) ||
                        singleNews.Lang != Functions.GetLanguage())
                    {
                        var langCookie = new HttpCookie(
                            "lang",
                            singleNews.Lang == (int)Languages.English ? "en" : "ar"
                            )
                        { HttpOnly = true };
                        Response.AppendCookie(langCookie);

                        return RedirectToAction(nameof(News), new { id });
                    }

                    return View("new", singleNews);
                }

                var data = dataModel.News
                    .Where(t => t.Lang == lang);

                if (!string.IsNullOrEmpty(keyword))
                {
                    data = data.Where(t => t.Title.Contains(keyword) || t.Details.Contains(keyword));
                }

                data = type.HasValue ? data.Where(t => t.type == NewsType.Social) : data.Where(t => !t.type.HasValue);

                var result = data.OrderByDescending(o => o.Id)
                    .Paginate(page);

                return View(result);
            }
        }

        public ActionResult Category(int id, int? page)
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var category = dataModel.ProductCats
                    .Include(e => e.Company)

                    .SingleOrDefault(s => s.Id == id);
                if (category == null)
                {
                    return NotFound();
                }

                ViewBag.products = dataModel.Products.Where(t => t.Lang == lang && t.ProductCatId == category.Id)
                    .OrderByDescending(t => t.Id).Paginate(page);
                return View(category);
            }
        }

        public async Task<ActionResult> Categories(int id, int? page)
        {
            using (var dataModel = new DataModel())
            {
                var company = dataModel.Companies
                    .SingleOrDefault(s => s.Id == id);
                if (company == null)
                {
                    return NotFound();
                }

                if (company.ShowProductsInstedOfCategories == true)
                {
                    await dataModel.Entry(company).Collection(e => e.Product).LoadAsync();
                }
                else
                {
                    ViewBag.Categories = dataModel.ProductCats
                        .Where(t => t.CompanyId == id)
                        .OrderByDescending(t => t.Id)
                        .Paginate(page);
                }

                return View(company);
            }
        }

        public ActionResult Product(int id)
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.AnotherNews = dataModel.Products
                    .Where(t => t.Lang == lang && t.Id != id)
                    .OrderByDescending(o => o.Id).Take(3).ToList();

                ViewBag.LeftNews = dataModel.News
                    .Where(t => t.Lang == lang && !t.type.HasValue)
                    .OrderByDescending(o => o.Id).Skip(3).Take(2)
                    .ToList();

                var products = dataModel.Products.Where(t => t.Lang == lang)
                    .Include(t => t.ProductCat)
                    .OrderByDescending(o => o.Id)
                    .Take(2)
                    .ToList();
                ViewBag.Products = products;

                ViewBag.Acts = dataModel.Activities
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                ViewBag.Next = dataModel.Products.Where(t => t.Lang == lang).FirstOrDefault(w => w.Id > id)?.Id ?? 0;
                ViewBag.prev = dataModel.Products.Where(t => t.Lang == lang).FirstOrDefault(w => w.Id < id)?.Id ?? 0;

                var product = dataModel.Products
                    .Include(e => e.ProductCat)
                    .Include(e => e.ProductCat.Company)
                    .Include(e => e.Company)
                    .SingleOrDefault(s => s.Id == id);
                if (product == null)
                {
                    return RedirectToAction(nameof(Error));
                }

                if (product.Lang != Functions.GetLanguage())
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("Product", product);
            }
        }

        public async Task<ActionResult> Products(int id, int? page)
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var products = dataModel.Products
                    .Where(t => t.Lang == lang && t.ProductCatId == id)
                    .Include(t => t.ProductCat)
                    .OrderByDescending(o => o.Id)
                    .Paginate(page);

                ViewBag.Category = await dataModel.ProductCats
                    .FirstOrDefaultAsync(e => e.Id == id);

                return View(products);
            }
        }

        public ActionResult Activities(int? id, int? page)
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                if (id.HasValue)
                {
                    ViewBag.AnotherNews = dataModel.News.Where(w => w.Id != id)
                        .Where(t => t.Lang == lang && !t.type.HasValue)
                        .OrderByDescending(o => o.Id).Take(3).ToList();

                    ViewBag.LeftNews = dataModel.News.Where(t => t.Id != id)
                        .Where(t => t.Lang == lang && !t.type.HasValue)
                        .OrderByDescending(o => o.Id).Skip(3).Take(2)
                        .ToList();

                    ViewBag.Products = dataModel.Products
                        .Where(t => t.Lang == lang)
                        .OrderByDescending(o => o.Id).Take(2)
                        .ToList();

                    ViewBag.Acts = dataModel.Activities
                        .Where(t => t.Lang == lang)
                        .OrderByDescending(o => o.Id).Take(2)
                        .ToList();

                    ViewBag.Next = dataModel.Activities.Where(t => t.Lang == lang).FirstOrDefault(w => w.Id > id)?.Id ??
                                   0;
                    ViewBag.prev = dataModel.Activities.Where(t => t.Lang == lang).FirstOrDefault(w => w.Id < id)?.Id ??
                                   0;

                    return View("Activity", dataModel.Activities.Find((object)id.Value));
                }
                return View(dataModel.Activities.Where(t => t.Lang == lang).OrderByDescending(o => o.Id)
                    .Paginate(page));
            }
        }

        public ActionResult Media()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Media.OrderByDescending(o => o.Id).ToList());
            }
        }

        public ActionResult Center()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Albums.Include("image").Where(w => w.type.HasValue && w.type.Value == 1)
                    .OrderByDescending(o => o.Id).ToList());
            }
        }

        public ActionResult Exhibtion()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Albums.Include("image").Where(w => w.type.HasValue && w.type.Value == 3)
                    .OrderByDescending(o => o.Id).ToList());
            }
        }

        public ActionResult Archive()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Albums.Include("image").Where(w => w.type.HasValue && w.type.Value == 2)
                    .OrderByDescending(o => o.Id).ToList());
            }
        }

        public ActionResult Show(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("index");
            }

            using (var dataModel = new DataModel())
            {
                return View(dataModel.Albums.Include("image").FirstOrDefault(w => w.Id == id));
            }
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult SendComment(Comment model, HttpPostedFileBase file, bool captchaValid)
        {
            using (var dataModel = new DataModel())
            {
                if (file != null)
                {
                    var str = Guid.NewGuid().ToString().Substring(0, 20) + new FileInfo(file.FileName).Extension;
                    file.SaveAs(Server.MapPath("~/Recources/Uploads/" + str));
                    model.comment_media = str;
                }
                model.cdate = DateTime.Now;
                dataModel.Comments.Add(model);
                dataModel.SaveChanges();

                TempData["success"] = "تم ارسال الطلب بنجاح";

                return RedirectToAction(nameof(Career), new { id = model.job_id });
            }
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult Send(Contact model, HttpPostedFileBase file, int? companyId, ContactType type, string redirectTo)
        {
            using (var dataModel = new DataModel())
            {
                var contact = new Contact
                {
                    Cdate = DateTime.Now,
                    Email = model.Email,
                    Message = model.Message,
                    Name = model.Name,
                    IsRead = false,
                    CompanyId = companyId,
                    Type = type,
                    Mobile = model.Mobile,
                    Phone = model.Phone,
                };

                if (file != null && file.ContentLength > 0)
                {
                    contact.CV = Functions.SaveTempFile(file, "~/Recources/CV", false);
                }

                dataModel.Contacts.Add(contact);
                dataModel.SaveChanges();

                return getMessage(MStatus.success, "", string.IsNullOrEmpty(redirectTo) ? "Index" : redirectTo, "Home");
            }
        }

        [HttpPost]
        public ActionResult addToCart(int product_id, int poduct_amount, HttpPostedFileBase file = null)
        {
            using (var dataModel = new DataModel())
            {
                var product = dataModel.Products
                    .Include(t => t.ProductCat)
                    .Single(t => t.Id == product_id);

                var productList1 = new List<ProductItem>();
                var item = new ProductItem();
                if (file != null)
                {
                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        item.photo = Convert.ToBase64String(reader.ReadBytes(file.ContentLength));
                    }
                }

                item.productAmount = poduct_amount;
                item.productItem = product_id;
                item.title = product.Title;
                item.productCompany = product.ProductCat.CompanyId;

                product.Discount_Cost = poduct_amount;
                if (Session["products"] == null)
                {
                    productList1.Add(item);
                    Session["products"] = productList1;
                }
                else
                {
                    var productList2 = (List<ProductItem>)Session["products"];
                    productList2.Add(item);
                    Session["products"] = productList2;
                }
                return RedirectToAction("product", new
                {
                    id = product_id
                });
            }
        }

        public ActionResult RemoveFromCart(int id)
        {
            if (Session["products"] != null)
            {
                var list = (List<ProductItem>)Session["products"];
                var product = list.First(s => s.productItem == id);
                list.Remove(product);
                Session["products"] = list;
            }
            return RedirectToAction(nameof(Cart));
        }

        public ActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Cart(CartModel model)
        {
            var entity1 = new UsersProducts
            {
                cdate = DateTime.Now,
                company_id = model.companyId,
                email = model.email,
                fullname = model.name,
                mobile = model.mobile,
                Country = model.Country,
                City = model.City,
                Notes = model.Notes,
                status = 0
            };
            using (var dataModel = new DataModel())
            {
                var usersProducts = dataModel.UsersProducts.Add(entity1);
                foreach (var productItem in model.items)
                {
                    var entity2 = new RequestedProducts
                    {
                        amount = productItem.productAmount,
                        product_id = productItem.productItem,
                        cdate = DateTime.Now,
                        company_id = productItem.productCompany,
                        UsersProduct = usersProducts,
                        status = 0,
                        Photo = !string.IsNullOrWhiteSpace(productItem.photo) ?
                            Functions.SaveTempFile(
                                Convert.FromBase64String(productItem.photo),
                                "~/Recources/ProductRequest"
                            ) : null
                    };
                    dataModel.RequestedProduct.Add(entity2);
                }
                dataModel.SaveChanges();
                Session["products"] = new List<ProductItem>();

                try
                {
                    var company = dataModel.Companies
                        .SingleOrDefault(e => e.Id == model.companyId);
                    if (company == null)
                    {
                        throw new NullReferenceException();
                    }

                    var options = await GetEmailConfig(dataModel);
                    var emailSender = new EmailSender
                    {
                        Body = Resources.Resource.NewOrder,
                        Sub = "Products order",
                        To = new List<string>
                        {
                            company.Email
                        }
                    };
                    await emailSender.SendEmailAsync(options);
                }
                catch (Exception)
                {
                    // ignore
                }

            }
            return getMessage(MStatus.success, "", "Cart", "Home");
        }

        private async Task<EmailCredintials> GetEmailConfig(DataModel dataModel)
        {
            var configs = await dataModel.Configrations
                .Where(t => t.Config_name.StartsWith("email_"))
                .ToListAsync();
            var emailCreds = new EmailCredintials();
            configs.ForEach(c =>
            {
                if (c.Config_name.Equals("email_password"))
                {
                    emailCreds.Password = c.Config_details;
                }
                else if (c.Config_name.Equals("email_enable_ssl"))
                {
                    emailCreds.EnableSsl = c.Config_details == "true" || c.Config_details == "1";
                }
                else if (c.Config_name.Equals("email_username"))
                {
                    emailCreds.Username = c.Config_details;
                }
                else if (c.Config_name.Equals("email_port"))
                {
                    emailCreds.Port = int.Parse(c.Config_details);
                }
                else if (c.Config_name.Equals("email_host"))
                {
                    emailCreds.Server = c.Config_details;
                }
                else if (c.Config_name.Equals("email_from"))
                {
                    emailCreds.From = c.Config_details;
                }
            });

            return emailCreds;
        }

        [HttpPost]
        public string Mailing(string email)
        {
            using (var dataModel = new DataModel())
            {
                if (dataModel.MailingLists.FirstOrDefault(w => w.Email.Contains(email)) != null)
                {
                    return
                        "<script language='javascript' type='text/javascript'>window.top.window.parent.error();</script>";
                }

                dataModel.MailingLists.Add(new MailingList
                {
                    Email = email
                });
                dataModel.SaveChanges();
                return
                    "<script language='javascript' type='text/javascript'>window.top.window.parent.error();</script>";
            }
        }
        public ActionResult About()
        {
            var lang = Functions.GetLanguage();

            using (var dataModel = new DataModel())
            {
                ViewBag.video =
                    dataModel.Pages
                        .FirstOrDefault(w => w.lang == lang && w.tag.Equals("about"))
                        ?.video;

                ViewBag.about =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("about"))?.details;

                ViewBag.vision =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("vision"))?.details;

                ViewBag.target =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("message"))?.details;

                return View();
            }
        }

        public ActionResult Social()
        {
            var lang = Functions.GetLanguage();

            using (var dataModel = new DataModel())
            {
                var social = dataModel.Pages
                    .Include(t => t.Image)
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("social"));
                ViewBag.social =
                    social?.details;
                ViewBag.socialPhoto =
                    social?.photo;

                ViewBag.Images = social?.Image;
                return View();
            }
        }

        public ActionResult HQ()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.photo = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("hq"))
                    ?.photo;

                ViewBag.hq = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("hq"))
                    ?.details;

                return View();
            }
        }

        public ActionResult Environment()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var env = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("enviroment"));
                var feature = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("features"));
                var training = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("training"));

                ViewBag.env_photo = env?.photo;
                ViewBag.env = env?.details;

                ViewBag.feature_photo = feature?.photo;
                ViewBag.feature = feature?.details;

                ViewBag.training_photo = training?.photo;
                ViewBag.training = training?.details;

                return View();
            }
        }

        public ActionResult Vision()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.target =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("target"))?.details;

                ViewBag.vision =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("vision"))?.details;

                ViewBag.message =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("message"))?.details;

                ViewBag.video =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("Vision"))?.video;

                ViewBag.value =
                    dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("value"))?.details;

                return View();
            }
        }

        public ActionResult Managment()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var ceo = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("ceo"));
                ViewBag.manager = ceo?.details;

                ViewBag.video = ceo?.video;

                ViewBag.photo = ceo?.photo;

                return View();
            }
        }

        public ActionResult CertificationsAndAccreditations()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var page = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("certifications_and_accreditations"));
                return View(page);
            }
        }
        public ActionResult CorporateStrategy()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var page = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("corporate_strategy"));
                return View(page);
            }
        }
        public ActionResult ExecutiveManagement()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.Exctives = dataModel.Exctives
                    .Where(t => t.Lang == lang && t.CompanyId.HasValue && t.CompanyId == 1)
                    .ToList();

                ViewBag.StaffWord = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("executive_management"))
                    ?.details;

                return View();
            }
           
            
        }
        public ActionResult PoliciesandGovernance()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var page = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("policies_and_governance"));
                return View(page);
            }
        }
        
            public ActionResult MediaContact()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                var page = dataModel.Pages.FirstOrDefault(w => w.lang == lang && w.tag.Equals("media_contact"));
                return View(page);
            }
        }
        public ActionResult Contact()
        {
            using (var db = new DataModel())
            {
                var cc = db.Companies.Include(e => e.Branches).ToList();
                ViewBag.MainCompany = cc.FirstOrDefault(t => t.Id == 1);
            }
            return View();
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult Contact(Contact model, bool captchaValid)
        {
            var lang = Functions.GetLanguage();
            using (var db = new DataModel())
            {
                db.Contacts.Add(model);
                db.SaveChanges();
            }
            TempData["Message"] = lang == 1 ? "تمت العملية بنجاح " : "Message sent successfully";
            return View();
        }

        public ActionResult Support()
        {
            var lang = Functions.GetLanguage();
            using (var db = new DataModel())
            {
                var faqs = db.Pages.Where(t => t.lang == lang && t.tag == "faq");
                ViewBag.FAQs = faqs.ToList();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Support(Contact model)
        {
            var lang = Functions.GetLanguage();
            TempData["Message"] = lang == 1 ? "تمت العملية بنجاح " : "Message sent successfully";
            return View();
        }

        public ActionResult Company(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var company = dataModel.Companies.FirstOrDefault(w => w.Id == id);
                if (company == null)
                {
                    return NotFound();
                }

                var companyName = Functions.IsEnglish() ? company.ETitle : company.Title;

                return RedirectToAction(nameof(Companies), new RouteValueDictionary(
                    new { title = companyName.Trim().Replace(" ", "-"), id }));
            }
        }

        [Route("/Companies/{id}/{title}")]
        public ActionResult Companies(int id, string title)
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.CompanyNews = dataModel.NewsCompanies
                    .Include(e => e.News)
                    .Where(t => t.CompanyId == id && t.News.Lang == lang && !t.News.type.HasValue)
                    .Select(s => s.News)
                    .OrderByDescending(t => t.Id)
                    .Take(4)
                    .ToList();

                ViewBag.Reports = dataModel.Report
                    .Where(w => w.company_id == id && w.Lang == lang)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                var company = dataModel.Companies
                    .Where(w => w.Id == id)
                    .Include(x => x.ProductCats)
                    .Include(x => x.Product)
                    .Include(s => s.Staff)
                    .Include(s => s.Exctive)
                    .Include(i => i.Image)
                    .Include(e => e.Branches)
                    .FirstOrDefault();
                var conTitle = (Functions.IsEnglish() ? company.ETitle : company.Title)
                    .Trim().Replace(" ", "-");
                if (title != conTitle)
                {
                    return RedirectToAction(nameof(Companies), new RouteValueDictionary(
                        new { title = conTitle, id }));
                }

                return View("Company", company);
            }
        }

        public ActionResult Staff()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.Staffs = dataModel.Staffs
                    .Where(t => t.Lang == lang && t.company_id.HasValue && t.company_id == 1)
                    .ToList();

                ViewBag.StaffWord = dataModel.Pages
                    .FirstOrDefault(w => w.lang == lang && w.tag.Equals("bod"))
                    ?.details;

                return View();
            }
        }

        public ActionResult Documents()
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.AnotherNews = dataModel.News
                    .Where(t => t.Lang == lang && !t.type.HasValue)
                    .OrderByDescending(o => o.Id).Take(3).ToList();

                ViewBag.LeftNews = dataModel.News
                    .Where(t => t.Lang == lang && !t.type.HasValue)
                    .OrderByDescending(o => o.Id).Skip(3).Take(2)
                    .ToList();

                ViewBag.Products = dataModel.Products
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                ViewBag.Acts = dataModel.Activities
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                return View();
            }
        }

        public ActionResult Reports(int? page)
        {
            var lang = Functions.GetLanguage();
            using (var dataModel = new DataModel())
            {
                ViewBag.AnotherNews = dataModel.News
                    .Where(t => t.Lang == lang && !t.type.HasValue)
                    .OrderByDescending(o => o.Id).Take(3).ToList();

                ViewBag.LeftNews = dataModel.News
                    .Where(t => t.Lang == lang && !t.type.HasValue)
                    .OrderByDescending(o => o.Id).Skip(3).Take(2)
                    .ToList();

                ViewBag.Products = dataModel.Products
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                ViewBag.Acts = dataModel.Activities
                    .Where(t => t.Lang == lang)
                    .OrderByDescending(o => o.Id).Take(2)
                    .ToList();

                return View(dataModel.Report.Where(w => w.Lang == lang).Include(m => m.Company)
                    .OrderByDescending(o => o.Id)
                    .Paginate(page));
            }
        }
    }
}