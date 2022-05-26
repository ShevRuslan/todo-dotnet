using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TODOJava.Models;

var builder = WebApplication.CreateBuilder(args);
string con = "Server=(localdb)\\mssqllocaldb;Database=tododb;Trusted_Connection=True;";
// Add services to the container.
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(con));
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
*/
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});
app.MapFallbackToFile("index.html"); 
app.Run();
