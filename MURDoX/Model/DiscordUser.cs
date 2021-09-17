using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Model
{
   public class DiscordUser
    {
        public int Id { get; set; }
        public ulong DiscordId { get; set; }
        public string Username { get; set; }
        public Rank Rank { get; set; }
        public string AvatarUrl { get; set; }
        public int Warnings { get; set; }
        public int Thanks { get; set; }
        public int XP { get; set; }
        public int BankAccountTotal { get; set; }
        public DateTime Created { get; set; }
    }
}
