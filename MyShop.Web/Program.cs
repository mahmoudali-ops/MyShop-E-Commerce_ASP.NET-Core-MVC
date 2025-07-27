using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyShop.DataAccsess.Data;
using MyShop.DataAccsess.DBIInitliazer;
using MyShop.DataAccsess.Implementaion;
using MyShop.Entities.Repositories;
using MyShop.Utilities;
using Stripe;


namespace MyShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
           // StripeConfiguration.ApiKey = "sk_test_YourSecretKeyHere";

            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container. 
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ));
           builder.Services.Configure<StripeKey>(builder.Configuration.GetSection("Stripe"));

            builder.Services.AddIdentity<IdentityUser,IdentityRole>(
                options=>options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromHours(1)
                )
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IDBIInitliazer, DBIInitliazer>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            StripeConfiguration.ApiKey = (builder.Configuration.GetSection("Stripe:SecertKey").Get<string>());
            SeeDDb();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Category}/{action=Index}/{id?}");

         

            app.Run();

            void SeeDDb() 
            {
                using (var scope = app.Services.CreateScope()) 
                {
                    var service = scope.ServiceProvider.GetRequiredService<IDBIInitliazer>();
                    service.Initialize();
                }
            }
        }
    }
}
