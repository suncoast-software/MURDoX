using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MURDoX.Data;
using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Helpers
{
    public class UtilityHelper
    {
        /// <summary>
        /// Get Database Latency
        /// </summary>
        /// <returns>int in milliseconds</returns>
        #region GET DATABASE LATENCY
        public static int GetDbLatency()
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                using var db = new AppDbContext();
                var tags = db.Tags.FirstOrDefault(u => u.Id == 1);
                sw.Stop();
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }

            return (int)sw.ElapsedMilliseconds;
        }
        #endregion

        #region GET LIST OF SERVER MEMBERS
        public static async Task<List<Model.DiscordUser>> GetServerMembers(DiscordClient client)
        {
            var guild = await client.GetGuildAsync(682367713011695617);
            var members = await guild.GetAllMembersAsync();
            var users = new List<Model.DiscordUser>();
            using var db = new AppDbContext();
            var savedUsers = db.Users.ToList();

            foreach (DiscordMember member in members)
             {
                var isCreated = db.Users.Where(x => x.DiscordId == member.Id).FirstOrDefault();
                if (isCreated != null)
                {
                    var dUser = member;
                    var user = new Model.DiscordUser()
                    {
                        Username = dUser.Username,
                        DiscordId = dUser.Id,
                        AvatarUrl = dUser.AvatarUrl,
                        Warnings = 0,
                        Thanks = 0,
                        XP = 0,
                        BankAccountTotal = 0,
                        Created = DateTime.Now
                    };
                    users.Add(user);
                }
           
            }
            return users;
        }

        #endregion

        #region CONVERT CATEGORY
        public static string ConvertCategory(string cat)
        {
           string result = cat switch
            {
                "geography" => "22",
                "general knowledge" => "9",
                "entertainment: books" => "10",
                "entertainment: films" => "11",
                "science & nature" => "17",
                "mythology" => "20",
                "sports" => "21",
                "history" => "23",
                "politics" => "24",
                "art" => "25",
                "celebrities" => "26",
                "animals" => "27",
                _ => "9"
            };
            return result;
        }
        #endregion

        #region PICK QUESTION
        public static Question PickQuestion(List<Question> questions)
        {
            Random rnd = new();
            var index = rnd.Next(0, questions.Count);
            return questions[index];
        }
        #endregion

        #region GENERATE RANDOM NUMBER
        public static int GenerateRandomeNumber(int min, int max)
        {
            Random rnd = new();
            return rnd.Next(min, max);
        }
        #endregion


    }
}
