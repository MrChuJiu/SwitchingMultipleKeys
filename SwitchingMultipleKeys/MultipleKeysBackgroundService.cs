using Microsoft.Extensions.Hosting;

namespace SwitchingMultipleKeys
{
    public class MultipleKeysBackgroundService: BackgroundService
    {
        private IMultipleKeysProvider<MultipleKeyEntity> multipleKeysProvider;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 86400000
            return Task.Delay(86400000, stoppingToken).ContinueWith(
                t =>
                {
                    multipleKeysProvider.TimingUpdateMultipleKeys();
                    ExecuteAsync(stoppingToken);
                }, stoppingToken);  
        }
    }
}
