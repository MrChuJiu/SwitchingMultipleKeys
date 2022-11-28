using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SwitchingMultipleKeys
{
    public class MultipleKeysBackgroundService: BackgroundService
    {
   
        private readonly IServiceProvider _serviceProvider;

        public MultipleKeysBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 86400000
            return Task.Delay(86400000, stoppingToken).ContinueWith(
                t =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var multipleKeysProvider = scope.ServiceProvider.GetService<IMultipleKeysProvider<MultipleKeyEntity>>();
                    multipleKeysProvider?.TimingUpdateMultipleKeys();

                    ExecuteAsync(stoppingToken);
                }, stoppingToken);  
        }
    }
}
