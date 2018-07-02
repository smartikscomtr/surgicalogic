﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surgicalogic.Contracts.Services;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Contracts.Stores.Base;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Utilities;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Services.Services;
using Surgicalogic.Services.Stores;

namespace Surgicalogic.Api
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
           //DbContext service registered 
            services.AddDbContext<DataContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DataContext"),
                            builder => builder.MigrationsAssembly("Surgicalogic.Data.Migrations"))
            );

            services.AddTransient<IAppServiceProvider, AppServiceProvider>();

            //CROS service registerd. This methode was add besause of allow-control-access-origin 
            services.AddCors(options =>            
                options.AddPolicy("CorsConfig",
                    builder =>
                        builder.WithOrigins(Configuration["AppSettings:Http:AllowedOrigin"])
                       .AllowAnyMethod()
                       .AllowAnyHeader())
            );

            #region StoreService Registeration

            services.AddScoped<IBranchStoreService, BranchStoreService>();
            services.AddScoped<IEquipmentStoreService, EquipmentStoreService>();
            services.AddScoped<IEquipmentTypeStoreService, EquipmentTypeStoreService>();
            services.AddScoped<IPersonnelStoreService, PersonnelStoreService>();
            services.AddScoped<IPersonnelTitleStoreService, PersonnelTitleStoreService>();
            services.AddScoped<IWorkTypeStoreService, WorkTypeStoreService>();
            
            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //StaticFile Allowance Initialized
            app.UseStaticFiles();

            app.UseCors("CorsConfig");

            app.UseMvc();            

            //Mapping Initialized
            Mapper.Initialize(cfg =>
            {
                MapUtility.ConfigureMapping(cfg);
            });



        }



    }
}
