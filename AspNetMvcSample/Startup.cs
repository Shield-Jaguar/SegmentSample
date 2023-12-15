using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Segment.Analytics;
using Segment.Analytics.Utilities;

namespace AspNetMvcSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Analytics.Logger = new SegmentLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // use `InMemoryStorageProvider` to make Analytics stateless
            var configuration = new Configuration(Configuration.GetValue<string>("SegmentKey"),
                flushAt: 5,
                flushInterval: 10,
                storageProvider: new InMemoryStorageProvider());

            services.AddControllersWithViews();
            services.AddScoped(_ => new Analytics(configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
class SegmentLogger : ISegmentLogger
{
    public void Log(LogLevel logLevel, Exception? exception = null, string? message = null)
    {
        switch (logLevel)
        {
            case LogLevel.Warning:
            case LogLevel.Information:
            case LogLevel.Debug:
                Console.Out.WriteLine("Message: " + message);
                break;
            case LogLevel.Critical:
            case LogLevel.Trace:
            case LogLevel.Error:
                Console.Error.WriteLine("Exception: " + exception?.StackTrace);
                Console.Error.WriteLine("Message: " + message);
                break;
            case LogLevel.None:
            default:
                break;
        }
    }
}
