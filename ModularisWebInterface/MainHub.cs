using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModularisWebInterface
{
    public class MainHub : Hub
    {

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task RegisterModule(string botName)
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }

        public async Task Test1(string input)
        {
            //var smth = JsonConvert.DeserializeObject<JsonTest>(input);
            //var asdad = input;
            Console.WriteLine($"test1 came forwarding: {input}");
            await Clients.All.SendAsync("test2", input);
            
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected client: {Context.ConnectionId}");

            return Task.CompletedTask;
        }
    }
}
