using Microsoft.Extensions.DependencyInjection;

namespace SwitchingMultipleKeys.SqlServer
{
    public static class MultipleKeysExtensions
    {
        public static void AddMultipleKeysSqlServer(this IServiceCollection services, Action<MultipleKeysOptions> optionsAction = null)
        {

            //services.AddSingleton<SqlServerMultipleKeyContext>();
            services.AddSingleton(typeof(IMultipleKeysProvider<>), typeof(SqlServerMultipleKeysProvider<>));

            services.Configure(optionsAction);
        }

    }


}
