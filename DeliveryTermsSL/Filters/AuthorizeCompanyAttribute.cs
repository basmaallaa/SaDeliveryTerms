using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeliveryTermsSL.Filters
{
    public class AuthorizeCompanyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claim = context.HttpContext.User.FindFirst("CompanyCode")?.Value;
            if (!int.TryParse(claim, out var company) || company <= 0)
                context.Result = new ForbidResult();
        }
    }
}
