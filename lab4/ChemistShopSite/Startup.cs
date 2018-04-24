using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChemistShopSite.Models;


namespace ChemistShopSite
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
            services.AddTransient<MedicamentsContext>();
            services.AddMemoryCache();
            services.AddResponseCaching();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Caching",
                   new CacheProfile()
                   {
                        Duration = 2 * 12 + 240,
                        Location = ResponseCacheLocation.Any
                   });
                options.CacheProfiles.Add("NoCaching",
                   new CacheProfile()
                   {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                   });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseLastElementCache();
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();
            app.UseResponseCaching();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
