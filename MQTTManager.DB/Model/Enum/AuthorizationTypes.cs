using System.ComponentModel;

namespace MQTTManager.DB.Model.Enum
{
    public enum AuthorizationTypes
    {
        [Description("Klucz")]
        KEY,
        [Description("Login i hasło")]
        NORMAL,
        [Description("Brak")]
        NONE,
    }
}