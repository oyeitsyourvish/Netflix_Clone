using Microsoft.EntityFrameworkCore;
using NetflixClone.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//Registers our DbContext with Dependency Injection.
builder.Services.AddDbContext<ApplicationDbContext>
//Use SQL Server as the database provider.
(options => options.UseSqlServer
//Get the connection string from appsettings.json.
(builder.Configuration.GetConnectionString("DefaultConnection")));

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
