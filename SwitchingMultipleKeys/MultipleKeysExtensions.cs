using Microsoft.Extensions.DependencyInjection;

namespace SwitchingMultipleKeys
{
    public static class MultipleKeysExtensions
    {
        public static void AddMultipleKeys(this IServiceCollection services, Action<MultipleKeysOptions> optionsAction = null)
        {

            services.AddSingleton(typeof(IMultipleKeysProvider<>), typeof(DefaultMultipleKeysProvider<>));

            services.Configure(optionsAction);
        }

    }
}
