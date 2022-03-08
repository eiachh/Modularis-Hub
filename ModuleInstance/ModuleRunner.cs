using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using ModularisInstanceCreator.Helper;
using Newtonsoft.Json;

namespace ModularisInstanceCreator
{
    public enum RunMode
    {
        Development,
        Release
    }
    public class ModuleRunner : IModuleRunner
    {
        public HubConnection HubConnection { get; set; }
        public string DisplayModule
        {
            get => _DisplayModule;
            set
            {
                _DisplayModule = value;
                DisplayModuleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public string ReleaseServerUrl { get; set; } = "https://modularis.duckdns.org:8001/mainhub";
        public string LocalServerUrl { get; set; } = "https://localhost:5001/mainhub";

        public event EventHandler DisplayModuleChanged;

        private volatile string _DisplayModule;
        private string usedServerUrl = "";
        private ModuleInstance moduleInstance;
        public ModuleRunner(ModuleInstance moduleInstance, RunMode mode)
        {
            if (mode == RunMode.Development)
                usedServerUrl = LocalServerUrl;
            else
                usedServerUrl = ReleaseServerUrl;

            this.moduleInstance = moduleInstance;
        }
        public async Task Run()
        {
            IgnoreUnsafeCertificate();
            RegisterDefaultFunctions();
            RegisterGivenFunctions();

            await HubConnection.StartAsync();

            var modulData = JsonConvert.SerializeObject(RunnableCommandInfoGenerator.GetModuleData(moduleInstance));
            await HubConnection.SendAsync("RegisterModule", modulData).ConfigureAwait(false);

            await RequestDisplayModule();
        }

        private void IgnoreUnsafeCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                              (sender, certificate, chain, sslPolicyErrors) => true;

            HubConnection = new HubConnectionBuilder()
                .WithUrl(usedServerUrl, (conf) => conf.HttpMessageHandlerFactory = (x) => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                })
                .Build();
        }

        private void RegisterDefaultFunctions()
        {
            HubConnection.On<string>("SetDisplayModule", (DispModule) => Idk(DispModule));
        }
        private void RegisterGivenFunctions()
        {
            foreach (var command in moduleInstance.RunnableCommandList)
            {
                HubConnection.On<string>(command.CommandInfo.Name, command.Command);
            }
        }

        private async Task RequestDisplayModule()
        {
            await HubConnection.SendAsync("GetDefaultDisplayModule");
        }

        private void Idk(string inp)
        {
            DisplayModule = inp;
        }
    }
}
