using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using T_Office.ApiCore.Hubs;
using T_Office.ApiCore.LiteDB;

namespace T_Office.ApiCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddTransient<ISmartCardReaderPersistence, LiteDBSmartCardReaderPersistence>();
            services.AddTransient<IAgentDataPersistence, LiteDBAgentDataPersistence>();

            services.AddSignalR();

            services.AddControllers()
                    // .AddNewtonsoftJson();
                    .AddJsonOptions(option => option.JsonSerializerOptions.PropertyNamingPolicy = null); // set Pascal-case property serialization
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            // important: this should go before UseAuthorization() and/or UseEndpoints() methods
            app.UseCors("CorsPolicy");

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TOfficeHub>("/tofficehub");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("TOffice API v1.0");
                });
            });
        }
    }
}
