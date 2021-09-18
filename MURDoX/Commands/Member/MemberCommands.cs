using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MURDoX.Helpers;
using MURDoX.Model;
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
        private MemberService _memService { get; }

        public MemberCommands(MemberService memService)
        {
            _memService = memService;
        }

        #region WHOIS
        [Command("whois")]
        [Description("guild member information")]
        public async Task Whois(CommandContext ctx, [RemainingText] string args)
        {
            var botAvatar = ctx.Client.CurrentUser.GetAvatarUrl(DSharpPlus.ImageFormat.Png);
            var members = await _memService.GetAllMembersAsync(ctx);
            var member = members.Where(x => x.Mention.Equals(args)).FirstOrDefault();

            if (member == null)                                                             
                member = members.Where(x => x.DisplayName.Equals(args)).FirstOrDefault(); // user didn't use the @mention format so query based on Displayname
           
            if (member is null)
            {
                await ctx.Channel.SendMessageAsync($"Member : {args} not found!");
                return;
            }
            else
            {
                var memCreated = (member is not null) ? member.JoinedAt : DateTime.Now;
                var today = DateTime.Now;
                var memFor = (int)(today - memCreated).TotalDays;
                var roles = (member is not null) ? member.Roles : new List<DiscordRole>();
                var fields = new EmbedField[]
                {
                new EmbedField { Name = "Member", Value = member.Username, Inline = true },
                new EmbedField { Name = "Time Created", Value = member.CreationTimestamp.ToString(), Inline = true },
                new EmbedField { Name = "Joined", Value = $"{memFor} days ago", Inline = true },
                };
                var embed = new Embed()
                {
                    Color = "blurple",
                    Author = ctx.Message.Author.Username,
                    ThumbnailImgUrl = botAvatar,
                    Desc = "Whois Info",
                    Fields = fields,
                    FooterImgUrl = botAvatar,
                    Footer = $"MURDoX {DateTime.Now}"
                };
                var builder = new EmbedBuilderHelper();
                await ctx.Channel.SendMessageAsync(builder.Build(embed));
            }
            
        }
        #endregion

        #region ADD MEMBER [must have MOD role or Higher]
        [Command("addmember")]
        [Description("add a member to the database, must have Mod or higher role to execute command")]
        [RequirePermissions(Permissions.ManageChannels)]
        public async Task AddMember(CommandContext ctx, [RemainingText] string args)
        {
            await ctx.Channel.SendMessageAsync("");
        }
        #endregion
    }
}
