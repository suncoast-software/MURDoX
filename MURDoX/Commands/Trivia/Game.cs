using DSharpPlus;
using DSharpPlus.CommandsNext;
using MURDoX.Data;
using MURDoX.Helpers;
using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MURDoX.Extentions.CollectionExtensions;


namespace MURDoX.Commands.Trivia
{
   public class Game
    {
        private static List<Question> _questions { get; set; }
        public static Question CurrentQuestion { get; set; }
        public static CommandContext _ctx { get; set; }
        private static Stopwatch _timer { get; set; }
        public static bool isAlive { get; set; }
        private static bool isAnswered { get; set; } 

        public static string _answered { get; set; }

        public Game(CommandContext ctx)
        {
            _ctx = ctx;
            _timer = new Stopwatch();
            _timer.Start();
            _questions = new();
            isAlive = true;
        }

        /// <summary>
        /// Starts a new Trivia game
        /// </summary>
        /// <param name="category"></param>
        /// <param name="difficulty"></param>
        /// <returns>Task</returns>
        #region START NEW GAME
        public static async Task StartNewGame(string category, string difficulty)
        {

           // ulong botId = 762465336300666881;
            var bot = _ctx.Client.CurrentUser.AvatarUrl;
            var questionResult = HttpHelper.MakeQuestionRequest(category, difficulty);
            _questions = HttpHelper.HandleQuestionResponse(questionResult);
            var question = UtilityHelper.PickQuestion(_questions);
            var question1 = question._Question.Replace("&quot;", String.Empty).Replace("&H039;", String.Empty);
            var embed = new Embed();
            var fields = new EmbedField[]
                                {   new EmbedField { Name = "Possible Answer", Value = question.Answers[0].ToString(), Inline = true},
                                    new EmbedField { Name = "Possible Answer", Value = question.Answers[1].ToString(), Inline = true},  
                                    new EmbedField { Name = "Possible Answer", Value = question.CorrectAnswer, Inline = true},
                                }.Shuffle();
          
            embed.Color = "blurple";
            embed.Title = "MURDoX Trivia Quest Version 1.0.0";
            embed.AuthorAvatar = bot;
            embed.Desc = $"```{question1}```";
            embed.Fields = fields;
            embed.Footer = $"Remaining Questions : {_questions.Count - 1} - correct answer {question.CorrectAnswer}";//change this
            var embedBuilder = new EmbedBuilderHelper();
            await _ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
            _questions.Remove(question);
            //var answered = ctx.Channel.GetMessageAsync(ctx.Message.Id);
            CurrentQuestion = question;

            while (isAlive)
            {
                if (_timer.ElapsedMilliseconds == 30000)
                {
                    if (_questions.Count > 0)
                    {
                        _timer.Reset();
                        question = UtilityHelper.PickQuestion(_questions);
                        question1 = question._Question.Replace("&quot;", String.Empty);
                        embed = new Embed();
                        fields = new EmbedField[]
                                            {   new EmbedField { Name = "Possible Answer", Value = question.Answers[0].ToString(), Inline = true},
                                            new EmbedField { Name = "Possible Answer", Value = question.Answers[1].ToString(), Inline = true},
                                            new EmbedField { Name = "Possible Answer", Value = question.CorrectAnswer, Inline = true},
                                            }.Shuffle();
                        embed.Color = "blurple";
                        embed.Title = "MURDoX Trivia Quest Version 1.0.0";
                        embed.AuthorAvatar = bot;
                        embed.Desc = $"```{question1}```";
                        embed.Fields = fields;
                        embed.Footer = $"Remaining Questions : {_questions.Count - 1} - correct answer {question.CorrectAnswer}";//change this

                        embedBuilder = new EmbedBuilderHelper();
                        await _ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
                        _questions.Remove(question);
                        _timer.Start();
                        CurrentQuestion = question;
                        isAnswered = false;

                    }
                    else
                    {
                        _timer.Reset();
                        await _ctx.Channel.SendMessageAsync("```generating new question list...```");
                        var delay = UtilityHelper.GenerateRandomeNumber(5000, 8000);
                        await Task.Delay(delay);
                        questionResult = HttpHelper.MakeQuestionRequest(category, difficulty);
                        _questions = HttpHelper.HandleQuestionResponse(questionResult);
                        question = UtilityHelper.PickQuestion(_questions);
                        question1 = question._Question.Replace("&quot", String.Empty);
                        embed = new Embed();
                        fields = new EmbedField[]
                                            {   new EmbedField { Name = "Possible Answer", Value = question.Answers[0].ToString(), Inline = true},
                                            new EmbedField { Name = "Possible Answer", Value = question.Answers[1].ToString(), Inline = true},
                                            new EmbedField { Name = "Correct Answer", Value = question.CorrectAnswer, Inline = true},
                                            }.Shuffle();
                        embed.Color = "blurple";
                        embed.Title = "MURDoX Trivia Quest Version  1.0.0";
                        embed.AuthorAvatar = bot;
                        embed.Desc = $"```{question1}```";
                        embed.Fields = fields;
                        embed.Footer = $"Remaining Questions : {_questions.Count - 1} - correct answer {question.CorrectAnswer}";//change this
                        embedBuilder = new EmbedBuilderHelper();
                        await _ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));

                        _questions.Remove(question);
                        _timer.Start();
                        CurrentQuestion = question;
                        isAnswered = false;
                    }

                }

            }
        }
        #endregion

        #region HANDLE ANSWER
        public static async Task HandleAnswer(CommandContext ctx, string answer)
        {
           if (isAlive == false) return;
           if (answer.ToLower() == CurrentQuestion.CorrectAnswer.ToLower())
            {
                if (isAnswered == false)
                {
                    isAnswered = true;
                    using var db = new AppDbContext();
                    var user = db.Users.Where(x => x.Username == ctx.Message.Author.Username).Select(x => x).FirstOrDefault();
                    if (user != null)
                    {
                        user.XP += 1;
                    }
                    else
                    {
                        user = new DiscordUser()
                        {
                            DiscordId = ctx.Message.Author.Id,
                            Username = ctx.Message.Author.Username,
                            AvatarUrl = ctx.Message.Author.AvatarUrl,
                            Warnings = 0,
                            Thanks = 0,
                            XP = 1,
                            Rank = Rank.NEWB,
                            BankAccountTotal = 0,
                            Created = DateTime.Now

                        };
                    }

                    int userXp = user.XP;
                    string username = user.Username;
                    db.Update(user);
                    await db.SaveChangesAsync();
                    var fields = new EmbedField[]
                                        {
                                        new EmbedField { Name = "User", Value = user.Username, Inline = true },
                                        new EmbedField { Name = "XP", Value = userXp.ToString(), Inline = true },
                                        };
                    var embed = new Embed()
                    {
                        Color = "orange",
                        Desc = $"```{username} answered correctly!```",
                        Fields = fields,
                        Footer = $"MURDoX scanning chat always"

                    };
                    var embedBuilder = new EmbedBuilderHelper();
                    await ctx.Channel.SendMessageAsync(embedBuilder.Build(embed));
                   // await ctx.Channel.SendMessageAsync($"```{username} { _ctx.Message.Author.Username}```");

                }
            }
        }
        #endregion

        #region STATS

        #endregion

        #region STOP GAME
        public static void StopGame()
        {
            isAlive = false;
        }
        #endregion

    }
}
