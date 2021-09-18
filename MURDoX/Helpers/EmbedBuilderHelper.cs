using DSharpPlus.Entities;
using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Helpers
{
   public class EmbedBuilderHelper
    {
        public DiscordEmbed Build(Embed embed)
        {
            var _embed = new DiscordEmbedBuilder()
                .WithAuthor(embed.Author, "", embed.AuthorAvatar)
                .WithColor(GetColor(embed.Color, DiscordColor.None))
                .WithTitle(embed.Title)
                .WithDescription(embed.Desc)
                .WithImageUrl(embed.ImgUrl)
                .WithThumbnail(embed.ThumbnailImgUrl)
                .WithFooter(embed.Footer, embed.FooterImgUrl);

            if (embed.Fields != null)
            {
                foreach (var field in embed.Fields)
                {
                    _embed.AddField(field.Name, field.Value, field.Inline);
                }
            }

            _embed.Build();

            return _embed;
        }

        private DiscordColor GetColor(string colorName, DiscordColor defaultColor)
        {
            try
            {
                if (BotColors.Colors.ContainsKey(colorName))
                {
                    return BotColors.Colors[colorName];
                }
                else
                    return defaultColor;
            }
            catch (Exception)
            {
                return DiscordColor.Rose;
            }
        }
    }
}
