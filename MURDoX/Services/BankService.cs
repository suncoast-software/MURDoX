using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MURDoX.Data;
using MURDoX.Interface;
using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services
{
    public class BankService : IBankService
    {
        public BankService()
        {

        }

        public async Task DepositAsync(CommandContext ctx, DiscordMember user, int amount)
        {
            using var db = new AppDbContext();
            var u = db.Users.Where(x => x.DiscordId == user.Id).FirstOrDefault();
            var mentioned = ctx.Message.MentionedUsers.Where(x => x.Username == user.Username).FirstOrDefault();
            if (u == null)
            {
                await ctx.Channel.SendMessageAsync($"```user {mentioned.Username} not found!```");
                await ctx.Channel.SendMessageAsync("```Please add a new Discord User [!adduser @Username] will add the mentioned user to the database```");
                return;
            }
            var depositAmount = u.BankAccountTotal + amount;
            u.BankAccountTotal = depositAmount;
            db.Update(u);
            await db.SaveChangesAsync();
        }

        public Task GetAccountDetailsAsync(ulong userId)
        {
            throw new NotImplementedException();
        }

        public Task WithdrawAsync(ulong userId, int amount)
        {
            throw new NotImplementedException();
        }

        //TODO: add user service
    }
}
