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

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<GameDropUser>>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//
//    string[] roleNames = { "Admin", "User", "Manager" };
//    foreach (var roleName in roleNames)
//    {
//        if (!await roleManager.RoleExistsAsync(roleName))
//        {
//            await roleManager.CreateAsync(new IdentityRole(roleName));
//        }
//    }
//
//    var adminUser = await userManager.FindByEmailAsync("email");
//    if (adminUser != null)
//    {
//        await userManager.AddToRoleAsync(adminUser, "Admin");
//    }
//}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<GameDropContext>();
    var userManager = services.GetRequiredService<UserManager<GameDropUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    context.Database.EnsureCreated();
    await SeedDatabase(context, userManager, roleManager);
}

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

async Task SeedDatabase(GameDropContext context, UserManager<GameDropUser> userManager, RoleManager<IdentityRole> roleManager)
{
    // Initialize default identity roles
    List<IdentityRole> identityRoles = new List<IdentityRole>
    {
        new IdentityRole { Name = RoleTypes.Admin },
    };

    foreach (var role in identityRoles)
    {
        if (!await roleManager.RoleExistsAsync(role.Name))
        {
            await roleManager.CreateAsync(role);
        }
    }

    // Initialize default user
    var admin = new GameDropUser
    {
        Email = "admin@admin.com",
        UserName = "Admin"
    };

    var user = await userManager.FindByEmailAsync(admin.Email);
    if (user == null)
    {
        await userManager.CreateAsync(admin, "1Admin!");
        await userManager.AddToRoleAsync(admin, RoleTypes.Admin);
    }

    // Add code to initialize context tables
}