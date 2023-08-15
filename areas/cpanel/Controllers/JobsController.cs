using alwatnia.Models;
using alwatnia.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.Controllers
{
    public class JobsController : CPBaseController
    {
        public ActionResult Index()
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var langId = Request.QueryString["langId"];

                var list = dataModel.Jobs.Include("Comments").OrderByDescending(o => o.Id).ToList();
                if (!string.IsNullOrEmpty(titleName))
                    list = list.Where(w => w.Title.Contains(titleName)).ToList();
                if (!string.IsNullOrEmpty(langId))
                    list = list.Where(w => w.Lang == int.Parse(langId)).ToList();

                return View(list);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult creattestt()
        {
            return View();
        }

        [HttpPost]
        public ActionResult creattestt(Job model)
        {
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Jobs.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Jobs");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Job model, bool postToFacebook = false, bool postToTwitter = false)
        {
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Jobs.Add(model);
                dataModel.SaveChanges();
                var path = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}/Home/career/" + model.Id;
                if (postToFacebook)
                {
                    await PublishToFaceBook(model.Title, path);
                }
                if (postToTwitter)
                {
                    await PublishToTwitter($"{model.Title} {path}");
                }

                return getMessage(MStatus.success, "", "Index", "Jobs");
            }
        }

        public ActionResult Edit(int? id)
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Jobs.Find((object)id));
            }
        }

        [HttpPost]
        public ActionResult Edit(Job model)
        {
            if (!model.Lang.HasValue)
            {
                model.Lang = 1;
            }
            using (var dataModel = new DataModel())
            {
                dataModel.Entry(model).State = EntityState.Modified;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Jobs");
            }
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");
            using (var dataModel = new DataModel())
            {
                var comments = dataModel.Comments;
                Expression<Func<Comment, bool>> predicate = w => w.job_id.HasValue && w.job_id == id;
                foreach (var entity in comments.Where(predicate))
                    dataModel.Comments.Remove(entity);
                var entity1 = dataModel.Jobs.Find((object)id.Value);
                dataModel.Jobs.Remove(entity1);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Jobs");
            }
        }

        public ActionResult Comments(int? id, string comment_auther, string job)
        {
            using (var dataModel = new DataModel())
            {
                var list = dataModel.Comments.Where(w => w.job_id.HasValue).Include(w => w.Job).ToList();
                if (id.HasValue)
                    list = list.Where(w =>
                    {
                        var jobId = w.job_id;
                        var nullable = id;
                        return jobId.GetValueOrDefault() == nullable.GetValueOrDefault() &&
                               jobId.HasValue == nullable.HasValue;
                    }).ToList();
                if (!string.IsNullOrEmpty(comment_auther))
                    list = list.Where(w => w.comment_auther.Contains(comment_auther)).ToList();
                if (!string.IsNullOrEmpty(job))
                    list = list.Where(w => w.Job.Title.Contains(job)).ToList();
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

        private async Task PublishToTwitter(string message)
        {
            try
            {
                var options = await GetTwitterProviderOptions();
                var twitterProvider = new TwitterProvider(options);
                twitterProvider.Post(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<TwitterProviderOptions> GetTwitterProviderOptions()
        {
            using (var dataModel = new DataModel())
            {
                var twitterCreds = new TwitterProviderOptions();
                var configs = await dataModel.Configrations
                    .Where(t => t.Config_name.StartsWith("twitter_"))
                    .ToListAsync();
                configs.ForEach(c =>
                {
                    if (c.Config_name.Equals("twitter_consumer_key"))
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

                return twitterCreds;
            }
        }

        #endregion
    }
}