using System.Globalization;
using DeliveryTermsPortal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DeliveryTermsPortal.Pages.SALES.DELIVERYTERMS;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    public IndexModel(IConfiguration configuration) => _configuration = configuration;

    public List<DeliveryTermRow> Items { get; set; } = new();

    public class DeliveryTermRow
    {
        public int Code { get; set; }
        public string? SName { get; set; }
        public string? BName { get; set; }
        public string? Name { get; set; }
        public int? Days { get; set; }
        public bool ActiveFlag { get; set; }
    }

    public async Task<IActionResult> OnGet()
    {
        var token = Helper.GetClaimValue(User, "token");
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        var response = await Helper.GetRequest(
            _configuration.GetValue<string>("Uri") +
            "SaDeliveryTerm/GetSaDeliveryTerms?culture=" + culture, token);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            return RedirectToPage("/Accounts/Login");   

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Items = JsonConvert.DeserializeObject<List<DeliveryTermRow>>(json) ?? new();
        }
        else
        {
            TempData["ErrorMessage"] = $"API error {(int)response.StatusCode}";
        }
        return Page();
    }
}