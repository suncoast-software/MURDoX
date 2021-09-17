using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Interface
{
   public interface IMemberService
    {
        public Task<List<DiscordMember>> GetAllMembersAsync(CommandContext ctx);
    }
}
