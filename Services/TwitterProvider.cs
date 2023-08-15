using System;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace alwatnia.Services
{
    public class TwitterProvider
    {
        private readonly TwitterProviderOptions _options;

        public TwitterProvider(TwitterProviderOptions options)
        {
            _options = options;
        }

        public void Post(string message)
        {
            var authHeader = GenerateAuthorizationHeader(message);
            var postBody = "status=" + Uri.EscapeDataString(message);

            var authRequest = (HttpWebRequest)WebRequest.Create(_options.OAuthUrl);
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.UserAgent = "OAuth gem v0.4.4";
            authRequest.Host = "api.twitter.com";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.ServicePoint.Expect100Continue = false;
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (var stream = authRequest.GetRequestStream())
            {
                var content = Encoding.UTF8.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            var authResponse = authRequest.GetResponse();
            authResponse.Close();
        }

        #region Helpers

        private string GenerateAuthorizationHeader(string status)
        {
            const string signatureMethod = "HMAC-SHA1";
            const string version = "1.0";
            var nonce = GenerateNonce();
            var timestamp = ConvertToUnixTimestamp(DateTime.Now);

            var dst = string.Empty;
            dst += "OAuth ";
            dst += $"oauth_consumer_key=\"{Uri.EscapeDataString(_options.ConsumerKey)}\", ";
            dst += $"oauth_nonce=\"{Uri.EscapeDataString(nonce)}\", ";
            dst +=
                $"oauth_signature=\"{Uri.EscapeDataString(GenerateOauthSignature(status, nonce, timestamp.ToString(CultureInfo.InvariantCulture)))}\", ";
            dst += $"oauth_signature_method=\"{Uri.EscapeDataString(signatureMethod)}\", ";
            dst += $"oauth_timestamp=\"{timestamp}\", ";
            dst += $"oauth_token=\"{Uri.EscapeDataString(_options.AccessToken)}\", ";
            dst += $"oauth_version=\"{Uri.EscapeDataString(version)}\"";
            return dst;
        }

        private string GenerateOauthSignature(string status,
                                              string nonce,
                                              string timestamp)
        {
            const string signatureMethod = "HMAC-SHA1";
            const string version = "1.0";
            var result = string.Empty;
            var dst = string.Empty;

            dst += $"oauth_consumer_key={Uri.EscapeDataString(_options.ConsumerKey)}&";
            dst += $"oauth_nonce={Uri.EscapeDataString(nonce)}&";
            dst += $"oauth_signature_method={Uri.EscapeDataString(signatureMethod)}&";
            dst += $"oauth_timestamp={timestamp}&";
            dst += $"oauth_token={Uri.EscapeDataString(_options.AccessToken)}&";
            dst += $"oauth_version={Uri.EscapeDataString(version)}&";
            dst += $"status={Uri.EscapeDataString(status)}";

            var signingKey =
                $"{Uri.EscapeDataString(_options.ConsumerSecret)}&{Uri.EscapeDataString(_options.AccessTokenSecret)}";

            result += "POST&";
            result += Uri.EscapeDataString(_options.OAuthUrl);
            result += "&";
            result += Uri.EscapeDataString(dst);

            var hmac = new HMACSHA1 { Key = Encoding.UTF8.GetBytes(signingKey) };

            var databuff = Encoding.UTF8.GetBytes(result);
            var hashbytes = hmac.ComputeHash(databuff);

            return Convert.ToBase64String(hashbytes);
        }

        private string GenerateNonce()
        {
            var nonce = string.Empty;
            var rand = new Random();
            for (var i = 0; i < 32; i++)
            {
                var next = rand.Next(65, 90);
                var c = Convert.ToChar(next);
                nonce += c;
            }

            return nonce;
        }

        private double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        #endregion
    }
}