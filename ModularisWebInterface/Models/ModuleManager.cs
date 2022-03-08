using ModularisInstanceCreator.DataDefinitions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ModularisWebInterface.Models
{
    public class ModuleManager : IModuleManager
    {
        private Dictionary<string, InitializeModuleData> moduleDataCollection = new Dictionary<string, InitializeModuleData>();
        public void AddModule(string connectionId, InitializeModuleData moduleData)
        {
            moduleDataCollection.Add(connectionId, moduleData);
        }
        public IEnumerable<string> GetModuleNames()
        {
            if(moduleDataCollection.Count == 0)
                yield break;

            foreach (var moduleData in moduleDataCollection.Values)
            {
                yield return moduleData.ModuleName;
            }
        }
        public InitializeModuleData? GetModuleData(string moduleName)
        {
            return GetKeyValuePairOfModule(moduleName)?.Value;
        }

        public string GetConnectionIdOfModule(string moduleName)
        {
            return GetKeyValuePairOfModule(moduleName)?.Key;
        }

        public void RemoveModule(string connectionId)
        {
            if (!moduleDataCollection.ContainsKey(connectionId))
                return;

            moduleDataCollection.Remove(connectionId);
        }

        private KeyValuePair<string, InitializeModuleData>? GetKeyValuePairOfModule(string moduleName)
        {
            var modulesWithMatchingName = moduleDataCollection.Where(module => module.Value.ModuleName == moduleName);
            if (modulesWithMatchingName.Count() > 1)
                throw new ArgumentException("The given module name was registered multiple time");

            return modulesWithMatchingName.FirstOrDefault();
        }
    }
}
