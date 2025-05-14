using System.IO;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Hubs;          // for ChatHub
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

// ───── 4) SignalR ─────
builder.Services.AddSignalR();

// ───── 5) MVC + Razor Pages ─────
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ───── 6) Middleware ─────
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

// ───── 7) Map your SignalR hubs ─────
app.MapHub<ChatHub>("/chathub");

// ───── 8) Seed Roles if not exist ─────
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

// ───── 9) Routes ─────
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();

// ───── 10) Force login before accessing app ─────
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var isPublic =
     path.StartsWithSegments("/Identity/Account/Login")
  || path.StartsWithSegments("/Identity/Account/Register")
  || path.StartsWithSegments("/Login")                         // this line is required
  || path.StartsWithSegments("/Register")                      // allow registration
  || path.StartsWithSegments("/css")
  || path.StartsWithSegments("/js")
  || path.StartsWithSegments("/lib")
  || path.StartsWithSegments("/images");


    if (!context.User.Identity.IsAuthenticated && !isPublic)
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }

    await next();
});

app.Run();
