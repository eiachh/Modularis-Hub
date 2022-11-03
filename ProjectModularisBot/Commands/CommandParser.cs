using DiscordBotTutorial.Bots;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ProjectModularisBot.Commands
{
    public class CommandParser : BaseCommandModule
    {
        [Command("modules")]
        public async Task GetModules(CommandContext ctx)
        {
            await Bot.HubConnection.SendAsync("GetModuleNames").ConfigureAwait(false);
        }

        [Command("commands")]
        public async Task GetModules(CommandContext ctx, string moduleName)
        {
            await Bot.HubConnection.SendAsync("GetModuleCommands", moduleName).ConfigureAwait(false);
        }

        [Command("run")]
        public async Task Run(CommandContext ctx, string moduleName, string methodName, string input)
        {
            await Bot.HubConnection.SendAsync("TargetedCall", moduleName, methodName, input).ConfigureAwait(false);
        }
    }
}
