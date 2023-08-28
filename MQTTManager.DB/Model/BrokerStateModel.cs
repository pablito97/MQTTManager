using MQTTManager.DB.Model.Enum;

namespace MQTTManager.DB.Model
{
    public class BrokerStateModel
    {
        public int Id { get; set; }
        public StateTypes State { get; set; }
    }
}
