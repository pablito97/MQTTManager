using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using MQTTManager.DB.Model;
using MQTTnet.Protocol;
using MQTTManager.DB.Model.Enum;
using MQTTnet.Packets;
using System.Text;
using MQTTnet.Client;
using Microsoft.EntityFrameworkCore;

namespace MQTTManager.Services
{
    public class MqttBrokerService : IMqttBrokerService
    {
        private readonly ILogger<MqttBrokerService> _logger;
        private readonly AppState _appState;
        private MqttServer _mqttServer;
        private List<string> _connectedClients = new List<string>();
        private List<MqttMessage> _brokerMessages = new List<MqttMessage>();
        private bool _isBrokerRunning = false;
        private BrokerConfigurationModel defaultBroker = new BrokerConfigurationModel();

        public MqttBrokerService(ILogger<MqttBrokerService> logger, AppState appState)
        {
            _logger = logger;
            _appState = appState;
        }

        public async Task StartBrokerAsync(BrokerConfigurationModel broker)
        {
            try
            {
                defaultBroker = broker;

                var mqttFactory = new MqttFactory();

                var mqttServerOptions = new MqttServerOptionsBuilder()
                    .WithDefaultEndpoint()
                    .WithDefaultEndpointPort(broker.Port)
                    .Build();

                _mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions);

                await _mqttServer.StartAsync();

                await ConnectingBrokerUsers(broker);
                await TakeTopicsAndPayloads(broker);
                //await ServerClientConnect();

                _isBrokerRunning = true;
                _appState.BrokerStatus = StateTypes.RUNNING;
                _logger.LogInformation("MQTT broker started on port {0}.", broker.Port);
            }
            catch (Exception ex)
            {
                _appState.BrokerStatus = StateTypes.DAMAGE;
                _logger.LogError("Failed to start MQTT broker.", ex);
                throw;
            }
        }

        public async Task StopBrokerAsync()
        {
            if (_mqttServer != null)
            {
                await _mqttServer.StopAsync();
                _mqttServer?.Dispose();
                _isBrokerRunning = false;
                _connectedClients.Clear();
                _brokerMessages.Clear();
                _appState.BrokerStatus = StateTypes.STOPPED;
                _logger.LogInformation("MQTT broker stop.");
            }
        }

        public async Task RestartBrokerAsync(BrokerConfigurationModel broker)
        {
            await StopBrokerAsync();
            await Task.Delay(1000);
            await StartBrokerAsync(broker);
        }

        public async Task<List<string>> GetConnectedClients()
        {
            return _connectedClients;
        }

        public async Task<int> GetConnectedClientsCount()
        {
            return _connectedClients.Count;
        }

        public async Task<StateTypes> IsBrokerRunning()
        {
            if (_isBrokerRunning) { return StateTypes.RUNNING; }
            else { return StateTypes.STOPPED; }
        }

        public async Task ConnectingBrokerUsers(BrokerConfigurationModel broker)
        {
            _mqttServer.ValidatingConnectionAsync += e =>
            {
                if (!_appState.BlackList.Contains(e.ClientId))
                {
                    if (broker.Authorization == AuthorizationTypes.NORMAL)
                    {
                        if (e.UserName != broker.Login)
                        {
                            e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            _logger.LogInformation($"Bad user name or password");
                        }

                        if (e.Password != broker.HashPassword)
                        {
                            e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            _logger.LogInformation($"Bad user name or password");
                        }

                    }
                    else if (broker.Authorization == AuthorizationTypes.KEY)
                    {
                        if (e.Password != broker.Key)
                        {
                            e.ReasonCode = MqttConnectReasonCode.NotAuthorized;
                            _logger.LogInformation($"Not authorized");
                        }
                    }
                }
                else
                {
                    e.ReasonCode = MqttConnectReasonCode.Banned;
                    _logger.LogInformation($"Client banned");
                }

                return Task.CompletedTask;
            };


            _mqttServer.ClientConnectedAsync += e =>
            {
                _connectedClients.Add(e.ClientId);
                _logger.LogInformation($"Client connected: {e.ClientId}");
                _appState.ConnectedUsersCount = _connectedClients.Count;

                return Task.CompletedTask;
            };

            _mqttServer.ClientDisconnectedAsync += e =>
            {
                _connectedClients.Remove(e.ClientId);
                
                _logger.LogInformation($"Client {e.ClientId} disconnected.");

                _appState.ConnectedUsersCount = _connectedClients.Count;

                return Task.CompletedTask;
            };

        }

        public async Task TakeTopicsAndPayloads(BrokerConfigurationModel broker)
        {
            _mqttServer.ApplicationMessageNotConsumedAsync += (e) =>
            {
                var topic = e.ApplicationMessage.Topic;

                _brokerMessages.Add(new MqttMessage
                {
                    BaseMessage = e.ApplicationMessage,
                    MessageDate = DateTime.Now
                });
                    
                if (!_appState.AllTopics.Contains(topic)) _appState.AllTopics.Add(topic);

                if (_appState.BrokerMessagesCount != _brokerMessages.Count) _appState.BrokerMessagesCount = _brokerMessages.Count;

                _logger.LogInformation($"New mmessage: {topic}");

                return Task.CompletedTask;
            };
        }

        public async Task SendMessage(string topic, int qos, string payload) 
        {
            MqttApplicationMessage message = new MqttApplicationMessage()
            {
                Topic = topic,
                QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce,
                PayloadSegment = Encoding.UTF8.GetBytes(payload),
            };

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer($"127.0.0.1")
                    .WithClientId("serverClient")
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                await mqttClient.PublishAsync(message, CancellationToken.None);

                await mqttClient.DisconnectAsync();

                Console.WriteLine("MQTT application message is published.");
            }

            _logger.LogInformation($"Send mmessage");
        }

        public async Task<List<MqttMessage>> GetAllMessages()
        {
            return _brokerMessages.OrderByDescending(b => b.MessageDate).ToList();
        }

        public async Task DisconnectClientAsync(string clientId)
        {
            if (_mqttServer != null)
            {
                await _mqttServer.DisconnectClientAsync(clientId, MqttDisconnectReasonCode.AdministrativeAction);
                _connectedClients.Remove(clientId);
                _logger.LogInformation($"Client disconnected manualy: {clientId}");
            }
        }
    }
}
