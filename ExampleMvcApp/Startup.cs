using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleMvcApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Feel free to comment and uncomment this line to check if application will automatically rebuild and redeploy.
            app.UseRafPieTimeMeasurementMiddleware(Configuration["Logging:LogLevel:Default"]);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    ///<summary>
    /// Simple class. In real live scenario this class should be moved to separate file. 
    ///</summary>
    public class RafPieServerTimeMeasurementMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly string _defaultLogLevel;

        public RafPieServerTimeMeasurementMiddleware(RequestDelegate next, string defaultLogLevel)        
        {
            _next = next;
            _defaultLogLevel = defaultLogLevel;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sw = new Stopwatch();
            sw.Start();
            await _next(httpContext);
            sw.Stop();
            // Http status code 304 means that cotent was not modified.
            // Response message shouldn't be modified.
            if (httpContext.Response.StatusCode != 304)
            {
                // That console WriteLine was added only to print changes in appsettings.json file.
                Console.WriteLine($"Passed value: {_defaultLogLevel}");

                var message = $"<div style='padding:75px'><p>Action took: {sw.ElapsedMilliseconds} ms. <br />Powered by RafPie.</p></div>";
                httpContext.Response.ContentLength += message.Length;
                await httpContext.Response.WriteAsync(message);
            }
        }
    }

    ///<summary>
    /// Extension class for custom middleware.
    ///</summary>
    public static class RafPieServerTimeMeasurementMiddlewareExtensions
    {
        ///<summary>
        /// Extension method which passes default logging level information to the middleware
        ///</summary>
        public static IApplicationBuilder UseRafPieTimeMeasurementMiddleware(this IApplicationBuilder builder, string defaultLogLevel)
        {
            return builder.UseMiddleware<RafPieServerTimeMeasurementMiddleware>(defaultLogLevel);
        }
    }
}
