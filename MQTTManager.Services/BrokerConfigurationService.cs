using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTTManager.DB;
using MQTTManager.DB.Model;

namespace MQTTManager.Services
{
    public class BrokerConfigurationService : IBrokerConfigurationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BrokerConfigurationService> _logger;

        public BrokerConfigurationService(ApplicationDbContext context, ILogger<BrokerConfigurationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddBrokerConfiguration(BrokerConfigurationModel brokerConfig)
        {
            try
            {
                _context.BrokerConfig.Add(brokerConfig);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with adding BrokerConfiguration");
                return false;
            }
        }

        public async Task<bool> RemoveBrokerConfiguration(BrokerConfigurationModel brokerConfig)
        {
            try
            {
                _context.BrokerConfig.Remove(brokerConfig);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with remove BrokerConfiguration");
                return false;
            }
        }

        public async Task<bool> RemoveBrokerConfigurationById(int id)
        {
            try
            {
                var config = await _context.BrokerConfig.FirstOrDefaultAsync(b => b.Id == id);
                if (config != null)
                {
                    _context.BrokerConfig.Remove(config);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogWarning($"No BrokerConfiguration found with id: {id}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Problem with deleting BrokerConfiguration with id: {id}");
                return false;
            }
        }

        public async Task<bool> SetAsDefaultBrokerConfigurationById(int id)
        {
            try
            {
                var oldDefault = await _context.BrokerConfig.SingleOrDefaultAsync(b => b.Default == true);
                var config = await _context.BrokerConfig.FirstOrDefaultAsync(b => b.Id == id);

                if (config != null)
                {
                    config.Default = true;

                    if (oldDefault != null)
                    {
                        oldDefault.Default = false;
                    }

                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    _logger.LogWarning("No BrokerConfiguration found with id: {Id}", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with updating BrokerConfiguration with id: {Id}", id);
                return false;
            }
        }

        public async Task<bool> SetStateAfterRunBrokerConfigurationById(int id, bool state)
        {
            try
            {
                var config = await _context.BrokerConfig.FirstOrDefaultAsync(b => b.Id == id);

                if (config != null)
                {
                    config.RunState = state;

                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    _logger.LogWarning("No BrokerConfiguration found with id: {Id}", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with updating BrokerConfiguration with id: {Id}", id);
                return false;
            }
        }

        public async Task<bool> UpdateBrokerConfiguration(BrokerConfigurationModel brokerConfig)
        {
            try
            {
                var config = await _context.BrokerConfig.FirstOrDefaultAsync(b => b.Id == brokerConfig.Id);
                if (config != null)
                {
                    config.Name = brokerConfig.Name;
                    config.Port = brokerConfig.Port;
                    config.Authorization = brokerConfig.Authorization;
                    config.Default = brokerConfig.Default;

                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogWarning("No BrokerConfiguration found with id: {Id}", brokerConfig.Id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with updating BrokerConfiguration with id: {Id}", brokerConfig.Id);
                return false;
            }
        }

        public async Task<BrokerConfigurationModel> GetBrokerConfigurationById(int id)
        {
            try
            {
                return await _context.BrokerConfig.FirstOrDefaultAsync(b => b.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with getting BrokerConfiguration");
                return null;
            }
        }

        public async Task<BrokerConfigurationModel> GetDefaultBrokerConfiguration()
        {
            try
            {
                return await _context.BrokerConfig.FirstOrDefaultAsync(b => b.Default == true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with getting BrokerConfiguration");
                return null;
            }
        }

        public async Task<List<BrokerConfigurationModel>> GetBrokerConfigurationList()
        {
            try
            {
                return await _context.BrokerConfig.OrderByDescending(b => b.Default).ThenBy(b => b.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with getting list from db");
                return new List<BrokerConfigurationModel>();
            }
        }

    }
}