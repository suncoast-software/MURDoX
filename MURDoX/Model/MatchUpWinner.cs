using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Model
{
    public class MatchUpWinner
    {
        public int Id { get; set; }
        public string WinnerTeam { get; set; }
        public string Opponent { get; set; }
        public string PlayerPick { get; set; }
        public string WinTeamLogoUrl { get; set; }
        public string OpponentLogoUrl { get; set; }
        public string PlayerPickLogoUrl { get; set; }
        public string WinRecord { get; set; }
        public string WinnerScore { get; set; }
        public string OpponentRecord { get; set; }
        public string PlayerPickRecord { get; set; }
        public string Win { get; set; }

        public MatchUpWinner(string winnerTeam, string playerPick, string winTeamLogoUrl, string playerPickLogoUrl, string win)
        {
            WinnerTeam = winnerTeam;
            PlayerPick = playerPick;
            WinTeamLogoUrl = winTeamLogoUrl;
            PlayerPickLogoUrl = playerPickLogoUrl;
            Win = win;
        }

        public MatchUpWinner(string winnerTeam, string opponent, string winTeamLogoUrl, string opponentLogoUrl,
            string winRecord, string winnerScore, string opponentRecord, string win)
        {
            WinnerTeam = winnerTeam;
            Opponent = opponent;
            WinTeamLogoUrl = winTeamLogoUrl;
            OpponentLogoUrl = opponentLogoUrl;
            WinRecord = winRecord;
            WinnerScore = winnerScore;
            OpponentRecord = opponentRecord;
            Win = win;
        }

        public MatchUpWinner(string winnerTeam, string playerPick, string winTeamLogoUrl, string playerPickLogoUrl, string winRecord, string playerPickRecord, string win)
        {
            WinnerTeam = winnerTeam;
            PlayerPick = playerPick;
            WinTeamLogoUrl = winTeamLogoUrl;
            PlayerPickLogoUrl = playerPickLogoUrl;
            WinRecord = winRecord;
            PlayerPickRecord = playerPickRecord;
            Win = win;
        }
    }
}
