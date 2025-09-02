using System.Text.Json.Serialization;

namespace BGT_Web_Boardgame.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum diff
    {
        None = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Dragon = 4,
    }
}
