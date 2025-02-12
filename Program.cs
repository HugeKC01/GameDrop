using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using GameDrop.Data;
using Microsoft.AspNetCore.Identity;
using GameDrop.Areas.Identity.Data;
using GameDrop.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameDropDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("GameDropConnectionString")));
builder.Services.AddDbContext<GameDropContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameDropConnectionString")));

builder.Services.AddDefaultIdentity<GameDropUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GameDropContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<PromoBannerService>();
builder.Services.AddScoped<ShoppingCartService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

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

builder.Services.AddHttpClient();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();