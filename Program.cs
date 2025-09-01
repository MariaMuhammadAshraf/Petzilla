using AptechVisionPetZilla.Data;
using Microsoft.EntityFrameworkCore;

namespace AptechVisionPetZilla
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            /////database
            builder.Services.AddDbContext<PetzillaContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

            //class 5
            //session services 
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //CACHE
            builder.Services.AddDistributedMemoryCache();

            //session destroy timing 
            builder.Services.AddSession();


            var app = builder.Build();

            // session call
            app.UseSession();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
