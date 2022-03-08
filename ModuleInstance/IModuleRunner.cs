using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ModularisInstanceCreator
{
    public interface IModuleRunner
    {
        public HubConnection HubConnection { get; set; }
        public string DisplayModule { get; set; }
        public event EventHandler DisplayModuleChanged;
        Task Run();
    }
}
