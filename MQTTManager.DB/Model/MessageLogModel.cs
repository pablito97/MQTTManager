namespace MQTTManager.DB.Model
{
    public class MessageLogModel
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Payload { get; set; }
        public DateTime Time { get; set; }
    }
}
