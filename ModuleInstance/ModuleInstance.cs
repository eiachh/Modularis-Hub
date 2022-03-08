using System;
using System.Collections.Generic;

namespace ModularisInstanceCreator
{
    public class ModuleInstance
    {
        /// <summary>
        /// Raised when instance creation finished.
        /// </summary>
        public event EventHandler InstanceCreated;

        /// <summary>
        /// The name of the module.
        /// </summary>
        public string ModuleName { get; set; } = "Module name not given";

        /// <summary>
        /// The long description of the module.
        /// </summary>
        public string ModuleLongDescription { get; set; } = "Description not given";

        public IEnumerable<IRunnableCommand> RunnableCommandList = null;

        /// <summary>
        /// Should be called when Instance creation finished.
        /// </summary>
        protected void OnInstanceCreationFinished()
        {
            InstanceCreated?.Invoke(this, EventArgs.Empty);
        }
    }
}
