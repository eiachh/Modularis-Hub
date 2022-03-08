using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModularisInstanceCreator.DataDefinitions;

namespace ModularisInstanceCreator
{
    public class ModuleRunnerFactory : IModuleRunnerFactory
    {
        public IModuleRunner CreateModuleRunner(IEnumerable<IRunnableCommand> moduleCommandList, string moduleName, string moduleLongDescription, RunMode mode)
        {
            var module = new ModuleInstance()
            {
                ModuleName = moduleName,
                ModuleLongDescription = moduleLongDescription,
                RunnableCommandList = moduleCommandList
            };

            return new ModuleRunner(module,mode);
        }

        public IRunnableCommand CreateCommand(CommandInfo commandInfo, Action<string> command)
        {
            return new RunnableCommand
            {
                CommandInfo = commandInfo,
                Command = command
            };
        }
    }
}
