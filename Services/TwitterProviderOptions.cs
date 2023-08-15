namespace alwatnia.Services
{
    public class TwitterProviderOptions
    {
        public readonly string OAuthUrl = "https://api.twitter.com/1.1/statuses/update.json";

        public TwitterProviderOptions()
        {

        }

        public TwitterProviderOptions(string accessTokenSecret,
                                      string accessToken,
                                      string consumerSecret,
                                      string consumerKey)
        {
            AccessTokenSecret = accessTokenSecret;
            AccessToken = accessToken;
            ConsumerSecret = consumerSecret;
            ConsumerKey = consumerKey;
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}