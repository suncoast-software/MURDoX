using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MURDoX.Config;
using MURDoX.Configuration;
using MURDoX.Data;
using MURDoX.Helpers;
using MURDoX.Interface;
using MURDoX.Services;
using MURDoX.Theme;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX
{
    public class Bot
    {
        public DiscordClient Client { get; set; }
        public CommandsNextExtension Commands { get; set; }
        public static Bot Instance { get; } = new Bot();
        public static InteractivityExtension Interactivity { get; }

        public async Task RunAsync()
        {
            //setup the data service for the config info
            DataService dataService = new();
            var configJson = dataService.GetApplicationConfig();

            //set up discord configuration
            var clientConfig = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,

            };

            //initialize the client
            Client = new DiscordClient(clientConfig);
            Client.Ready += Client_Ready;

            //set up client interactivity configuration
            var iteractivityConfig = new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(1),
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationEmojis = new PaginationEmojis(),
                PaginationBehaviour = PaginationBehaviour.WrapAround,
                PaginationDeletion = PaginationDeletion.KeepEmojis
            };

            Client.UseInteractivity(iteractivityConfig);

            //configure the services
            var services = new ServiceCollection()
                .AddTransient<AppDbContext>()
                .AddSingleton<BankService>()
                .AddSingleton<MemberService>()
                .BuildServiceProvider();

            //set up commands configuration
            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = services
            };

            this.Commands = this.Client.UseCommandsNext(commandsConfig);
            RegisterCommands();

            //register the events
            Client.MessageCreated += Client_MessageCreated;
            Client.ClientErrored += Client_ClientErrored;
            Client.GuildMemberAdded += Client_GuildMemberAdded;
            Client.ChannelCreated += Client_ChannelCreated;
            Client.InviteCreated += Client_InviteCreated;

            //connect client to gateway
            await Client.ConnectAsync(new DiscordActivity("Everyone", ActivityType.Watching)).ConfigureAwait(false);

            //start the server timer
            TimerService timerService = new TimerService();
            timerService.Start();

            //Load all Server Members into a List<DiscordMember>
            // var members = await UtilityHelper.GetServerMembers(Client);
            var guild = await Client.GetGuildAsync(682367713011695617);
            var members = await guild.GetAllMembersAsync();
         
            //wait - this keeps the console running
            await Task.Delay(-1).ConfigureAwait(false);
        }

        //TODO: fix Logging

        //private static void AddLogging()
        //{

        //        LoggerConfiguration? logger = new LoggerConfiguration()
        //            .WriteTo.Console(outputTemplate: "[{Timestamp:h:mm:ss ff tt}] [{Level:u3}] [{SourceContext}] {Message:lj} {Exception:j}{NewLine}", theme: new LogTheme()) 
        //            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
        //            .MinimumLevel.Override("DSharpPlus", LogEventLevel.Fatal);

        //    Log.Logger = "LogLevel" switch
        //    {
        //        "All" => logger.MinimumLevel.Verbose().CreateLogger(),
        //        "Info" => logger.MinimumLevel.Information().CreateLogger(),
        //        "Debug" => logger.MinimumLevel.Debug().CreateLogger(),
        //        "Warning" => logger.MinimumLevel.Warning().CreateLogger(),
        //        "Error" => logger.MinimumLevel.Error().CreateLogger(),
        //        "Panic" => logger.MinimumLevel.Fatal().CreateLogger(),
        //        _ => logger.MinimumLevel.Verbose().CreateLogger()
        //    };

        //}

        private Task Client_InviteCreated(DiscordClient sender, InviteCreateEventArgs e)
        {
            Task.Run(async () =>
            {
                //ulong channelId = 762711629829767218;
                //var logChannel = await sender.GetChannelAsync(channelId).ConfigureAwait(false);
                var message = $"Invite {e.Invite} created at {DateTime.Now} by {e.Invite.Inviter.Username}";
                await SendLogMessage(message); 

            });
            return Task.CompletedTask;
        }

        private Task Client_ChannelCreated(DiscordClient sender, ChannelCreateEventArgs e)
        {
            Task.Run(async () =>
            {
                ulong channelId = 762711629829767218;
                var logChannel = await Client.GetChannelAsync(channelId);
                var message = $"channel {e.Channel.Name} created at {DateTime.Now}";
                await logChannel.SendMessageAsync(message);
            });
            return Task.CompletedTask;
        }

        private Task Client_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            ulong channelId = 762711629829767218;
            ulong wChannel = 762711555322019860;
            var userName = e.Member.Username;
            var joinedAt = DateTime.Now;

            Task.Run(async () =>
            {
                var logChannel = await Client.GetChannelAsync(channelId);
                var welcomeChannel = await Client.GetChannelAsync(wChannel);
                var welcomeMsg = $"Welcome {e.Member.Mention}";
                var u = e.Member;
                try
                {
                    await u.SendMessageAsync("```Welcome to the server, your Id and usename has been saved in the database!```");

                }
                catch (Exception ex)
                {
                    await logChannel.SendMessageAsync($"dm's are turned off for user {u.Username}");
                    return Task.CompletedTask;
                }

                var message = $"```#{userName} joined the Guild at {joinedAt}```";

                await logChannel.SendMessageAsync(message);
                await welcomeChannel.SendMessageAsync(welcomeMsg);
                return Task.CompletedTask;
            });
            return Task.CompletedTask;

        }

        private Task Client_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(String.Format("Something went wrong - message : {0}", e.Exception.Message));
            return Task.CompletedTask;
        }

        private Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            Task.Run(async () =>
            {
                var author = e.Message.Author;
                var message = e.Message.Content;
                ulong logChannelId = 762711629829767218;
                var banneddWords = new string[] { "shit", "bitch", "asshole", "fuck", "fucker", "dickhead", "pussy" };
                var channel = await sender.GetChannelAsync(logChannelId).ConfigureAwait(false);
                
                if (author.IsBot) return;
            });
            return Task.CompletedTask;
        }

        private async Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            var botName = sender.CurrentUser.Username;
            await SendLogMessage($"```{botName} is Online: Connected At: {DateTime.Now}```").ConfigureAwait(false);
            Console.WriteLine($"{botName} Connected!");
            //return Task.CompletedTask;
        }

        private async Task SendLogMessage(string message)
        {
            ulong channelId = 762711629829767218;
            var logChannel = await Client.GetChannelAsync(channelId).ConfigureAwait(false);
            await logChannel.SendMessageAsync(message);
        }

        private void RegisterCommands() => Client.GetCommandsNext().RegisterCommands(Assembly.GetExecutingAssembly());
    }
}
