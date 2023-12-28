using CNPM_ktxUtc2Store.Areas.Admin.Service;
using CNPM_ktxUtc2Store.Areas.Admin.Service.Impl;
using CNPM_ktxUtc2Store.Data;
using CNPM_ktxUtc2Store.Models;
using CNPM_ktxUtc2Store.Service;
using CNPM_ktxUtc2Store.Service.Impl;
using CNPM_ktxUtc2Store.Setting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddIdentity<applicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddDefaultIdentity<applicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.TryAddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.AddTransient<IHomeService, HomeService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IUserOrderService, UserOrderService>();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddDefaultIdentity<applicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.UseAuthorization();
app.MapAreaControllerRoute(
    name: "areas",
    areaName:"Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
    );


app.MapAreaControllerRoute(
    name: "areas",
    areaName: "Saler",
    pattern: "Saler/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRoleAndAdmin(scope.ServiceProvider);
    await DbSeeder.SeedInfor(scope.ServiceProvider);
    await DbSeeder.SeedSattus(scope.ServiceProvider);
}

app.Run();
