using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using DeliveryTermsPortal.Helpers;
using DeliveryTermsPortal.Localization;

namespace DeliveryTermsPortal.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ISharedLocalizer _localizer;

        public LoginModel(IConfiguration configuration, ISharedLocalizer localizer)
        {
            _configuration = configuration;
            _localizer = localizer;
        }

        [BindProperty, Required] public string Username { get; set; } = "";
        [BindProperty, Required] public string Password { get; set; } = "";
        [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }

        public string? Error { get; set; }

        private string Lang =>
            RouteData.Values["culture"]?.ToString() == "ar" ? "ar" : "en";
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            HttpResponseMessage response;
            try
            {
                response = await Helper.PostRequest(
                    _configuration.GetValue<string>("Uri") + "Auth/Login",
                    JsonConvert.SerializeObject(new { Username, Password }),
                    token: null);
            }
            catch (HttpRequestException)
            {
                Error = _localizer["9031"];
                return Page();
            }

            if (!response.IsSuccessStatusCode)
            {
                Error = _localizer["9015"];
                return Page();
            }

            var token = JObject.Parse(await response.Content.ReadAsStringAsync())["token"]?.ToString();
            if (string.IsNullOrEmpty(token))
            {
                Error = _localizer["9015"];
                return Page();
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim("token", token)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }
            return LocalRedirect($"/{Lang}/");
        }
    }
}
