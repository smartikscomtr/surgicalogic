﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Services;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Data.Migrations.Initialize;
using Surgicalogic.Data.Utilities;
using Surgicalogic.Services.Common;
using Surgicalogic.Services.Stores;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

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

            // services.AddIdentity<User, IdentityRole>(options =>
            // {
            //     options.User.RequireUniqueEmail = true;
            // })
            //.AddEntityFrameworkStores<DataContext>()
            //.AddDefaultTokenProviders();

            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
               .AddEntityFrameworkStores<DataContext>()
               .AddDefaultTokenProviders();

            //remove default claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["AppSettings:Token:Issuer"],
                        ValidAudience = Configuration["AppSettings:Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Token:SecurityKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire

                    };
                });

            services.AddTransient<IAppServiceProvider, AppServiceProvider>();
            services.AddTransient<ITokenService, TokenService>();

            #region Applicaiton Cookie Settings

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            #endregion

            #region CROS Settings

            //CROS service registerd. This methode was add besause of allow-control-access-origin
            services.AddCors(options =>
                options.AddPolicy("CorsConfig",
                    builder =>
                        builder.WithOrigins(Configuration["AppSettings:Http:AllowedOrigin"])
                       .AllowAnyMethod()
                       .AllowAnyHeader())
            );

            #endregion

            services.AddTransient<IAppServiceProvider, AppServiceProvider>();

            #region StoreService Registeration
            services.AddScoped<IAppointmentCalendarStoreService, AppointmentCalendarStoreService>();
            services.AddScoped<IBranchStoreService, BranchStoreService>();
            services.AddScoped<IDoctorCalendarStoreService, DoctorCalendarStoreService>();
            services.AddScoped<IEquipmentStoreService, EquipmentStoreService>();
            services.AddScoped<IEquipmentTypeStoreService, EquipmentTypeStoreService>();
            services.AddScoped<IFeedbackStoreService, FeedbackStoreService>();
            services.AddScoped<IOperationStoreService, OperationStoreService>();
            services.AddScoped<IOperationBlockedOperatingRoomStoreService, OperationBlockedOperatingRoomStoreService>();
            services.AddScoped<IOperationPersonnelStoreService, OperationPersonnelStoreService>();
            services.AddScoped<IOperationPlanStoreService, OperationPlanStoreService>();
            services.AddScoped<IOperationPlanHistoryStoreService, OperationPlanHistoryStoreService>();
            services.AddScoped<IOperatingRoomCalendarStoreService, OperatingRoomCalendarStoreService>();
            services.AddScoped<IOperatingRoomEquipmentStoreService, OperatingRoomEquipmentStoreService>();
            services.AddScoped<IOperatingRoomOperationTypeStoreService, OperatingRoomOperationTypeStoreService>();
            services.AddScoped<IOperatingRoomStoreService, OperatingRoomStoreService>();
            services.AddScoped<IOperationTypeStoreService, OperationTypeStoreService>();
            services.AddScoped<IOperationTypeEquipmentStoreService, OperationTypeEquipmentStoreService>();
            services.AddScoped<IPatientStoreService, PatientStoreService>();
            services.AddScoped<IPersonnelStoreService, PersonnelStoreService>();
            services.AddScoped<IPersonnelBranchStoreService, PersonnelBranchStoreService>();
            services.AddScoped<IPersonnelTitleStoreService, PersonnelTitleStoreService>();
            services.AddScoped<ISettingStoreService, SettingStoreService>();
            services.AddScoped<ISettingDataTypeStoreService, SettingDataTypeStoreService>();
            services.AddScoped<IUserStoreService, UserStoreService>();
            services.AddScoped<IWorkTypeStoreService, WorkTypeStoreService>();

            #endregion

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //StaticFile Allowance Initialized
            app.UseStaticFiles();
            app.UseCors("CorsConfig");
            app.UseAuthentication();

            AuthAppBuilderExtensions.UseAuthentication(app);

            //Mapping Initialized
            Mapper.Initialize(cfg =>
            {
                MapUtility.ConfigureMapping(cfg);
            });

            if (Convert.ToBoolean(Configuration["AppSettings:Migration:DbSeed"]))
            {
                DbInitializer.Seed(context);
            }

            BuildAppSettings();
            app.UseMvc();
        }

        private void BuildAppSettings()
        {
            AppSettings.TokenSecurityKey = Configuration["AppSettings:Token:SecurityKey"];
            AppSettings.TokenValidityPeriodInMinutes = Configuration["AppSettings:Token:ValidityPeriodInMinutes"].ToNCInt();
            AppSettings.Issuer = Configuration["AppSettings:Token:Issuer"];
            AppSettings.ApiBaseUrl = Configuration["AppSettings:Planning:ApiBaseUrl"];
            AppSettings.ApiPostUrl = Configuration["AppSettings:Planning:ApiPostUrl"];
            AppSettings.AdminRole = Configuration["AppSettings:Role:Admin"];
            AppSettings.MemberRole = Configuration["AppSettings:Role:Member"];
            AppSettings.DoctorId = Configuration["AppSettings:General:DoctorId"].ToNCInt();
            AppSettings.DoctorPicture = Configuration["AppSettings:DoctorPicture:Url"];
        }
    }
}