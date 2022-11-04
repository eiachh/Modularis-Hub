//using DiscordBotTutorial.Bots.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using ProjectModularisBot.Commands;
using Microsoft.AspNetCore.SignalR.Client;

namespace DiscordBotTutorial.Bots
{
    public class Bot
    {
        public static volatile HubConnection HubConnection;
        public event EventHandler Started;
        public DiscordClient DiscordClient { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            if (!File.Exists("config/config.json"))
            {
                Console.WriteLine("Config.json is not present.. shutting down");
                Environment.Exit(0);
            }

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };
            DiscordClient = new DiscordClient(config);

            DiscordClient.Ready += OnClientReady;
            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,

            };

            Commands = DiscordClient.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<CommandParser>();

            await DiscordClient.ConnectAsync();
            await Task.Delay(-1);


        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            Started?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }
    }
}