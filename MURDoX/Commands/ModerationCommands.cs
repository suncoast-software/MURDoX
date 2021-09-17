using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Data;
using MURDoX.Helpers;
using MURDoX.Model;
using MURDoX.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Commands
{
    public class ModerationCommands : BaseCommandModule
    {

        #region UPTIME COMMAND
        [Command("uptime")]
        [Description("Returns the total time the bot has been online")]
        public async Task Uptime(CommandContext ctx)
        {
            var botStartDate = TimerService.StartDate;
            var uptime = TimerService.GetBotUpTime();

            var startDateField = new EmbedField { Name = "Started On", Value = $"{botStartDate}", Inline = false };

            var secondsField = new EmbedField { Name = "Seconds", Value = $"{uptime.Seconds}", Inline = true };
            var minutesField = new EmbedField { Name = "Minutes", Value = $"{uptime.Minutes}", Inline = true };
            var hoursField = new EmbedField { Name = "Hours", Value = $"{uptime.Hours}", Inline = true };
            var daysField = new EmbedField { Name = "Days", Value = $"{uptime.Days}", Inline = true };
            var weeksField = new EmbedField { Name = "Weeks", Value = $"{uptime.Weeks}", Inline = true };
            var yearsField = new EmbedField { Name = "Years", Value = $"{uptime.Years}", Inline = true };

            var messageAuthor = ctx.Message.Author;
            var botAvatar = ctx.Client.CurrentUser.AvatarUrl;
            var botName = ctx.Client.CurrentUser.Username;

            var fields = new EmbedField[] { startDateField, yearsField, weeksField, daysField, hoursField, minutesField, secondsField };
            var embedBuilder = new EmbedBuilderHelper();

            Embed embed = new Embed
            {
                Title = "UPTIME",
                Author = $"User {messageAuthor.Username} Requested Uptime!",
                Desc = $"total time \'{botName}\' has been online.",
                Footer = $"{botName} ©️{DateTime.Now.ToLongDateString()}",
                AuthorAvatar = messageAuthor.GetAvatarUrl(DSharpPlus.ImageFormat.Jpeg),
                ImgUrl = null,
                ThumbnailImgUrl = botAvatar,
                FooterImgUrl = botAvatar,
                Color = "orange",
                Fields = fields
            };

            var _embed = embedBuilder.Build(embed);

            await ctx.Channel.SendMessageAsync(_embed);
        }
        #endregion

        #region LATENCY COMMAND
        [Command("Ping")]
        [Description("ping the database and discord and return the speed of each.")]
        public async Task Ping(CommandContext ctx)
        {
            Stopwatch sw = new Stopwatch();
            var embedBuilder = new EmbedBuilderHelper();
           
            var botAvatar = ctx.Client.CurrentUser.AvatarUrl;
            var botName = ctx.Client.CurrentUser.Username;

            var dbLatTime = UtilityHelper.GetDbLatency();
            sw.Start();
            var pongMessage = await ctx.Message.RespondAsync("```pong```").ConfigureAwait(false);
            var messageLatTime = sw.ElapsedMilliseconds;
            sw.Stop();
            await pongMessage.DeleteAsync();

            EmbedField dbSpeedField = new EmbedField { Name = "Database Latency", Value = $":hourglass_flowing_sand: {dbLatTime} ms", Inline = true };
            EmbedField discordSpeedField = new EmbedField { Name = "Message Latency", Value = $":notepad_spiral: {messageLatTime} ms", Inline = true };
            EmbedField pingSpeedField = new EmbedField { Name = "Discord Latency", Value = $":timer: {ctx.Client.Ping} ms", Inline = true };
            var fields = new EmbedField[] { dbSpeedField, discordSpeedField, pingSpeedField };

            Embed embed = new Embed
            {
                Title = "LATENCY",
                Author = $"User {ctx.Message.Author.Username} Requested Latency!!",
                Desc = $"total time \'{botName}\' took to round trip the database, discord and message server.",
                Footer = $"{botName} ©️{DateTime.Now.ToLongDateString()}",
                AuthorAvatar = ctx.Message.Author.GetAvatarUrl(DSharpPlus.ImageFormat.Jpeg),
                ImgUrl = null,
                ThumbnailImgUrl = botAvatar,
                FooterImgUrl = botAvatar,
                Color = "orange",
                Fields = fields
            };

            await ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
        }
        #endregion

        #region PURGE
        [Command("purge")]
        [Description("delete a set number of channel messages")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Purge(CommandContext ctx, [RemainingText] string count)
        {
            var success = int.TryParse(count, out int newCount);
            int mCount = 0;

            if (success)
            {
                IEnumerable<DiscordMessage> allMessages = await ctx.Channel.GetMessagesAsync(newCount + 1).ConfigureAwait(false);

                foreach (var mes in allMessages)
                {
                    mCount++;
                }

                await ((DiscordChannel)ctx.Channel).DeleteMessagesAsync(allMessages);
                const int delay = 1500;
                DiscordMessage m = await ctx.Channel.SendMessageAsync($"```Delete {mCount} messages [SUCESS!]```").ConfigureAwait(false);
                await Task.Delay(delay);
                await m.DeleteAsync();
            }
        }
        #endregion

    }
}
