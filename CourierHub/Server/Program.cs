using CourierHub.Cloud;
using CourierHub.Server.Api;
using CourierHub.Server.Containers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Static;
using Microsoft.EntityFrameworkCore;

namespace CourierHub;
public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        IConfiguration configuration = builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

        builder.Services.AddDbContext<CourierHubDbContext>(options =>
            options.UseSqlServer(Base64Coder.Decode(configuration.GetSection("AzureSQLDatabase")["ConnectionString"] ??
                throw new NullReferenceException("Database connection string could not be loaded!"))));

        builder.Services.AddSingleton<ICloudStorage>(provider => {
            string azure = configuration.GetSection("AzureStorage")["ConnectionString"] ??
                throw new NullReferenceException("Storage connection string could not be loaded!");
            string sas = configuration.GetSection("AzureStorage")["SasToken"] ??
                throw new NullReferenceException("Storage SAS token could not be loaded!");
            return new AzureStorage(azure, sas);
        });

        builder.Services.AddSingleton<ICloudCommunicationService>(provider => {
            string connection = configuration.GetSection("AzureCommunicationService")["ConnectionString"] ??
                throw new NullReferenceException("Communication Service connection string could not be loaded!");
            string sender = configuration.GetSection("AzureCommunicationService")["Sender"] ??
                throw new NullReferenceException("Communication Service sender could not be loaded!");
            return new AzureCommunicationService(connection, sender);
        });

        builder.Services.AddSingleton<InquireCodeContainer>();
        builder.Services.AddSingleton<WebApiContainer>();

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