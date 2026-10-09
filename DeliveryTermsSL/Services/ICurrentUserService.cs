namespace DeliveryTermsSL.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? CompanyCode { get; }
    }
}
