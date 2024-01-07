
using CourierHub.Shared.Data;
using CourierHubWebApi.Middleware;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services;
using CourierHubWebApi.Services.Contracts;
using CourierHubWebApi.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CourierHubWebApi {
    public class Program {
        [Obsolete]
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IInquireService, InquireService>();
            builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
            builder.Services.AddScoped<IPriceCacheService, PriceCacheService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme{
                    Description = "The API key to access the API",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "x-api-key",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    { scheme, new List<string>() }
                };
                x.AddSecurityRequirement(requirement);
            });

            builder.Services.AddDbContext<CourierHubDbContext>(options => options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnection"]));

            builder.Services.AddScoped<IValidator<CreateInquireRequest>, CreateInquireRequestValidator>();
            builder.Services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
            builder.Services.AddScoped<IValidator<ApiSideAddress>, ApiSideAddressValidator>();  
            builder.Services.AddScoped<IValidator<WithdrawOrderRequest>,WithdrawOrderRequestValidator>();
            builder.Services.AddScoped<IValidator<GetOrderStatusRequest>,GetOrderStatusRequestValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseApiKeyMiddleware();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            Console.WriteLine("Koniec");
        }
    }
}