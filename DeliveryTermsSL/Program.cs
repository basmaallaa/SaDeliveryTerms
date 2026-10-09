
using DeliveryTermsBL.IServices.ISharedService;
using DeliveryTermsBL.Services.SharedService;
using DeliveryTermsDL;
using DeliveryTermsSL.Extensions;
using DeliveryTermsSL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace DeliveryTermsSL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<SupplyChainContext>(options =>
            options.UseOracle(
                builder.Configuration.GetConnectionString("OracleConnection")));

            builder.Services.AddScoped<ISharedService, SharedService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddSalesModule();
            builder.Services.AddJwtAuth(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Sales", new OpenApiInfo { Title = "Sales", Version = "v1" });
                c.SwaggerDoc("Auth", new OpenApiInfo { Title = "Auth (dev only)", Version = "v1" });
                c.DocInclusionPredicate((doc, api) => (api.GroupName ?? "Auth") == doc);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Paste the token here"
                });
                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        "/swagger/Sales/swagger.json",
                        "Sales API v1");

                    options.SwaggerEndpoint(
                        "/swagger/Auth/swagger.json",
                        "Auth API v1");

                    options.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}
