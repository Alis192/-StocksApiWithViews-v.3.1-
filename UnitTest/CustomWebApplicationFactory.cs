using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test"); //by default asp.net core project runs in production env

            builder.ConfigureServices(services =>
            {
                //service descriptor represents type of service and its lifetime
                var descriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<OrdersDbContext>));

                if(descriptor != null) //if db context services found in the application
                {
                    services.Remove(descriptor);
                }
                //Now we have to add db context services with Entity framework In-Memory
                //Entity framework In-Memory is an alternative implementation of actual entity framework core with SQL server
                //The difference is SQL server tries to access the real Database
                //But Entity framework In-Memory doesn't do that instead it tries to perform all operations upon an existing In-Memory Collection

                services.AddDbContext<OrdersDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseForTesting");
                });
            });

            //add configuration source for the host builder

            builder.ConfigureAppConfiguration((WebHostBuilderContext ctx, Microsoft.Extensions.Configuration.IConfigurationBuilder config) =>
            {
                var newConfiguration = new Dictionary<string, string>
                {
                    { "userToken", "cfuriqpr01qiqjqkogggcfuriqpr01qiqjqkogh0" } //add token value
                };

                config.AddInMemoryCollection(newConfiguration);
            });
        }
    }
}
