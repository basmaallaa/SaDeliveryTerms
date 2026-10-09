using DeliveryTermsBL.IServices.IAuthService;
using DeliveryTermsBL.IServices.ISalesService;
using DeliveryTermsBL.Services.AuthService;
using DeliveryTermsBL.Services.SalesService;
using DeliveryTermsDL.UnitOfWork;

namespace DeliveryTermsSL.Extensions
{
    public static class SalesModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddSalesModule(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISaDeliveryTermService, SaDeliveryTermService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}
