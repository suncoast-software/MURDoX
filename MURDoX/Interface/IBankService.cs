using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Interface
{
    public interface IBankService
    {
        Task GetAccountDetailsAsync(ulong userId);
        Task DepositAsync(CommandContext ctx, DiscordMember user, int amount);
        Task WithdrawAsync(ulong userId, int amount);
    }
}
