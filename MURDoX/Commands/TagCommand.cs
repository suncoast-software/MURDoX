using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MURDoX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Commands
{
    public class TagCommand : BaseCommandModule
    {
        #region TAG COMMAND
        [Command("tag")]
        [Description("Add desired tag")]
        public async Task AddTag(CommandContext ctx, [RemainingText] string args)
        {
            string m = null;
            try
            {
                using var db = new AppDbContext();
                var tags = db.Tags.FirstOrDefault();
                await ctx.Channel.SendMessageAsync("```this feature is a wip [work in progress] the devs are hard at work getting this feature working!```");
            }
            catch(Exception ex)
            {
                 m = ex.Message;
                
            }

            if (m != null)
            {
                await ctx.Channel.SendMessageAsync($"```An Error Occured making the request ERROR MESSAGE {m}").ConfigureAwait(false);
            }

 
        }
        #endregion
    }
}
