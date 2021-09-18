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

            Embed embed = new()
            {
                Title = "UPTIME",
                Author = $"{messageAuthor.Username} Requested Uptime!",
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
            Stopwatch sw = new();
            var embedBuilder = new EmbedBuilderHelper();
           
            var botAvatar = ctx.Client.CurrentUser.AvatarUrl;
            var botName = ctx.Client.CurrentUser.Username;

            var dbLatTime = UtilityHelper.GetDbLatency();
            sw.Start();
            var pongMessage = await ctx.Message.RespondAsync("```pong```").ConfigureAwait(false);
            var messageLatTime = sw.ElapsedMilliseconds;
            sw.Stop();
            await pongMessage.DeleteAsync();

            EmbedField dbSpeedField = new() { Name = "Database Latency", Value = $":hourglass_flowing_sand: {dbLatTime} ms", Inline = true };
            EmbedField discordSpeedField = new() { Name = "Message Latency", Value = $":notepad_spiral: {messageLatTime} ms", Inline = true };
            EmbedField pingSpeedField = new() { Name = "Discord Latency", Value = $":timer: {ctx.Client.Ping} ms", Inline = true };
            var fields = new EmbedField[] { dbSpeedField, discordSpeedField, pingSpeedField };

            Embed embed = new()
            {
                Title = "LATENCY",
                Author = $"{ctx.Message.Author.Username} Requested Latency!!",
                Desc = $"total time \'{botName}\' took to round trip the database, discord and message server.",
                Footer = $"{botName} ©️{DateTime.Now.ToLongDateString()}",
                AuthorAvatar = ctx.Message.Author.GetAvatarUrl(DSharpPlus.ImageFormat.Png),
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

        #region TODO
        [Command("todo")]
        [Description("list's the top 5 todo development items")]
        [RequireRoles(RoleCheckMode.Any, roleNames: "Dev")]
        public async Task Todo(CommandContext ctx)
        {
            using var db = new AppDbContext();
            var todoList = db.Todos
                              .OrderBy(x => x.Created)
                              .Take(5)
                              .ToList();
            var fields = new EmbedField[4];
            for (int i = 0; i < todoList.Count; i++)
            {
                var curTodo = todoList[i];
                fields[0] = new EmbedField { Name = "TODO", Value = curTodo.Name, Inline = true };
                fields[1] = new EmbedField { Name = "Description", Value = curTodo.Desc, Inline = true };
                fields[2] = new EmbedField { Name = "Status", Value = curTodo.Status.ToString(), Inline = true };
                fields[3] = new EmbedField { Name = "Comment", Value = curTodo.Comment, Inline = false };
            }

            Embed embed = new()
            {

            };

            await ctx.Channel.SendMessageAsync("");
        }
        #endregion

    }
}
