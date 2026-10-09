using DeliveryTermsPortal.Helpers;
using DeliveryTermsPortal.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DeliveryTermsPortal.Pages.SALES.DELIVERYTERMS;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ISharedLocalizer _localizer;
    public IndexModel(IConfiguration configuration, ISharedLocalizer localizer)
    {
        _configuration = configuration;
        _localizer = localizer;
    }  

    public List<DeliveryTermRow> Items { get; set; } = new();
    [BindProperty] public DeliveryTermInput Input { get; set; } = new();

    public class DeliveryTermRow
    {
        public int Code { get; set; }
        public string? SName { get; set; }
        public string? BName { get; set; }
        public string? Name { get; set; }
        public int? Days { get; set; }
        public bool ActiveFlag { get; set; }
    }

    public class DeliveryTermInput
    {
        [Range(1, 999)] public int Code { get; set; }
        [Required, StringLength(60)] public string SName { get; set; } = "";
        [Required, StringLength(60)] public string BName { get; set; } = "";
        [Range(0, 9999)] public int? Days { get; set; }
        public bool ActiveFlag { get; set; } = true;
    }

    private string Lang => RouteData.Values["culture"]?.ToString() == "ar" ? "ar" : "en";
    private string Token => Helper.GetClaimValue(User, "token") ?? "";
    private string ApiBase => _configuration.GetValue<string>("Uri") + "Sales/SaDeliveryTerm/";

    private IActionResult BackToPage() => LocalRedirect($"/{Lang}/SALES/DELIVERYTERMS/Index");
    private IActionResult ToLogin() => LocalRedirect($"/{Lang}/Accounts/Login");

    public async Task<IActionResult> OnGet()
    {
        try
        {
            var response = await Helper.GetRequest(
                ApiBase + "GetSaDeliveryTerms?culture=" + Lang, Token);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return ToLogin();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Items = JsonConvert.DeserializeObject<List<DeliveryTermRow>>(json) ?? new();
            }
            else
            {
                TempData["ErrorMessage"] = _localizer["9031"];
            }
        }
        catch (HttpRequestException)
        {
            TempData["ErrorMessage"] = _localizer["9031"];
        }
        return Page();
    }

    public async Task<IActionResult> OnGetByCode(int code)
    {
        try
        {
            var r = await Helper.GetRequest(ApiBase + $"GetSaDeliveryTerm/{code}", Token);
            if (r.StatusCode == System.Net.HttpStatusCode.Unauthorized) return Unauthorized();
            return Content(await r.Content.ReadAsStringAsync(), "application/json");
        }
        catch (HttpRequestException)
        {
            return StatusCode(502);
        }
    }

    public Task<IActionResult> OnPostAdd() => Save(HttpMethod.Post, "AddSaDeliveryTerm");
    public Task<IActionResult> OnPostEdit() => Save(HttpMethod.Put, "EditSaDeliveryTerm");

    public Task<IActionResult> OnPostDelete(int code)
        => CallApi(HttpMethod.Delete, $"DeleteSaDeliveryTerm/{code}", "{}");

    private async Task<IActionResult> Save(HttpMethod method, string action)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = _localizer["9030"];
            return BackToPage();
        }
        return await CallApi(method, action, JsonConvert.SerializeObject(Input));
    }

    private async Task<IActionResult> CallApi(HttpMethod method, string action, string json)
    {
        HttpResponseMessage response;
        try
        {
            var url = ApiBase + action;
            response = method.Method switch
            {
                "POST" => await Helper.PostRequest(url, json!, Token),

                "PUT" => await Helper.PutRequest(url, json!, Token),

                "DELETE" => await Helper.DeleteRequest(url, Token),

                _ => throw new InvalidOperationException(
                    $"Unsupported HTTP method: {method}")
            };
        }
        catch (HttpRequestException)
        {
            TempData["ErrorMessage"] = _localizer["9031"];
            return BackToPage();
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            return ToLogin();

        await SetResponseMessage(response);
        return BackToPage();
    }

    private async Task SetResponseMessage(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = _localizer["9031"];
            return;
        }

        var obj = JObject.Parse(await response.Content.ReadAsStringAsync());
        var status = obj["status"]?.Value<bool>() ?? false;
        var message = (Lang == "ar" ? obj["messageAr"] : obj["messageEn"])?.ToString();

        TempData[status ? "SuccessMessage" : "ErrorMessage"] = message;
    }
}