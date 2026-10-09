using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace DeliveryTermsPortal.Helpers
{
    public static class Helper
    {
        private static readonly HttpClient client = new();

        public static string? GetClaimValue(ClaimsPrincipal user, string claimType)
        {
            return user.FindFirst(claimType)?.Value;
        }

        public static Task<HttpResponseMessage> GetRequest(string url, string? token)
        {
            return SendRequest(HttpMethod.Get, url, null, token);
        }

        public static Task<HttpResponseMessage> PostRequest(string url, string json, string? token)
        {
            return SendRequest(HttpMethod.Post, url, json, token);
        }

        public static Task<HttpResponseMessage> PutRequest(string url, string json, string? token)
        {
            return SendRequest(HttpMethod.Put, url, json, token);
        }

        public static Task<HttpResponseMessage> DeleteRequest(string url, string? token)
        {
            return SendRequest(HttpMethod.Delete, url, null, token);
        }

        //private static HttpRequestMessage Build(HttpMethod method, string url, string? json, string? token)
        //{
        //    var req = new HttpRequestMessage(method, url);
        //    if(json != null)
        //    {
        //        req.Content = new StringContent(json, Encoding.UTF8,"application/json");
        //    }
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    }

        //    return req;
        //}
        private static Task<HttpResponseMessage> SendRequest(HttpMethod method, string url, string? json, string? token)
        {
            var req = new HttpRequestMessage(method, url);
            if (json != null)
            {
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            if (!string.IsNullOrEmpty(token))
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client.SendAsync(req);
        }
    }
}
