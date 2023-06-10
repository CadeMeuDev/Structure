using Microsoft.EntityFrameworkCore;
using Structure.WebAppMvc.Data;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var connectionsString = builder.Configuration.GetConnectionString("Postgresql")
    ?? throw new InvalidOperationException("Connection string 'postgres' not found.");

services.AddDbContext<StructureContext>(options =>
    options.UseNpgsql(connectionsString,
    m => m.MigrationsHistoryTable("__EFMigrationsHistory", "structure")));

services.AddControllersWithViews();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<StructureContext>();
    dataContext.Database.Migrate();
}

app.Run();