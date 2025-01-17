using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ecf2.Data;
using ecf2.Models;
using ecf2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ecf2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ecf2Context") ?? throw new InvalidOperationException("Connection string 'ecf2Context' not found.")));

builder.Services.Configure<EcfDatabaseSettings>(
    builder.Configuration.GetSection("ecfDatabase"));

builder.Services.AddSingleton<StatsService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Evenement}/{action=Index}/{id?}");

app.Run();
