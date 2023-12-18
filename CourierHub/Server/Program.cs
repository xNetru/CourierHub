using CourierHub.Server.Data;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierHub {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            IConfiguration configuration = builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

            builder.Services.AddDbContext<CourierHubDbContext>(options => options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnection"]));

            builder.Services.AddSingleton<ICloudStorage>(provider => {
                string azure = configuration.GetSection("AzureStorage")["ConnectionString"] ?? throw new NullReferenceException("Azure connection string could not be loaded!");
                string sas = configuration.GetSection("AzureStorage")["SasToken"] ?? throw new NullReferenceException("Azure SAS token could not be loaded!");
                return new AzureStorage(azure, sas);
            });

            builder.Services.AddSingleton(provider => {
                return new ApiContainer(new CourierHubApi(), new SzymoHubApi(), new WeraHubApi());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseWebAssemblyDebugging();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}