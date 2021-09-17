using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Commands.Member
{
   public class MemberCommands : BaseCommandModule
    {
        private  MemberService _memService { get; }

        public MemberCommands(MemberService memService)
        {
            _memService = memService;
        }

        #region WHOIS
        [Command("whois")]
        [Description("gets server member information")]
        public async Task Whois(CommandContext ctx, [RemainingText] string args)
        {
            var members = await _memService.GetAllMembersAsync(ctx);
            string test = "";
        }
        #endregion

        #region ADD MEMBER [must have MOD role or Higher]
        [Command("addmember")]
        [Description("add a member to the database, must have Mod or higher role to execute command")]
        [RequirePermissions(Permissions.ManageChannels)]
        public async Task AddMember(CommandContext ctx, [RemainingText] string args)
        {

        }
        #endregion
    }
}
