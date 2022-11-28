using Microsoft.Extensions.Hosting;

namespace SwitchingMultipleKeys
{
    public class MultipleKeysBackgroundService: BackgroundService
    {
        private IMultipleKeysProvider<MultipleKeyEntity> multipleKeysProvider;

        public MultipleKeysBackgroundService(IMultipleKeysProvider<MultipleKeyEntity> multipleKeysProvider)
        {
            this.multipleKeysProvider = multipleKeysProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 86400000
            return Task.Delay(1000, stoppingToken).ContinueWith(
                t =>
                {
                    multipleKeysProvider.TimingUpdateMultipleKeys();
                    ExecuteAsync(stoppingToken);
                }, stoppingToken);  
        }
    }
}
