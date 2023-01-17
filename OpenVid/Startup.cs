using CatalogManager;
using Database;
using Database.Models;
using Database.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TagCache;
using VideoHandler;
using VideoHandler.SearchFilters;

namespace OpenVid
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<OpenVidContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DefaultDatabase")));
            services.AddDbContext<UserDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DefaultDatabase")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<UserDbContext>();

            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<ISearchManager, SearchManager>();
            services.AddScoped<IVideoManager, VideoManager>();
            services.AddScoped<IUrlResolver, UrlResolver>();

            services
              .AddScoped<IFilter, GeneralFilter>()
              .AddScoped<IFilter, HashFilter>()
              .AddScoped<IFilter, TagFilter>()
              .AddScoped<IFilter, CategoryFilter>()
              .AddScoped<IFilter, MetaFilter>()
              .AddScoped<IFilter, ExtensionFilter>()
              .AddScoped<IFilter, RatingFilter>()
              .AddScoped<IFilter, RatingOrSaferFilter>()
              .AddScoped<IFilter, RatingOrRiskierFilter>()
              .AddScoped<IFilter, MinDurationFilter>()
              .AddScoped<IFilter, MaxDurationFilter>();

            // Class Library Installers
            services.TagCacheInstaller();
            services.CatalogManagerInstaller(Configuration);

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                //options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<OpenVidContext>().Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();

                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var videoContext = serviceScope.ServiceProvider.GetRequiredService<OpenVidContext>();
                    var videoSuccess = videoContext.Database.EnsureCreated();

                    var userContext = serviceScope.ServiceProvider.GetRequiredService<UserDbContext>();
                    var userSuccess = userContext.Database.EnsureCreated();
                }
            }

            bool.TryParse(Configuration["DevErrors"], out bool devErrors);
            if (!devErrors)
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "defaultareas",
                    pattern: "{area:exists}/{controller=Home}/{id?}",
                    defaults: new { action = "Index" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    defaults: new { action = "Index" },
                    pattern: "{controller=Home}/{id?}");
            });
        }
    }
}
