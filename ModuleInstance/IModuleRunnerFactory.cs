using ModularisInstanceCreator.DataDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularisInstanceCreator
{
    public interface IModuleRunnerFactory
    {
        public IModuleRunner CreateModuleRunner(IEnumerable<IRunnableCommand> moduleCommandList, string moduleName, string moduleLongDescription, RunMode mode);
        public IRunnableCommand CreateCommand(CommandInfo commandInfo, Action<string> command);
    }
}
