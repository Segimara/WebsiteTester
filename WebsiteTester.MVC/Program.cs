using WebsiteTester.MVC.Services;
using WebsiteTester.Persistenñe;

namespace WebsiteTester.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddWebsiteTesterLogic();

            builder.Services.AddWebsiteTesterPersistenñe(Environment.GetEnvironmentVariable("DB_CONNECTION"));

            builder.Services.AddScoped<ResultsSaverService>();
            builder.Services.AddScoped<ResultsReceiverService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=WebsiteTester}/{action=Index}/{id?}");

            app.Run();
        }
    }
}