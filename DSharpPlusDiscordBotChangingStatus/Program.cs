using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DSharpPlusDiscordBotChangingStatus
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static Timer StatusUpdateTimer { get; set; }
        private static List<DiscordActivity> StatusList { get; set; }
        private static int CurrentStatusIndex { get; set; }

        static async Task Main(string[] args)
        {
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = "TOKEN", 
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(discordConfig);

            Client.Ready += Client_Ready;

            StatusList = new List<DiscordActivity>
            {
                new DiscordActivity("Watching", ActivityType.Watching),
                new DiscordActivity("Listening To", ActivityType.ListeningTo),
                new DiscordActivity("Playing", ActivityType.Playing),
            };

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            CurrentStatusIndex = 0;
            await UpdateBotStatus();

            StatusUpdateTimer = new Timer(async _ => await UpdateBotStatus(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }
        private static async Task UpdateBotStatus(object state = null)
        {
            if (StatusList.Count > 0)
            {
                await Client.UpdateStatusAsync(StatusList[CurrentStatusIndex]);

                CurrentStatusIndex = (CurrentStatusIndex + 1) % StatusList.Count;
            }
        }

    }
}
