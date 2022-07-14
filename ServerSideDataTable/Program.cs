using Microsoft.Extensions.DependencyInjection;
using ServerSideDataTable.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Register GTContext..!!
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddTransient<TestDBContext>();
builder.Services.AddTransient<TestDBContext>(provider =>
{
    //resolve another classes from DI
    //var anyOtherClass = provider.GetService<AnyOtherClass>();

    //pass any parameters
    return new TestDBContext(configuration.GetConnectionString("BloggingDatabase"));
});

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
