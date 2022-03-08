using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularisInstanceCreator.DataDefinitions
{
    public struct CommandInfo
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("ParameterSyntax")]
        public string ParameterSyntax { get; set; }
    }
}
