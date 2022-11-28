
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace SwitchingMultipleKeys.SqlServer
{
    public static class MultipleKeysExtensions
    {
        public static void AddMultipleKeysSqlServer(this IServiceCollection services, Action<MultipleKeysOptions> optionsAction = null)
        {
            services.AddScoped(typeof(IMultipleKeysProvider<>), typeof(SqlServerMultipleKeysProvider<>));

            services.AddHostedService<MultipleKeysBackgroundService>();

            services.Configure(optionsAction);

            services.AddTransient<IMultipleKeySeedData,SqlServerMultipleKeySeedData>();
        }

        public static IApplicationBuilder UseMultipleKeysSqlServerSeedData(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var seedData= app.ApplicationServices.GetService<IMultipleKeySeedData>();

            seedData?.SeedAsync().Wait();
            return app;

        }
    }


}
