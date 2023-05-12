using StocksApiBasics;
using ServiceContracts;
using Services;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repositories;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();
builder.Services.Configure<StocksApiOptions>(builder.Configuration.GetSection("TradingOptions")); //we are adding configuration as a service with option type StocksApiOptions 
builder.Services.AddScoped<IFinnhubService, FinnhubService>(); //initializing interface and class in IoC
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddHttpClient(); //Now httpclient is available in our appliaction
builder.Services.AddMemoryCache(); //adding caching


var cultureInfo = new CultureInfo("en-US");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //means that we are going to use sql server connection
}, ServiceLifetime.Scoped);


//CONNECTION STRING
//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OrdersDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False


var app = builder.Build();

if (builder.Environment.IsEnvironment("Test") == false)
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa"); //set up rotativa package
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();   

app.Run();

public partial class Program { } //make the auto-generated Program accessible programatically
//if we don't do that Program class will be generated automatically by the compiler but as a developer we will not have an access to that