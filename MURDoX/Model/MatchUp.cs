using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Model
{
   public class MatchUp
    {
        public int Id { get; set; }

        public string AwayTeam { get; set; }
        public string HomeTeam { get; set; }

        public string AwayAbr { get; set; }
        public string HomeAbr { get; set; }

        public string AwayLogoUrl { get; set; }
        public string HomeLogoUrl { get; set; }

        public int AwayScore { get; set; }
        public int HomeScore { get; set; }

        public string AwayRecord { get; set; }
        public string HomeRecord { get; set; }

        public string Week { get; set; }
        public string Year { get; set; }

        public MatchUp()
        {

        }

        public MatchUp(string awayTeam, string homeTeam, string week, string year)
        {
            AwayTeam = awayTeam;
            HomeTeam = homeTeam;
            Week = week;
            Year = year;
        }

        public MatchUp(string awayTeam, string homeTeam, string awayAbr, string homeAbr, string awayLogoUrl, string homeLogoUrl,
            int awayScore, int homeScore, string awayRecord, string homeRecord, string week, string year)
        {
            AwayTeam = awayTeam;
            HomeTeam = homeTeam;
            AwayAbr = awayAbr;
            HomeAbr = homeAbr;
            AwayLogoUrl = awayLogoUrl;
            HomeLogoUrl = homeLogoUrl;
            AwayScore = awayScore;
            HomeScore = homeScore;
            AwayRecord = awayRecord;
            HomeRecord = homeRecord;
            Week = week;
            Year = year;
        }

        public MatchUp(string awayTeam, string homeTeam, int awayScore, int homeScore, string week, string year)
        {
            AwayTeam = awayTeam;
            HomeTeam = homeTeam;
            AwayScore = awayScore;
            HomeScore = homeScore;
            Week = week;
            Year = year;
        }
    }
}
