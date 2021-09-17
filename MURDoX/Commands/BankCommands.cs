using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Interface;
using MURDoX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Commands
{
    public class BankCommands : BaseCommandModule
    {
        private BankService _bankService { get; }
        public BankCommands(BankService bankService)
        {
            _bankService = bankService;
        }

        [Command("deposit")]
        [Description("deposit into users bank")]
        public async Task Deposit(CommandContext ctx, [RemainingText] string args)
        {
            var userId = ctx.Message.Author.Id;
            var user = await ctx.Guild.GetMemberAsync(userId);
            await _bankService.DepositAsync(ctx, user, args[1]);
        }
        
    }
}
