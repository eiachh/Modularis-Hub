using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModularisInstanceCreator.DataDefinitions;

namespace ModularisInstanceCreator.Helper
{
    public static class RunnableCommandInfoGenerator
    {
        public static InitializeModuleData GetModuleData(ModuleInstance instance)
        {
            IEnumerable<CommandInfo> commandInfos = GetCommandInfos(instance);
            InitializeModuleData data = new()
            {
                ModuleName = instance.ModuleName,
                ModuleLongDescription = instance.ModuleLongDescription,
                CommandInfos = commandInfos
            };

            return data;
        }
        private static IEnumerable<CommandInfo> GetCommandInfos(ModuleInstance instance)
        {
            if (instance is null || instance.RunnableCommandList is null || !instance.RunnableCommandList.Any())
                yield break;

            foreach (var command in instance.RunnableCommandList)
            {
                yield return command.CommandInfo;
            }
        }
    }
}
