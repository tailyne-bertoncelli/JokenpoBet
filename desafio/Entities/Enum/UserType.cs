using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace desafio.Entities.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserType
    {
        PLAYER,
        ADMIN
    }
}
