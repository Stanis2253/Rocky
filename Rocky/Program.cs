using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky_DataAccess.Repository;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Utility;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddDefaultTokenProviders()
            .AddDefaultUI()
            .AddEntityFrameworkStores<ApplicationDbContext>();


        builder.Services.AddTransient<IEmailSender,EmailSender>();

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSession(Options =>
        {
            Options.IdleTimeout = TimeSpan.FromMinutes(10);
            Options.Cookie.HttpOnly = true;
            Options.Cookie.IsEssential = true;
        });

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(

            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        app.Run();
    }
}