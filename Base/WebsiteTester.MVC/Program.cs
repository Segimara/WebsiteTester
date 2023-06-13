using WebsiteTester.Persistence;
using WebsiteTester.Web.Logic;

namespace WebsiteTester.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder
            //    .AddConsole()
            //    .SetMinimumLevel(LogLevel.Trace);
            //});

            //builder.Services.AddSingleton(loggerFactory.CreateLogger("Program"));
            builder.Services.AddLogging(opt =>
            {
                opt.AddConsole();
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddWebsiteTesterLogic();
            builder.Services.AddWebsiteTesterPersistence(Environment.GetEnvironmentVariable("DB_CONNECTION"));
            builder.Services.AddWebLogic();

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