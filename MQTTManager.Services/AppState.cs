using MQTTManager.DB.Model;
using MQTTManager.DB.Model.Enum;

namespace MQTTManager.Services
{
    public class AppState
    {
        public event Action OnBrokerChanged;
        public event Action OnBrokerStatus;
        public event Action OnNewMessage;

        private StateTypes _brokerStatus = StateTypes.STOPPED;
        private List<string> _subscriptionTopics = new List<string>();
        private List<string> _allTopics = new List<string>();
        private List<string> _blackList = new List<string>();

        public StateTypes BrokerStatus
        {
            get => _brokerStatus;
            set
            {
                if (_brokerStatus != value)
                {
                    _brokerStatus = value;
                    OnBrokerStatus?.Invoke();
                }
            }
        }

        private int _brokerMessagesCount = 0;
        public int BrokerMessagesCount
        {
            get => _brokerMessagesCount;
            set
            {
                if (_brokerMessagesCount != value)
                {
                    _brokerMessagesCount = value;
                    OnBrokerChanged?.Invoke();
                    OnNewMessage?.Invoke();
                }
            }
        }

        public List<string> SubscriptionTopics
        {
            get => _subscriptionTopics;
            set
            {
                if (_subscriptionTopics != value)
                {
                    _subscriptionTopics = value;
                    //OnNewMessage?.Invoke();
                }
            }
        }

        public List<string> AllTopics
        {
            get => _allTopics;
            set
            {
                if (_allTopics != value)
                {
                    _allTopics = value;
                }
            }
        }

        public List<string> BlackList
        {
            get => _blackList;
            set
            {
                if (_blackList != value)
                {
                    _blackList = value;
                }
            }
        }

        private int _connectedUsersCount = 0;
        public int ConnectedUsersCount
        {
            get => _connectedUsersCount;
            set
            {
                if (_connectedUsersCount != value)
                {
                    _connectedUsersCount = value;
                    OnBrokerChanged?.Invoke();
                }
            }
        }
    }
}
