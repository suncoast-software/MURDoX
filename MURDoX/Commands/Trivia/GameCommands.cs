using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Data;
using MURDoX.Extentions;
using MURDoX.Helpers;
using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Commands.Trivia
{
    public class GameCommands : BaseCommandModule
    {
        /// <summary>
        /// Starts a new GAme object
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="input"></param>
        /// <returns>Task</returns>
        #region NEW GAME
        [Command("game")]
        [Description("Starts a new Trivia game")]
        [RequirePermissions(Permissions.ManageChannels)]
        public async Task NewGame(CommandContext ctx, [RemainingText] string input) //format = !newgame [category] [difficulty]
        {
            var botAvatar = ctx.Client.CurrentUser.GetAvatarUrl(DSharpPlus.ImageFormat.Png);
            if (input is null or "")
            {
                var builder = new EmbedBuilderHelper();
                var embed = new Embed()
                {
                    Color = "darkgray",
                    Title = "FORMAT ERROR",
                    Desc = "```command parameters must not be empty, please try again```",
                    FooterImgUrl = botAvatar,
                    Footer = $"MURDoX {DateTime.Now}"
                };
                await ctx.Channel.SendMessageAsync(builder.Build(embed));
            }
            else
            {
                var inputs = input.Split(" ");
                if (inputs.Length < 2)
                {
                    var builder = new EmbedBuilderHelper();
                    var embed = new Embed()
                    {
                        Color = "darkgray",
                        Title = "FORMAT ERROR",
                        Desc = "```command was in wrong format, or not enough parameters were given, please try again```",
                        FooterImgUrl = botAvatar,
                        Footer = $"MURDoX {DateTime.Now}"
                    };
                    await ctx.Channel.SendMessageAsync(builder.Build(embed));
                }
                else
                {
                    var cat = UtilityHelper.ConvertCategory(inputs[0]);
                    var diff = inputs[1];

                    Game _ = new(ctx);
                    await Game.StartNewGame(cat, diff);
                }
               
            }


        }
        #endregion

        #region STOP GAME
        [Command("stop")]
        [Description("stop a game in progress")]
        [RequireUserPermissions(Permissions.ManageChannels)]
        public async Task StopGame(CommandContext ctx, [RemainingText] string args)
        {
            if (args is null or "")
            {
                await ctx.Channel.SendMessageAsync($"```{ctx.Message.Author.Username} your command had no context, command ignored!```");
            }
            else
            {
                var inputs = args.Split(" ");
                switch (inputs[0])
                {
                    case "trivia":
                        if (!Game.isAlive)
                        {
                            await ctx.Channel.SendMessageAsync($"```{ctx.Message.Author.Username} there is no Game active, command ignored!```");
                        }
                        else
                        {
                            Game.StopGame();
                            using var db = new AppDbContext();
                            var member = db.Users.Where(x => x.Username == ctx.Message.Author.Username).FirstOrDefault();

                            var fields = new EmbedField[]
                            {
                                new EmbedField { Name = "Member", Value = ctx.Message.Author.Username, Inline = true },
                                new EmbedField { Name = "XP", Value = member.XP.ToString(), Inline = true },
                            };
                            var embed = new Embed()
                            {
                                Color = "hotpink",
                                Author = "MURDoX Version 1.0.0",
                                Desc = $"Trivia stopped by : {ctx.Message.Author.Username}",
                                Fields = fields,
                                Footer = $"MURDoX {DateTime.Now}"
                            };
                            var embedBuilder = new EmbedBuilderHelper();
                            await ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
                        }
                        break;

                    default:
                        await ctx.Channel.SendMessageAsync($"```{ctx.Message.Author.Username} your command had no context, command ignored!```");
                        break;

                }
            }
        }
        #endregion

        /// <summary>
        /// Listens for the correct answer from the chat
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="answer"></param>
        /// <returns>Task</returns>
        #region ANSWER
        [Command("answer")]
        [Description("Handle the answer to the trivia question")]
        public async Task HandleAnswer(CommandContext ctx, [RemainingText] string answer)
        {
            if (answer is null or "")
            {
            }
            else
            {
                await Game.HandleAnswer(ctx, answer);
            }

        }
        #endregion

        #region HELP
        [Command("triviahelp")]
        [Description("list commands and categories for trivia")]
        public async Task TriviaHelp(CommandContext ctx)
        {
            var botAvatar = ctx.Client.CurrentUser.GetAvatarUrl(DSharpPlus.ImageFormat.Png);
            var fields = new EmbedField[]
            {
                new EmbedField { Name = "Categories", Value = ":grey_question: General Knowledge\r\n:scientist: Science & Nature\r\n" +
                                        ":football: Sports\r\n:feather: Mythology\r\nHistory\r\n:speaking_head: Politics\r\nArt\r\nCelebrities\r\n" +
                                        "Animals\r\nEntertainment: Books\r\nEntertainment: Films\r\nMore...", Inline = true},
                new EmbedField { Name = "Type", Value = "True/False\r\nMultiple Choice\r\nSingle Answer", Inline = true},
                new EmbedField { Name = "Difficulty", Value = "Easy\r\nMedium\r\nDifficult\r\nInsane", Inline = true},

            };
            var embed = new Embed()
            {
                Color = "darkgray",
                Title = $"{ctx.Client.CurrentUser.Username} Hosting Trivia Ver 1.0.0.0",
                Desc = "listing Trivia Categories...",
                ThumbnailImgUrl = botAvatar,
                Fields = fields,
                Footer = $"MURDox {DateTime.Now}",
                FooterImgUrl = botAvatar

            };
            var embedBuilder = new EmbedBuilderHelper();
            await ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
        }
        #endregion
    }
}
