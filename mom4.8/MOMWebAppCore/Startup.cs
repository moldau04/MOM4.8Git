using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MOMWebAppCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        } 
         
        
        public IConfiguration Configuration { get; }

        public static string MOMBaseWebAPIURL { get; set; }

        public static string MOMBaseReportAPIURL { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddControllersWithViews();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSession(options => {                
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
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
            }
            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "dashboard",
                    template: "{area:exists}/{controller=List}/{action=Index}");

                routes.MapRoute(
                    name: "Customers",
                    template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
              name: "Recurring",
              template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
              name: "Schedule",
              template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
                  name: "Billing",
                  template: "{area:exists}/{controller=List}/{action=Index}");

                routes.MapRoute(
                name: "AccountPayable",
                template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
           name: "Purchase",
           template: "{area:exists}/{controller=List}/{action=Index}");

                routes.MapRoute(
              name: "Sales",
              template: "{area:exists}/{controller=List}/{action=Index}");

                routes.MapRoute(
                 name: "Project",
                 template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
              name: "Inventory",
              template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
              name: "Payroll",
              template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
              name: "Financials",
              template: "{area:exists}/{controller=List}/{action=Index}");

                routes.MapRoute(
          name: "Statements",
          template: "{area:exists}/{controller=List}/{action=Index}");


                routes.MapRoute(
                name: "Programs",
                template: "{area:exists}/{controller=List}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}"); 


            });



            app.UseRouting();

            app.UseAuthorization();

            MOMBaseWebAPIURL = Configuration["BaseAPIURL:MOMBaseWebAPIURL"];

            MOMBaseReportAPIURL = Configuration["BaseAPIURL:MOMBaseReportAPIURL"];


        }
    }
}
