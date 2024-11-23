using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using GameDrop.Data;
using Microsoft.AspNetCore.Identity;
using GameDrop.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameDropDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("GameDropConnectionString")));
builder.Services.AddDbContext<GameDropContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameDropConnectionString")));

builder.Services.AddDefaultIdentity<GameDropUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<GameDropContext>();

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

app.Run();
