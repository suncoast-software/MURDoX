using HtmlAgilityPack;
using MURDoX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services
{
  public class SportDataService
    {
        /// <summary>
        /// load selected week MatchUps
        /// </summary>
        /// <param name="week"></param>
        /// <returns>List</returns>
        #region LOAD MatchUpS
        public static List<MatchUp> Load_MatchUps(string year, string week)
        {
            string url = "https://www.footballdb.com/scores/index.html?lg=NFL&yr=" + year.Trim() + "&type=reg&wk=" + week.Trim();

            List<MatchUp> MatchUps = new();
            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection gameNodes = doc.DocumentNode.SelectNodes("//table//tbody//tr");

            for (int i = 0; i < gameNodes.Count - 1; i += 2)
            {
                //AwayTeam
                string[] awayDetails = gameNodes[i].ChildNodes[1].InnerText.Split('(');
                string awayRecord = awayDetails[1].Replace(")", "");
                string awayTeamName = awayDetails[0].Trim();
                // string awayFinalScore = gameNodes[i].ChildNodes[7].InnerText;
                string awayAbr = GetTeamAbr(awayTeamName.Trim());
                string awayLogoUrl = "~/img/" + awayAbr + ".png";

                //HomeTeam
                string[] homeDetails = gameNodes[i + 1].ChildNodes[1].InnerText.Split('(');
                string homeRecord = homeDetails[1].Replace(")", "");
                string homeTeamName = homeDetails[0].Trim();
                //string homeFinalScore = gameNodes[i + 1].ChildNodes[7].InnerText;
                string homeAbr = GetTeamAbr(homeTeamName.Trim());
                string homeLogoUrl = "~/img/" + homeAbr + ".png";

                MatchUp MatchUp = new(awayTeamName, homeTeamName, awayAbr, homeAbr, awayLogoUrl, homeLogoUrl,
                    0, 0, awayRecord, homeRecord, week, "2019");

                MatchUps.Add(MatchUp);
            }

            return MatchUps;
        }
        #endregion
        /// <summary>
        /// Get current week winners
        /// </summary>
        /// <returns>IEnumerable Team</returns>
        #region GET CURRENT WEEK WINNERS
        public static List<MatchUpWinner> Get_Current_Week_Winners()
        {
            List<MatchUpWinner> winners = new();
            string week = Get_Current_Week();
            string year = DateTime.Now.Year.ToString();
            // Uri url = new Uri("https://www.footballdb.com/scores/index.html");
            Uri url = new("https://www.footballdb.com/scores/index.html?lg=NFL&yr=" + year + "&type=reg&wk=" + week);

            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection gameNodes = doc.DocumentNode.SelectNodes("//table");

            foreach (var node in gameNodes)
            {
                if (node.ChildNodes[3].ChildNodes[1].ChildNodes.Count > 4)
                {
                    string awayTeam = node.ChildNodes[3].ChildNodes[1].ChildNodes[1].InnerText;
                    string homeTeam = node.ChildNodes[3].ChildNodes[3].ChildNodes[1].InnerText;
                    string awayScore = node.ChildNodes[3].ChildNodes[1].ChildNodes[7].InnerText;
                    string homeScore = node.ChildNodes[3].ChildNodes[3].ChildNodes[7].InnerText;

                    int recordIndex = awayTeam.IndexOf('(');
                    string awayRecord = awayTeam.Substring(recordIndex);
                    string awayTeamName = awayTeam.Replace(awayRecord, "").Trim();
                    recordIndex = homeTeam.IndexOf('(');
                    string homeRecord = homeTeam.Substring(recordIndex);
                    string homeTeamName = homeTeam.Replace(homeRecord, "").Trim();


                    bool awayScoreResult = int.TryParse(awayScore, out int aScore);
                    bool homeScoreResult = int.TryParse(homeScore, out int hScore);

                    if (awayScoreResult && homeScoreResult)
                    {
                        if (aScore > hScore)
                        {
                            string winnerAbr = GetTeamAbr(awayTeamName);
                            string opponentAbr = GetTeamAbr(homeTeamName);
                            string winnerLogoUrl = String.Format("/img/{0}.png", winnerAbr);
                            string opponentLogoUrl = String.Format("/img/{0}.png", opponentAbr);
                            winners.Add(new MatchUpWinner(awayTeamName, homeTeamName, winnerLogoUrl, opponentLogoUrl, awayRecord, aScore.ToString(), homeRecord, ""));

                        }
                        else
                        {
                            string winnerAbr = GetTeamAbr(awayTeamName);
                            string opponentAbr = GetTeamAbr(homeTeamName);
                            string winnerLogoUrl = String.Format("/img/{0}.png", winnerAbr);
                            string opponentLogoUrl = String.Format("/img/{0}.png", opponentAbr);
                            winners.Add(new MatchUpWinner(homeTeamName, awayTeamName, winnerLogoUrl, opponentLogoUrl, homeRecord, hScore.ToString(), awayRecord, ""));
                        }

                    }
                }

            }

            return winners;
        }
        #endregion

        /// <summary>
        /// Get the selected week scores
        /// </summary>
        /// <param name="week"></param>
        /// <returns>List</returns>
        #region GET WEEK SCORES
        public static List<MatchUp> Get_Week_Scores(string year, string week)
        {
            List<MatchUp> scores = new();
            string url = "https://www.footballdb.com/scores/index.html?lg=NFL&yr=" + year + "&type=reg&wk=" + week;

            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection gameNodes = doc.DocumentNode.SelectNodes("//table//tbody//tr");

            for (int i = 0; i < gameNodes.Count - 1; i += 2)
            {

                if (gameNodes[i].ChildNodes.Count == 9)
                {
                    string[] awayDetails = gameNodes[i].ChildNodes[1].InnerText.Split("(");
                    string awayName = awayDetails[0].Trim();
                    string awayAbr = GetTeamAbr(awayName);
                    string awayLogo = "/img/" + awayAbr + ".png";
                    string awayRecord = awayDetails[1].Replace(")", "");
                    string awayScore = gameNodes[i].ChildNodes[8].InnerText;
                    string[] homeDetails = gameNodes[i + 1].ChildNodes[1].InnerText.Split('(');
                    string homeName = homeDetails[0].Trim();
                    string homeAbr = GetTeamAbr(homeName);
                    string homeLogo = "/img/" + homeAbr + ".png";
                    string homeRecord = homeDetails[1].Replace(")", "");
                    string homeScore = gameNodes[i + 1].ChildNodes[8].InnerText;
                    bool aScoreResult;
                    bool hScoreResult;

                    aScoreResult = int.TryParse(awayScore, out int aScore);
                    hScoreResult = int.TryParse(homeScore, out int hScore);

                    if (aScoreResult && hScoreResult)
                    {
                        MatchUp score = new(awayName, homeName, awayAbr, homeAbr, awayLogo, homeLogo, aScore, hScore,
                           awayRecord, homeRecord, week, "2019");

                        scores.Add(score);
                    }
                    else
                    {
                        MatchUp score = new(awayName, homeName, awayAbr, homeAbr, awayLogo, homeLogo, 0, 0,
                            awayRecord, homeRecord, week, "2019");
                        scores.Add(score);
                    }

                }
                else if (gameNodes[i].ChildNodes.Count > 4)
                {
                    string[] awayDetails = gameNodes[i].ChildNodes[1].InnerText.Split("(");
                    string awayName = awayDetails[0].Trim();
                    string awayAbr = GetTeamAbr(awayName);
                    string awayLogo = "/img/" + awayAbr + ".png";
                    string awayRecord = awayDetails[1].Replace(")", "");
                    string awayScore = gameNodes[i].ChildNodes[7].InnerText;
                    string[] homeDetails = gameNodes[i + 1].ChildNodes[1].InnerText.Split('(');
                    string homeName = homeDetails[0].Trim();
                    string homeAbr = GetTeamAbr(homeName);
                    string homeLogo = "/img/" + homeAbr + ".png";
                    string homeRecord = homeDetails[1].Replace(")", "");
                    string homeScore = gameNodes[i + 1].ChildNodes[7].InnerText;
                    bool aScoreResult;
                    bool hScoreResult;

                    aScoreResult = int.TryParse(awayScore, out int aScore);
                    hScoreResult = int.TryParse(homeScore, out int hScore);

                    if (aScoreResult && hScoreResult)
                    {
                        MatchUp score = new(awayName, homeName, awayAbr, homeAbr, awayLogo, homeLogo, aScore, hScore,
                           awayRecord, homeRecord, week, "2019");

                        scores.Add(score);
                    }
                    else
                    {
                        MatchUp score = new(awayName, homeName, awayAbr, homeAbr, awayLogo, homeLogo, 0, 0,
                            awayRecord, homeRecord, week, "2019");
                        scores.Add(score);
                    }

                }
                else
                {
                    string[] awayDetails = gameNodes[i].ChildNodes[1].InnerText.Split("(");
                    string awayName = awayDetails[0].Trim();
                    string awayAbr = GetTeamAbr(awayName);
                    string awayLogo = "/img/" + awayAbr + ".png";
                    string awayRecord = awayDetails[1].Replace(")", "");
                    string awayScore = gameNodes[i].ChildNodes[3].InnerText;
                    string[] homeDetails = gameNodes[i + 1].ChildNodes[1].InnerText.Split('(');
                    string homeName = homeDetails[0].Trim();
                    string homeAbr = GetTeamAbr(homeName);
                    string homeLogo = "/img/" + homeAbr + ".png";
                    string homeRecord = homeDetails[1].Replace(")", "");
                    string homeScore = gameNodes[i + 1].ChildNodes[3].InnerText;

                    bool aScoreResult;
                    bool hScoreResult;

                    aScoreResult = int.TryParse(awayScore, out int aScore);
                    hScoreResult = int.TryParse(homeScore, out int hScore);

                    if (aScoreResult && hScoreResult)
                    {
                        MatchUp score = new(awayName, homeName, awayAbr, homeAbr, awayLogo, homeLogo, aScore, hScore,
                          awayRecord, homeRecord, week, "2019");
                        scores.Add(score);
                    }
                    else
                    {
                        MatchUp score = new(awayName, homeName, awayAbr, homeAbr, awayLogo, homeLogo, 0, 0,
                         awayRecord, homeRecord, week, "2019");
                        scores.Add(score);
                    }

                }

            }
            return scores;
        }
        #endregion

        /// <summary>
        /// Get the current week 
        /// </summary>
        /// <returns>string</returns>
        #region GET CURRENT WEEK
        public static string Get_Current_Week()
        {
            string pageUrl = "https://www.footballdb.com/scores/index.html";

            HtmlWeb page = new();
            HtmlDocument doc = page.Load(pageUrl);

            string[] weekDetails = doc.DocumentNode.SelectSingleNode("//div[@id='leftcol']//h1").InnerText.Split('-');
            string week = weekDetails[1].Replace("Week", "").Trim();

            //return week;
            return week;
        }
        #endregion

        /// <summary>
        /// returns the requested team's record
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns>string</returns>
        #region GET TEAM RECORD
        public static string Get_Team_Record(string teamName)
        {
            string name = Convert_Team_Name(teamName);
            string url = "https://www.footballdb.com/teams/nfl/" + name;
            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            string banner = doc.DocumentNode.SelectSingleNode("//div[@id='teambanner']//b").InnerText;
            string[] details = banner.Split(new char[] { ':', '(' });

            return details[1];
        }
        #endregion

        /// <summary>
        /// get the requested team's abbreviation
        /// </summary>
        /// <param name="team"></param>
        /// <returns>string</returns>
        #region GET TEAM ABRREVIATION
        public static string GetTeamAbr(string team)
        {
            return team switch
            {
                "Arizona" => "ARI",
                "Atlanta" => "ATL",
                "Baltimore" => "BAL",
                "Buffalo" => "BUF",
                "Carolina" => "CAR",
                "Chicago" => "CHI",
                "Cincinnati" => "CIN",
                "Cleveland" => "CLE",
                "Dallas" => "DAL",
                "Denver" => "DEN",
                "Detroit" => "DET",
                "Green Bay" => "GB",
                "Houston" => "HOU",
                "Indianapolis" => "IND",
                "Jacksonville" => "JAX",
                "Kansas City" => "KC",
                "LA Chargers" => "LAC",
                "LA Rams" => "LAR",
                "Miami" => "MIA",
                "Minnesota" => "MIN",
                "New England" => "NE",
                "New Orleans" => "NO",
                "NY Giants" => "NYG",
                "NY Jets" => "NYJ",
                "Oakland" => "OAK",
                "Philadelphia" => "PHI",
                "Pittsburgh" => "PIT",
                "Seattle" => "SEA",
                "San Francisco" => "SF",
                "Tampa Bay" => "TB",
                "Tennessee" => "TEN",
                "Washington" => "WAS",
                _ => "",
            };
        }
        #endregion

        /// <summary>
        /// converts the team name
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns>string</returns>
        #region CONVERT TEAM NAME
        private static string Convert_Team_Name(string teamName)
        {
            return teamName switch
            {
                "Buffalo" => "buffalo-bills",
                "Miami" => "miami-dolphins",
                "New England" => "new-england-patriots",
                "Jets" => "new-york-jets",
                "Ravens" => "baltimore-ravens",
                "Bengals" => "cincinnati-bengals",
                "Browns" => "cleveland-browns",
                "Pittsburgh" => "pittsburgh-steelers",
                "Houston" => "houston-texans",
                "Indianapolis" => "indianapolis-colts",
                "Jacksonville" => "jacksonville-jaguars",
                "Tennessee" => "tennessee-titans",
                "Broncos" => "denver-broncos",
                "Kansas City" => "kansas-city-chiefs",
                "LA Chargers" => "los-angeles-chargers",
                "Oakland" => "oakland-raiders",
                "Cowboys" => "dallas-cowboys",
                "Giants" => "new-york-giants",
                "Philadelphia" => "philadelphia-eagles",
                "Redskins" => "washington-redskins",
                "Chicago" => "chicago-bears",
                "Detroit" => "detroit-lions",
                "Green Bay" => "green-bay-packers",
                "Minnesota" => "minnesota-vikings",
                "Atlanta" => "atlanta-falcons",
                "Panthers" => "carolina-panthers",
                "New Orleans" => "new-orleans-saints",
                "Tampa Bay" => "tampa-bay-buccaneers",
                "Arizona" => "arizona-cardinals",
                "LA Rams" => "los-angeles-rams",
                "San Francisco" => "san-francisco-49ers",
                "Seattle" => "seattle-seahawks",
                _ => "",
            };
        }
        #endregion

        /// <summary>
        /// Get MatchUp Count
        /// </summary>
        /// <param name="week"></param>
        /// <returns>int</returns>
        #region GET MatchUp COUNT   
        public static int Get_Matchhup_Count(string week)
        {
            Uri url = new("https://www.footballdb.com/scores/index.html?lg=NFL&yr=2019&type=reg&wk=" + week);

            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection gameNodes = doc.DocumentNode.SelectNodes("//table//tbody//tr");

            return gameNodes.Count;
        }
        #endregion
    }
}
