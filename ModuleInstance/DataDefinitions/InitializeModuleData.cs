using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularisInstanceCreator.DataDefinitions
{
    public struct InitializeModuleData
    {
        [JsonProperty("ModuleName")]
        public string ModuleName { get; set; }

        [JsonProperty("ModuleLongDescription")]
        public string ModuleLongDescription { get; set; }

        [JsonProperty("CommandInfos")]
        public IEnumerable<CommandInfo> CommandInfos;
    }
}
