using AspNetIdentity.CustomValidators;
using AspNetIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AspNetIdentity
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //DbContext Tanımlanıyor. ConnectionString 'appsettings.json' içerisinden alýnýyor.
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(_configuration["ConnectionStrings:Default"]);
            });

            //Asp.Net Identity kütüphanesi ekleniyor.
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcçdefghýijklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSTUÜVWXYZ0123456789-._";

                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddPasswordValidator<CustomPasswordValidator>()
            .AddUserValidator<CustomUserValidator>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

            //Authentication için Cookie Ayarlarý
            CookieBuilder cookieBuilder = new CookieBuilder()
            {
                Name = "MyBlog",
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                SecurePolicy = CookieSecurePolicy.SameAsRequest
            };

            //Authentication için ayarlanan cookie'nin sisteme eklenmesi
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Home/LogIn");
                options.LogoutPath = new PathString("/Member/LogOut");
                options.Cookie = cookieBuilder;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = new PathString("/Member/AccessDenied");
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
