namespace alwatnia.Services
{
    public class FacebookProviderOptions
    {
        protected FacebookProviderOptions()
        {

        }

        public FacebookProviderOptions(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
        }

        public FacebookProviderOptions(string appId, string appSecret, string pageId)
        {
            AppId = appId;
            AppSecret = appSecret;
            PageId = pageId;
        }

        public FacebookProviderOptions(string appId, string appSecret, string pageId, string accessToken)
        {
            AppId = appId;
            AppSecret = appSecret;
            PageId = pageId;
            AccessToken = accessToken;
        }

        public void SetPageId(string pageId) => PageId = pageId;
        public void SetAccessToken(string token) => AccessToken = token;

        public string AppId { get; private set; }
        public string AppSecret { get; private set; }
        public string PageId { get; private set; }
        public string AccessToken { get; private set; }
    }
}