using Newtonsoft.Json;
using System.Collections.Generic;

namespace CounterStrikeHunter.Core.Steam.VdfParser
{
    public class VdfUserDictionary
    {
        [JsonProperty("users")]
        public Dictionary<long, VdfUser> Source { get; set; }
    }
}
