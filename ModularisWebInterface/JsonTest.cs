using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public struct JsonTest
    {
        public JsonTest(string botName, string description)
        {
            Botname = "Sent Bot";
            Description = "This was sent with SignalR";
        }

        [JsonProperty("botName")]
        public string Botname { get; private set; }

        [JsonProperty("desc")]
        public string Description { get; private set; }
    }

