using Facebook;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Web;

namespace alwatnia.Services
{
    public class FacebookProvider
    {
        private FacebookProviderOptions _options;

        public FacebookProvider()
        {
        }

        public FacebookProvider(FacebookProviderOptions options)
        {
            _options = options;
        }

        public void SetOptions(FacebookProviderOptions options) => _options = options;

        public string RefereshTokens()
        {
            var scope = "manage_pages,publish_pages";
            if (HttpContext.Current.Request["code"] == null)
            {
                HttpContext.Current.Response.Redirect(
                    $"https://graph.facebook.com/oauth/authorize?client_id={_options.AppId}&" +
                    $"redirect_uri={HttpContext.Current.Request.Url.AbsoluteUri}&scope={scope}");
            }

            var url =
                "https://graph.facebook.com/oauth/access_token?client_id=" +
                $"{_options.AppId}&redirect_uri={HttpContext.Current.Request.Url.AbsoluteUri}" +
                $"&scope={scope}&code={HttpContext.Current.Request["code"]}&client_secret={_options.AppSecret}";

            var request = WebRequest.Create(url) as HttpWebRequest;

            using (var response = request.GetResponse() as HttpWebResponse)
            {

                var reader = new StreamReader(response.GetResponseStream());
                var vals = reader.ReadToEnd();
                var jsonObj = JObject.Parse(vals);
                var userAccessToken = (string)jsonObj["access_token"];

                var client = new FacebookClient(userAccessToken);
                var token = client.Get($"/{_options.PageId}?fields=access_token&access_token={userAccessToken}");

                var accessToken = (string)((JsonObject)token)["access_token"];
                _options.SetAccessToken(accessToken);
                return accessToken;
            }
        }

        public void Post(string message, string link)
        {
            dynamic post = new ExpandoObject();
            post.message = message;
            post.link = link;

            var client = new FacebookClient(_options.AccessToken);
            client.Post("/me/feed", post);
        }

        public string GetPageId(string pageUrl)
        {
            var getPageId =
                $"https://graph.facebook.com/v2.6/?id={pageUrl}&access_token={_options.AccessToken}";
            var req = WebRequest.Create(getPageId) as HttpWebRequest;
            using (var res = req.GetResponse() as HttpWebResponse)
            {
                var reader = new StreamReader(res.GetResponseStream());
                var vals = reader.ReadToEnd();
                var jsonObj = JObject.Parse(vals);
                var id = (string)jsonObj["id"];

                _options.SetPageId(id); ;
                return id;
            }
        }
    }
}