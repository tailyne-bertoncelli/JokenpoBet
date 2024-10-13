using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace desafio.Entities.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusTransaction
    {
        GANHOU,
        PERDEU,
        CANCELADA,
        EM_ANDAMENTO,
        CONCLUIDA,
        EMPATE
    }
}
