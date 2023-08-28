using System.ComponentModel;

namespace MQTTManager.DB.Model.Enum
{
    public enum StateTypes
    {
        [Description("Pracuje")]
        RUNNING,
        [Description("Zatrzymany")]
        STOPPED,
        [Description("Awaria")]
        DAMAGE,
    }
}