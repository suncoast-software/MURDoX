using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MURDoX.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services
{
    public class MemberService : IMemberService
    {
        public async Task<List<DiscordMember>> GetAllMembersAsync(CommandContext ctx)
        {
            var memList = await ctx.Guild.GetAllMembersAsync();
            return memList.ToList();
        }

    }
}
