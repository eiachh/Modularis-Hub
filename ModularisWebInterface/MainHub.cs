using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModularisInstanceCreator.DataDefinitions;
using ModularisWebInterface.Models;

namespace ModularisWebInterface
{
    public class MainHub : Hub
    {
        private IModuleManager _moduleManager;
        public MainHub(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        public void RegisterModule(string moduleInfoJson)
        {
            var registerData = JsonConvert.DeserializeObject<InitializeModuleData>(moduleInfoJson);
            _moduleManager.AddModule(Context.ConnectionId, registerData);
        }
        public async Task GetDefaultDisplayModule()
        {
            await Clients.Caller.SendAsync("SetDisplayModule", "ModularisMainBot");
        }
        public async Task GetModuleNames()
        {
            var moduleInfo = _moduleManager.GetModuleNames();

            var moduleNames = String.Join(';', moduleInfo);
            await Clients.Caller.SendAsync("ReceiveList", moduleNames);
        }

        public async Task GetModuleCommands(string moduleName)
        {
            var moduleInfo = _moduleManager.GetModuleData(moduleName);
            if (!moduleInfo.HasValue)
                return;

            var tmp = moduleInfo.Value.CommandInfos.Select(cmdInfo => cmdInfo.Name);
            var cmdNames = String.Join(';', tmp);
            await Clients.Caller.SendAsync("ReceiveList", cmdNames);
        }

        public async Task TargetedCall(string moduleName, string methodName, string input)
        {
            var connectionId = _moduleManager.GetConnectionIdOfModule(moduleName);
            if (string.IsNullOrEmpty(connectionId))
                return;

            await Clients.Client(connectionId).SendAsync(methodName, input);
        }

        public async Task Test1(string input)
        {
            var asdad = input;
            Console.WriteLine($"test1 came forwarding: {input}");

            await Clients.All.SendAsync("WriteOnConsole", input);
            await Clients.All.SendAsync("WriteToBotChannel", input);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected client: {Context.ConnectionId}");

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _moduleManager.RemoveModule(Context.ConnectionId);

            return Task.CompletedTask;
        }
    }
}
