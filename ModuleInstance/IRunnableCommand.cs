using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModularisInstanceCreator.DataDefinitions;

namespace ModularisInstanceCreator
{
    public interface IRunnableCommand
    {
        /// <summary>
        /// The command name.
        /// </summary>
        CommandInfo CommandInfo { get; set; }

        /// <summary>
        /// The actual command.
        /// </summary>
        Action<string> Command { get; set; }
    }
}
