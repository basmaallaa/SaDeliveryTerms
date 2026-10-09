using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryTermsPortal.Pages.Accounts;

public class LogoutModel : PageModel
{
    public IActionResult OnGet() => RedirectToLogin();

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToLogin();
    }

    private IActionResult RedirectToLogin()
    {
        var lang = RouteData.Values["culture"]?.ToString() == "ar" ? "ar" : "en";
        return LocalRedirect($"/{lang}/Accounts/Login");
    }
}