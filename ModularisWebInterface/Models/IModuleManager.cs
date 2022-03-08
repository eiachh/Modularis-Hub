using ModularisInstanceCreator.DataDefinitions;
using System.Collections.Generic;

namespace ModularisWebInterface.Models
{
    public interface IModuleManager
    {
        void AddModule(string connectionId,InitializeModuleData moduleData);
        void RemoveModule(string connectionId);
        IEnumerable<string> GetModuleNames();
        InitializeModuleData? GetModuleData(string moduleName);
        public string GetConnectionIdOfModule(string moduleName);
    }
}
