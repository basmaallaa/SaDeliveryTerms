using DeliveryTermsBL.IServices.IAuthService;
using DeliveryTermsBL.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DeliveryTermsSL.Extensions
{
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuth (this IServiceCollection services, IConfiguration config) 
        {
            var key = config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(key) || key.Length < 32)
                throw new InvalidOperationException(
                    "Jwt:Key is missing or shorter than 32 characters. Set it with dotnet user-secrets.");

            // token creation lives in the BL
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            // token validation lives here (middleware)
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = config["Jwt:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });

            services.AddAuthorization();
            return services;
        }
    }
}
