using StocksApiBasics;
using ServiceContracts;
using Services;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Entities;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<StocksApiOptions>(builder.Configuration.GetSection("TradingOptions")); //we are adding configuration as a service with option type StocksApiOptions 
builder.Services.AddScoped<IFinnhubService, FinnhubService>(); //initializing interface and class in IoC
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddHttpClient();
var cultureInfo = new CultureInfo("en-US");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //means that we are going to use sql server connection
});


//CONNECTION STRING
//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OrdersDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False



var app = builder.Build();


Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa"); //set up rotativa package
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();   

app.Run();
