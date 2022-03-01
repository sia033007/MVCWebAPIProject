using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyMVCProject.Models;
using Microsoft.AspNetCore.Identity;
using MyMVCProject.Data;
using Microsoft.EntityFrameworkCore;
using MyMVCProject.Mapping;
using MyMVCProject.Settings;

namespace MyMVCProject
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
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options=> options.UseSqlServer
            (Configuration.GetConnectionString("DefaultConnection")));
            //Data Source = SQL5101.site4now.net,1433; Initial Catalog = db_a82996_amindatabase; User Id = db_a82996_amindatabase_admin; Password = Amin3510258852
            services.AddHttpClient("MyClient", options=> {

                options.BaseAddress = new Uri("http://localhost:42045/api/");


            });
            services.AddIdentity<MyUser, IdentityRole>(options=> {

                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.SignIn.RequireConfirmedEmail = true;
            
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(typeof(UserMapper));
            services.ConfigureApplicationCookie(options=> {

                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LoginPath = "/Home/LoginRequired";
            });
            services.Configure<SmtpSettings>(Configuration.GetSection("SMTP"));
            services.AddScoped<IEmailSender, EmailSender>();
            //services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            //{
            //    options.Cookie.Name = "MyCookie";
            //    options.AccessDeniedPath = "/Home/AccessDenied";
            //    options.LoginPath = "/Home/LoginRequired";
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);


            //});
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
            app.UseAuthentication();

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
