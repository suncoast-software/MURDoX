using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Model
{
   public class Embed
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Desc { get; set; }
        public string Footer { get; set; }
        public string AuthorAvatar { get; set; }
        public string ImgUrl { get; set; }
        public string ThumbnailImgUrl { get; set; }
        public string FooterImgUrl { get; set; }
        public string Color { get; set; }
        public EmbedField[] Fields { get; set; }
    }
}
