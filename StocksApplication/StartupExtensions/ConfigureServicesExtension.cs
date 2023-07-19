using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using ServiceContracts;
using Services;
using StocksApplication.Core.Domain.IdentityEntities;
using StocksApplication.Core.Domain.RepositoryContracts;
using StocksApplication.Infrastructure.Repositories;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using StocksApplication.Core.ServiceContracts;
using StocksApplication.Core.Services;

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
            services.AddScoped<ITopUpService, TopUpService>();
            services.AddScoped<ITopUpRepository, TopUpRepository>();
            services.AddScoped<IUserBalanceUpdate, UserBalanceUpdate>();
            services.AddHttpContextAccessor();

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
            //CONNECTION STRING
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OrdersDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False



            //Enable Identity in this project
            // Initialize and configure the ASP.NET Core Identity system.
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {

                //Password complexity setup
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3; //AB12AB

            }) // Define the ApplicationUser and ApplicationRole classes as the user and role entities for ASP.NET Identity.

                // Configure the Identity system to use our ApplicationDbContext, which includes our application's specific database context, connection string, and configuration settings.
                // This allows the Identity system to interact with the database using the same DbContext as the rest of our application.
                .AddEntityFrameworkStores<OrdersDbContext>()

                // Add a token provider for generating tokens, such as email confirmation tokens and password reset tokens (one-time passwords or OTPs).
                // Default token providers are added for these two purposes and others, but more can be added if needed.
                .AddDefaultTokenProviders()

                // Configure the Identity system to use the UserStore class for persisting user information.
                // UserStore provides methods for creating, retrieving, updating, and deleting users, as well as for adding/removing user roles, verifying passwords, etc.
                // This line specifies that the UserStore should use our custom ApplicationUser class, our custom ApplicationRole class, our ApplicationDbContext, and GUIDs for user IDs.
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, OrdersDbContext, Guid>>()

                // Configure the Identity system to use the RoleStore class for persisting role information.
                // RoleStore provides methods for creating, retrieving, updating, and deleting roles, as well as for adding/removing users from roles.
                // This line specifies that the RoleStore should use our custom ApplicationRole class, our ApplicationDbContext, and GUIDs for role IDs.
                .AddRoleStore<RoleStore<ApplicationRole, OrdersDbContext, Guid>>();


            //It ensures a role for all the users such that in order to access any action method of the entire application the user must be logged in that means the request should have the identity cookie
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //enforces authorization policy (authorization filter will work for all action methods)

                options.AddPolicy("NotAuthorized", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity.IsAuthenticated; //Gives true if user is logged in
                    });
                });
            });


            //Whenever above mentioned policy is not respected by the request that means the user has not submitted the identity cookie as a product of the request, in other words if the user is not logged in, then automatically it has to redirect to this particular URL
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });


            return services;
        }
    }
}
