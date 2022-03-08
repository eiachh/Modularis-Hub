using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModularisInstanceCreator.DataDefinitions;

namespace ModularisInstanceCreator
{
    public class RunnableCommand : IRunnableCommand
    {
        public CommandInfo CommandInfo { get; set; }
        public Action<string> Command { get; set; }
    }
}
