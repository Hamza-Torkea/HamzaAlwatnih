using alwatnia.Areas.Cpanel.Services;
using alwatnia.Areas.Cpanel.ViewModels;
using alwatnia.Helper;
using alwatnia.Models;
using alwatnia.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public partial class NewsController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.News.Include("Comments").OrderByDescending(o => o.Id).ToList();
                if (!string.IsNullOrEmpty(titleName))
                {
                    list = list.Where(w => w.Title.Contains(titleName)).OrderByDescending(o => o.Id).ToList();
                }

                if (!string.IsNullOrEmpty(langId))
                {
                    list = list.Where(w => w.Lang == int.Parse(langId)).OrderByDescending(o => o.Id).ToList();
                }

                return View(list);
            }
        }

        public ActionResult Create()
        {
            ViewBag.company = getCompanies();
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewsViewModel model)
        {
            ViewBag.company = getCompanies();

            var tuple = Functions.ValidateImage(model.File, true);
            if (!tuple.Item1)
            {
                TempData["Message"] = tuple.Item2;
                TempData["Status"] = "danger";
                return View(model);
            }
            var str = Functions.SaveTempFile(model.File, "~/Recources/News");
            var news = News.Create(model);
            news.Photo = str;
            news.Lang = model.Lang ?? 1;
            using (var dataModel = new DataModel())
            {
                dataModel.News.Add(news);
                dataModel.SaveChanges();

                if (model.Companies.Any())
                {
                    var newsCompanies = new List<NewsCompany>();
                    model.Companies.ForEach(c => newsCompanies.Add(new NewsCompany
                    {
                        CompanyId = c,
                        News = news
                    }));
                    dataModel.NewsCompanies.AddRange(newsCompanies);
                    dataModel.SaveChanges();
                }

                return RedirectToAction(nameof(SendNews), new { newsId = news.Id });
                // return getMessage(MStatus.success, "", "Index", "News");
            }
        }

        public async Task<ActionResult> SendNews(int newsId)
        {
            using (var dataModel = new DataModel())
            {
                var emails = await dataModel.MailingLists
                    .Where(m => m.cat1.Contains("مراسلة الأخبار") ||
                                m.cat2.Contains("مراسلة الأخبار") ||
                                m.cat3.Contains("مراسلة الأخبار"))
                    .ToListAsync();

                ViewBag.NewsId = newsId;
                ViewBag.Emails = emails;
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Send(ViewModels.NewsController.SendNewsViewModel model)
        {
            using (var dataModel = new DataModel())
            {
                var configs = await dataModel.Configrations
                    .Where(t => t.Config_name.StartsWith("email_") ||
                                t.Config_name.StartsWith("twitter_"))
                    .ToListAsync();
                var emailCreds = new EmailCredintials();
                var twitterCreds = new TwitterProviderOptions();
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
                    else if (c.Config_name.Equals("twitter_consumer_key"))
                    {
                        twitterCreds.ConsumerKey = c.Config_details;
                    }
                    else if (c.Config_name.Equals("twitter_consumer_secret"))
                    {
                        twitterCreds.ConsumerSecret = c.Config_details;
                    }
                    else if (c.Config_name.Equals("twitter_access_token"))
                    {
                        twitterCreds.AccessToken = c.Config_details;
                    }
                    else if (c.Config_name.Equals("twitter_access_token_secret"))
                    {
                        twitterCreds.AccessTokenSecret = c.Config_details;
                    }
                });

                var news = await dataModel.News.SingleAsync(s => s.Id == model.NewsId);
                var path = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}Home/News/" +
                           model.NewsId +
                           $"?culture={(news.Lang == (int)Languages.Arabic ? "ar" : "en")}";
                var emailSender = new EmailSender
                {
                    Body = string.Format(
                        news.Lang == (int)Languages.Arabic ? Resources.Resource.NewsBodyAr : Resources.Resource.NewsBodyEn,
                        news.Title,
                        $"<a href=" + path + $">{news.Title}</a>"
                    ),
                    Sub = news.Title,
                    To = model.Emails
                };

                if (model.SendToTwitter)
                {
                    await Task.Run(() => SendTweet(twitterCreds, $"{news.Title} {path}"));
                }

                if (model.SendToFacebook)
                {
                    await PublishToFaceBook($"{news.Title}", path);
                }

                if (model.Emails != null && model.Emails.Any())
                {
                    await emailSender.SendEmailAsync(emailCreds);
                }

                return getMessage(MStatus.success, "", "Index", "News");
            }
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.company = getCompanies();
            if (!id.HasValue)
            {
                return RedirectToAction("index");
            }

            using (var dataModel = new DataModel())
            {
                var news = dataModel.News.Find((object)id.Value);
                var newsCompanies = dataModel.NewsCompanies
                    .Where(s => s.NewsId == id.Value)
                    .ToList();
                return View(NewsViewModel.MapFromNews(news, newsCompanies));
            }
        }

        [HttpPost]
        public ActionResult Edit(NewsViewModel model)
        {
            ViewBag.company = getCompanies();
            using (var dataModel = new DataModel())
            {
                var news = dataModel.News.Find((object)model.Id);
                if (news == null)
                {
                    TempData["Status"] = "danger";
                    return View(model);
                }

                if (model.File != null)
                {
                    var tuple = Functions.ValidateImage(model.File, true);
                    if (!tuple.Item1)
                    {
                        TempData["Message"] = tuple.Item2;
                        TempData["Status"] = "danger";
                        return View(model);
                    }
                    var str = Functions.SaveTempFile(model.File, "~/Recources/News");
                    news.Photo = str;
                }
                news.Edit(model);
                var newsCompanies = dataModel.NewsCompanies
                    .Where(s => s.NewsId == model.Id)
                    .ToList();
                var toBeRemoved = new List<NewsCompany>();
                var toBeAdded = new List<NewsCompany>();
                foreach (var companyId in model.Companies)
                {
                    if (newsCompanies.All(c => c.CompanyId != companyId))
                    {
                        toBeAdded.Add(new NewsCompany
                        {
                            News = news,
                            CompanyId = companyId
                        });
                    }
                }

                foreach (var newsCompany in newsCompanies)
                {
                    if (!model.Companies.Contains(newsCompany.CompanyId))
                    {
                        toBeRemoved.Add(newsCompany);
                    }
                }

                dataModel.NewsCompanies.AddRange(toBeAdded);
                dataModel.NewsCompanies.RemoveRange(toBeRemoved);

                dataModel.Entry(news).State = EntityState.Modified;
                dataModel.SaveChanges();

                return getMessage(MStatus.success, "", "Index", "News");
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
                var entity = dataModel.News.Find((object)id.Value);
                dataModel.News.Remove(entity);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "News");
            }
        }

        public ActionResult Comments(int? id, string comment_auther, string title)
        {
            using (var dataModel = new DataModel())
            {
                var list = dataModel.Comments.Where(w => w.news_id.HasValue).Include(w => w.News).ToList();
                if (id.HasValue)
                {
                    list = list.Where(w =>
                    {
                        var newsId = w.news_id;
                        var nullable = id;
                        return newsId.GetValueOrDefault() == nullable.GetValueOrDefault() &&
                               newsId.HasValue == nullable.HasValue;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(comment_auther))
                {
                    list = list.Where(w => w.comment_auther.Contains(comment_auther)).ToList();
                }

                if (!string.IsNullOrEmpty(title))
                {
                    list = list.Where(w => w.News.Title.Contains(title)).ToList();
                }

                return View(list);
            }
        }

        #region PublishToFacebook

        private async Task PublishToFaceBook(string message, string link)
        {
            var facebookProvider = new FacebookProvider();
            await SetProviderObtions(facebookProvider);

            try
            {
                facebookProvider.Post(message, link);
            }
            catch (Exception)
            {
                facebookProvider.RefereshTokens();
                facebookProvider.Post(message, link);
            }
        }

        private async Task SetProviderObtions(FacebookProvider provider)
        {
            using (var dataModel = new DataModel())
            {
                var appId = await dataModel.Configrations
                    .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_app_id"));
                var appSecret = await dataModel.Configrations
                    .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_app_secret"));
                var pageId = await dataModel.Configrations
                    .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_page_id"));
                var accessToken = await dataModel.Configrations
                    .SingleOrDefaultAsync(s => s.Config_name.Equals("facebook_access_token"));
                if (appId == null || appSecret == null || pageId == null)
                {
                    throw new NullReferenceException();
                }

                var facebookProviderOptions = new FacebookProviderOptions(appId.Config_details,
                    appSecret.Config_details,
                    pageId.Config_details);
                provider.SetOptions(facebookProviderOptions);
                var token = provider.RefereshTokens();
                if (accessToken == null)
                {
                    accessToken = new Configration
                    {
                        Config_name = "facebook_access_token",
                        Config_details = token
                    };
                    dataModel.Configrations.Add(accessToken);
                }
                else
                {
                    accessToken.Config_details = token;
                    dataModel.Entry(accessToken).State = EntityState.Modified;
                }

                await dataModel.SaveChangesAsync();
            }
        }

        #endregion

        #region PulishToTwitter

        private void SendTweet(TwitterProviderOptions options, string message)
        {
            try
            {
                var twitterProvider = new TwitterProvider(options);
                twitterProvider.Post(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}