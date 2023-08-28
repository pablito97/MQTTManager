using MQTTManager.DB.Model;
using MQTTnet.Server;
using MQTTnet;
using MQTTManager.DB.Model.Enum;

namespace MQTTManager.Services
{
    public interface IMqttBrokerService
    {
        Task StartBrokerAsync(BrokerConfigurationModel broker);
        Task StopBrokerAsync();
        Task RestartBrokerAsync(BrokerConfigurationModel broker);
        Task<List<string>> GetConnectedClients();
        Task<int> GetConnectedClientsCount();
        Task <StateTypes> IsBrokerRunning();
        Task<List<MqttMessage>> GetAllMessages();
        Task SendMessage(string topic, int qos, string payload);
        Task DisconnectClientAsync(string clientId);
    }
}