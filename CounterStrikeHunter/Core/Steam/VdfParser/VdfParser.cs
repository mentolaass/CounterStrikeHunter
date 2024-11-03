using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using Gameloop.Vdf.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace CounterStrikeHunter.Core.Steam.VdfParser
{
    public class VdfParser
    {
        private static readonly string LOGIN_USERS_LOCATION = "\\config\\loginusers.vdf";

        private string _steamPath;

        public VdfParser(string steamPath)
        {
            _steamPath = steamPath;
        }

        public bool HasParse()
        {
            return File.Exists(_steamPath + LOGIN_USERS_LOCATION);
        }

        public VdfUserDictionary Parse()
        {
            if (!HasParse())
            {
                return new VdfUserDictionary()
                {
                    Source = new Dictionary<long, VdfUser>()
                };
            }

            VProperty vProp = VdfConvert.Deserialize(File.ReadAllText(_steamPath + LOGIN_USERS_LOCATION));

            JObject jObjectVdf = new JObject(vProp.ToJson().ToObject<JProperty>());

            return jObjectVdf.ToObject<VdfUserDictionary>();
        }
    }
}
