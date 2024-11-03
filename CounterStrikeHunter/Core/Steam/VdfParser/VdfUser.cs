using Newtonsoft.Json;

namespace CounterStrikeHunter.Core.Steam.VdfParser
{
    public class VdfUser
    {
        public long Id { get; set; }

        [JsonProperty("AccountName")]
        public string AccountName { get; set; }

        [JsonProperty("PersonaName")]
        public string PersonaName { get; set; }

        [JsonProperty("Timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("MostRecent")]
        public int MostRecent { get; set; }
    }
}
