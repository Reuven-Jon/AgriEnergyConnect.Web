using System.IO;
using AgriEnergyConnect.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ───── 1) Set DataDirectory for SQL file attach ─────
var basePath = AppContext.BaseDirectory;
AppDomain.CurrentDomain.SetData("DataDirectory",
    Path.Combine(basePath, "..", "..", "App_Data"));

// ───── 2) Register EF Core with SQL Server ─────
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ───── 3) Identity + Roles + Default UI ─────
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// ───── 4) Razor Pages + MVC ─────
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ───── 5) Middleware ─────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ───── 6) Seed Roles if not exist ─────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    db.Database.Migrate();

    foreach (var role in new[] { "Farmer", "Employee", "Unassigned" })
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// ───── 7) Default route goes to Login page ─────
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages(); // REQUIRED for /Areas Razor Pages

// ───── 8) Force login before accessing app ─────
app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    var isPublicPage = path.StartsWithSegments("/Identity/Account/Login")
                    || path.StartsWithSegments("/Identity/Account/Register")
                    || path.StartsWithSegments("/css")
                    || path.StartsWithSegments("/js")
                    || path.StartsWithSegments("/lib")
                    || path.StartsWithSegments("/images");

    if (!context.User.Identity.IsAuthenticated && !isPublicPage)
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }

    await next();
});

app.Run();
