using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using GameDrop.Data;
using Microsoft.AspNetCore.Identity;
using GameDrop.Areas.Identity.Data;
using GameDrop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameDropDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("GameDropConnectionString")));
builder.Services.AddDbContext<GameDropContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameDropConnectionString")));

builder.Services.AddDefaultIdentity<GameDropUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GameDropContext>();

builder.Services.AddHttpClient();   
builder.Services.AddHttpClient<DateTimeService>(); // Register HttpClient for date time service

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapControllers(); // Ensure API controllers are mapped

app.Run();
