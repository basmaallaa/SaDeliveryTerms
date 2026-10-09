using DeliveryTermsPortal.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DeliveryTermsPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages(o =>
            {
                o.Conventions.AddFolderRouteModelConvention("/", model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        var template = selector.AttributeRouteModel!.Template;
                        selector.AttributeRouteModel.Template =
                            AttributeRouteModel.CombineTemplates("{culture=en}", template);
                    }
                });
                o.Conventions.AuthorizeFolder("/");
                o.Conventions.AllowAnonymousToPage("/Accounts/Login");
                o.Conventions.AllowAnonymousToPage("/Error");
            });

            builder.Services.AddSingleton<ISharedLocalizer, SharedLocalizer>();

            builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromMinutes(480);   
                o.Events.OnRedirectToLogin = ctx =>
                {
                    var first = ctx.Request.Path.Value?
                        .Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                    var culture = first == "ar" ? "ar" : "en";
                    var returnUrl = Uri.EscapeDataString(ctx.Request.Path + ctx.Request.QueryString);
                    ctx.Response.Redirect($"/{culture}/Accounts/Login?ReturnUrl={returnUrl}");
                    return Task.CompletedTask;
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var cultures = new[] { "en", "ar" };
            var locOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("en")
                .AddSupportedCultures(cultures)
                .AddSupportedUICultures(cultures);
            locOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = locOptions });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseRequestLocalization(locOptions);   

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
