
using CourierHub.Shared.Data;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services;
using CourierHubWebApi.Services.Contracts;
using CourierHubWebApi.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CourierHubWebApi {
    public class Program {
        [Obsolete]
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IInquireService, InquireService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CourierHubDbContext>(options => options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnection"]));

            builder.Services.AddScoped<IValidator<CreateInquireRequest>, CreateInquireRequestValidator>();
            // auto validation 
            //builder.Services.AddFluentValidation();
            //builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}