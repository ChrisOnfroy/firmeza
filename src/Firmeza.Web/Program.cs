using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "The PostgreSQL connection string is missing. " +
        "Configure ConnectionStrings:DefaultConnection in appsettings.json, " +
        "appsettings.Development.json, User Secrets, or environment variables.");
}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FirmezaDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FirmezaDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();