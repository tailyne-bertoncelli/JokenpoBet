using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace desafio.Entities.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Jokenpon
    {
        PEDRA,
        PAPEL,
        TESOURA
    }
}
