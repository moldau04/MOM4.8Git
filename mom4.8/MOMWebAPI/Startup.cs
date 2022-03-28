using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MOMWebAPI.Areas.AP.Controllers;
using MOMWebAPI.Areas.Projects.Controllers;
using MOMWebAPI.Utility;
using MOMWebAPI.Areas.Payroll.Controllers;
using MOMWebAPI.Areas.Payroll;
using MOMWebAPI.Areas.Programs.Controllers;
using MOMWebAPI.Areas.Inventory.Controllers;
using MOMWebAPI.Areas.Customers.Controllers;

using MOMWebAPI.Areas.DashBoard.Controllers;
using MOMWebAPI.Areas.Schedule.Controllers;

namespace MOMWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        readonly string _allowSpecificOrigins = "_allowSpecificOrigins";
        public static string connectionString { get; set; }
        public static string MSconnectionString { get; set; }
        public static string originString { get; set; }
        public static string originName { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddCors(options =>
            {
                originName = Configuration["OriginString:Origin"];
                //originName = Configuration["AppSettings:CorsOrigin"];
                options.AddPolicy("_allowSpecificOrigins",
                     //builder => builder.WithOrigins(originName)
                     builder => builder.WithOrigins("http://localhost:62344", "http://localhost:2018", "http://webstaging.myesserp.com", "http://webdev.myesserp.com:81/MomWebApp", "http://demomom5.myesserp.com/")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowCredentials()
                    );
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);             

            services.AddSingleton<IUtilityRepository, UtilityRepository>();

            services.AddSingleton<IUserAuthenticationRepository, UserAuthenticationRepository>();

            services.AddSingleton<IeTimesheet, eTimeSheetRepository>();

            #region Customers Module

            services.AddSingleton<ICustomersRepository, CustomersRepository>();
            services.AddSingleton<ILocationsRepository, LocationsRepository>();
            services.AddSingleton<IEquipmentRepository, EquipmentRepository>();
            services.AddSingleton<IReceivePaymentRepository, ReceivePaymentRepository>();
            services.AddSingleton<IMakeDepositRepository, MakeDepositRepository>();
            services.AddSingleton<IiCollectionsRepository, iCollectionsRepository>();

            #endregion

            #region AP Module

            services.AddSingleton<IVendorRepository, VendorRepository>(); 

            services.AddSingleton<IManageChecksRepository, ManageChecksRepository>();

            services.AddSingleton<IBillRepository, BillRepository>();

            #endregion

            #region Inventory Module

            services.AddSingleton<IInventoryRepository, InventoryRepository>();

            services.AddSingleton<IPostToProjectRepository, PostToProjectRepository>();

            services.AddSingleton<IItemAdjustmentRepository, ItemAdjustmentRepository>();

            #endregion

            #region Dashboard Module
            services.AddSingleton<IDashBoardRepository, DashBoardRepository>();
            #endregion

            #region ProgramsModules

            //services.AddSingleton<ISetupRepository, SetupRepository>();

            #endregion

            #region ProjectModules

            services.AddSingleton<IProjectRepository, ProjectRepository>();

            #endregion

            #region Scheduler 
            services.AddSingleton<IGoogleMapRepository, GoogleMapRepository>();            
            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
           
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_allowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }); 
           
            connectionString = Configuration["ConnectionStrings:DbConnection"];
            MSconnectionString = Configuration["MSConnectionStrings:MSConnection"];
        }
    }
}
