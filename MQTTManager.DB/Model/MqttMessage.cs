using MQTTnet;

namespace MQTTManager.DB.Model
{
    public class MqttMessage
    {
        public MqttApplicationMessage BaseMessage { get; set; }
        public DateTime MessageDate { get; set; }

        public MqttMessage()
        {
            BaseMessage = new MqttApplicationMessage();
            MessageDate = DateTime.Now;
        }
    }
}
