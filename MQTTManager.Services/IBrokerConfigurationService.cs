using MQTTManager.DB.Model;

namespace MQTTManager.Services
{
    public interface IBrokerConfigurationService
    {
        Task<BrokerConfigurationModel> GetBrokerConfigurationById(int id);
        Task<BrokerConfigurationModel> GetDefaultBrokerConfiguration();
        Task<List<BrokerConfigurationModel>> GetBrokerConfigurationList();
        Task<bool> AddBrokerConfiguration(BrokerConfigurationModel brokerConfig);
        Task<bool> UpdateBrokerConfiguration(BrokerConfigurationModel brokerConfig);
        Task<bool> RemoveBrokerConfiguration(BrokerConfigurationModel brokerConfig);
        Task<bool> RemoveBrokerConfigurationById(int id);
        Task<bool> SetAsDefaultBrokerConfigurationById(int id);
        Task<bool> SetStateAfterRunBrokerConfigurationById(int id, bool state);
    }
}