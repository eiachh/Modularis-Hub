using DiscordBotTutorial.Bots;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ModularisInstanceCreator;
using System.Collections.Generic;
using ModularisInstanceCreator.DataDefinitions;
using Microsoft.AspNetCore.SignalR.Client;
using DSharpPlus.Entities;

namespace ProjectModularis
{
    class Program
    {
        public static bool Stop = false;
        private static Bot bot;
        public static void Main()
        {
            StartBot();
            while (!Stop)
            {
                Thread.Sleep(500);
            }
        }

        public static void StartBot()
        {
            bot = new Bot();
            bot.Started += async (s, e) => await TheBot_Started();
            _ = bot.RunAsync();
        }

        private static async Task TheBot_Started()
        {
            DiscordChannel botChannel = await GetBotChannel();
            IModuleRunner moduleRunner = GetModuleRunner(botChannel);

            await moduleRunner.Run();

            Bot.HubConnection = moduleRunner.HubConnection;
            //MOCK
            await moduleRunner.HubConnection.SendAsync("GetModuleNames");

            await Task.Delay(-1);
        }

        private static IModuleRunner GetModuleRunner(DiscordChannel botChannel)
        {
            ModuleRunnerFactory factory = new();
            var writeToBotChannelCmdInf = new CommandInfo
            {
                Name = "WriteToBotChannel",
                Description = "Write to the channel dedicated for the bot any string.",
                ParameterSyntax = "strin param = \"Literally any string.\"",
            };
            var receiveListCmdInf = new CommandInfo
            {
                Name = "ReceiveList",
                Description = "Displays the received string separated by ; sign's.",
                ParameterSyntax = "strin param = \"some;string;separated;like;this\"",
            };
            var receiveStringCmdInf = new CommandInfo
            {
                Name = "Display",
                Description = "Displays the received string.",
                ParameterSyntax = "strin param = \"some;string;separated;like;this\"",
            };

            List<IRunnableCommand> commands = new()
            {
                factory.CreateCommand(writeToBotChannelCmdInf, (text) => botChannel.SendMessageAsync(text)),
                factory.CreateCommand(receiveListCmdInf, (text) => botChannel.SendMessageAsync(text)),
                factory.CreateCommand(receiveStringCmdInf, (text) => botChannel.SendMessageAsync(text)),
            };

            var moduleRunner = factory.CreateModuleRunner(commands, "ModularisMainBot", "modue long description Test", RunMode.Development);
            return moduleRunner;
        }

        private static async Task<DiscordChannel> GetBotChannel()
        {
            int timeoutCounter = 5;

            var guild = bot.Client.Guilds.FirstOrDefault();
            var botChannel = guild.Value.Channels.SingleOrDefault(channelDict => channelDict.Key == 947971806617272390).Value;
            while (botChannel is null && timeoutCounter >= 0)
            {
                await Task.Delay(500);
                timeoutCounter--;

                botChannel = guild.Value.Channels.SingleOrDefault(channelDict => channelDict.Key == 947971806617272390).Value;
            }

            return botChannel;
        }
    }
}
