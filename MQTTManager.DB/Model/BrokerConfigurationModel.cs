using MQTTManager.DB.Model.Enum;

namespace MQTTManager.DB.Model
{
    public class BrokerConfigurationModel : IEquatable<BrokerConfigurationModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Port { get;set; }

        public AuthorizationTypes Authorization { get; set; }
        public bool Default { get; set; }
        public bool RunState { get; set; }
        public string Configuration { get; set; }
        public string? Login { get; set; }
        public string? HashPassword { get; set; }
        public string? Key { get; set; }

        public BrokerConfigurationModel(string name, int port, AuthorizationTypes authorization, bool def, bool runState, string configuration, string login, string hashPassword, string key)
        {
            Name = name;
            Port = port;
            Authorization = authorization;
            Default = def;
            RunState= runState;
            Configuration = configuration;
            Login = login;
            HashPassword = hashPassword;
            Key = key;
        }

        public BrokerConfigurationModel()
        {
        }
        public bool Equals(BrokerConfigurationModel other)
        {
            if (other == null) return false;

            return this.Name == other.Name
                && this.Port == other.Port
                && this.Configuration == other.Configuration;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BrokerConfigurationModel);
        }

        public override int GetHashCode()
        {
            return (Name, Port).GetHashCode();
        }
    }
}
