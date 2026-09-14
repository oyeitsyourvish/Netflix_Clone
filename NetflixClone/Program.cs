using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Data;
using NetflixClone.Models;

var builder = WebApplication.CreateBuilder(args);

//Registers our DbContext with Dependency Injection.  //UseSQLServer as the database provider. //Get the connection string from appsettings.json.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registers Identity services with Dependency Injection.  //Use ApplicationUser as the user class and IdentityRole as the role class.
//Use Entity Framework to store user and role information in the database.
//Add default token providers for password reset, email confirmation, etc.
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedAsync(services);
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
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();
