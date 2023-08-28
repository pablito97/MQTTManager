using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTManager.Services
{
    public class MqttHostedService : IHostedService
    {
        private readonly IMqttBrokerService _mqttBrokerService;
        private readonly IServiceProvider _serviceProvider;

        public MqttHostedService(IMqttBrokerService mqttBrokerService, IServiceProvider serviceProvider)
        {
            _mqttBrokerService = mqttBrokerService;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var brokerConfigService = scope.ServiceProvider.GetRequiredService<IBrokerConfigurationService>();
                var defaultBroker = await brokerConfigService.GetDefaultBrokerConfiguration();

                if (defaultBroker != null && defaultBroker.RunState)
                {
                    await _mqttBrokerService.StartBrokerAsync(defaultBroker);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // W razie potrzeby dodaj logikę czyszczenia podczas zatrzymywania aplikacji.
            return Task.CompletedTask;
        }
    }
}