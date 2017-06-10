using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FalloutBuild
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Special
    {
        Strength,
        Perception,
        Endurance,
        Charisma,
        Intelligence,
        Agility,
        Luck
    }
}