namespace DeliveryTermsSL.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;
        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private string? Claim(string claimType)
        {
            return _accessor.HttpContext?.User?.FindFirst(claimType)?.Value;
        }

        public int? UserId => int.TryParse(Claim("Id"), out var userId) ? userId : null;

        public int? CompanyCode => int.TryParse(Claim("CompanyCode"), out var companyCode) ? companyCode : null;
    }
}
