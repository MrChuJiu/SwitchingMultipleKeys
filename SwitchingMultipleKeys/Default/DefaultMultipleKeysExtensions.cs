using Microsoft.Extensions.DependencyInjection;

namespace SwitchingMultipleKeys.Default
{
    public static class DefaultMultipleKeysExtensions
    {
        public static void AddMultipleKeys(this IServiceCollection services, Action<MultipleKeysOptions> optionsAction = null)
        {

            services.AddSingleton(typeof(IMultipleKeysProvider<>), typeof(DefaultMultipleKeysProvider<>));

            services.AddHostedService<MultipleKeysBackgroundService>();

            services.Configure(optionsAction);
        }

    }
}
