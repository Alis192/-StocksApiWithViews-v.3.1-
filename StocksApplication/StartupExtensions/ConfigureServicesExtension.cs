using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using ServiceContracts;
using Services;
using StocksApplication.Core.Domain.RepositoryContracts;
using StocksApplication.Infrastructure.Repositories;
using System.Globalization;

namespace StocksApiBasics.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllersWithViews();
            services.AddScoped<IStocksRepository, StocksRepository>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.Configure<StocksApiOptions>(configuration.GetSection("TradingOptions")); //we are adding configuration as a service with option type StocksApiOptions 
            services.AddScoped<IFinnhubService, FinnhubService>(); //initializing interface and class in IoC
            services.AddScoped<IStocksGetterService, StocksGetterService>();
            services.AddScoped<IStocksCreaterService, StocksCreaterService>();

            services.AddHttpClient(); //Now httpclient is available in our appliaction
            services.AddMemoryCache(); //adding caching


            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); //means that we are going to use sql server connection
            }, ServiceLifetime.Scoped);


            return services;
            //CONNECTION STRING
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OrdersDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False

        }
    }
}
