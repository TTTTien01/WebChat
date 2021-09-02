using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webchat.Entities;
using Webchat.Hubs;

namespace Webchat
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
            services.AddSignalR();
            services.AddDbContext<WebChatDbContext>(options => // cau hinh,,phạm vi truy cập 1 ng dùng 
            {
                options.UseSqlServer(Configuration.GetConnectionString("WebChat"));///note
            });
            services.AddAuthentication("Cookies")
                .AddCookie(options =>
                {
                    options.LoginPath = "/dang-nhap";// đường dẫn trang đăng nhập
                    options.ExpireTimeSpan = TimeSpan.FromHours(6);//tự đăng xuất sau 6h
                    options.Cookie.HttpOnly = true;//lí do bảo mật
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();///thêm
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
            endpoints.MapHub<ChatHub>("/chat");
            //Map URL
            endpoints.MapControllerRoute(
                     name: "signup",
                    pattern: "dang-ky",
                    defaults: new { controller = "AppUser", action = "SignUp"});
              //ỦRL MỚI cho trang Đăng nhập
                endpoints.MapControllerRoute(
                     name: "login",
                    pattern: "dang-nhap",
                    defaults: new { controller = "AppUser", action = "Login" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
