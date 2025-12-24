using Microsoft.EntityFrameworkCore;
using RestaurantMenuManager.Models;
using RestoranMenuYonetimSistemi.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddSession();

// DbContext (SQLite – Render uyumlu)
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlite("Data Source=/app/restaurantmenu.db"));

var app = builder.Build();

// =====================
// M I G R A T I O N
// =====================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
    db.Database.Migrate();
}

// =====================
// MIDDLEWARE
// =====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

// =====================
// ROUTE
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();
