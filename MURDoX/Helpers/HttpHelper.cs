using HtmlAgilityPack;
using MURDoX.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace MURDoX.Helpers
{
    public class HttpHelper
    {
        public static string EndPoint { get; set; }

        #region MAKE QUESTION REQUEST
        public static string MakeQuestionRequest(string category, string difficulty)
        {
            EndPoint = "https://opentdb.com/api.php?amount=10&category=" + category + "&difficulty=" + difficulty + "&type=multiple";

            string responseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint);
            request.Method = HttpMethod.Get.ToString();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException(String.Format("Error Code: {0}", response.StatusCode.ToString()));
                }

                using Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using StreamReader reader = new(responseStream);
                    responseValue = reader.ReadToEnd();
                }
            }
            return responseValue;
        }
        #endregion

        #region HANDLE QUESTION RESPONSE
        public static List<Question> HandleQuestionResponse(string response)
        {
            dynamic quests = JsonConvert.DeserializeObject(response);
            List<Question> Questions = new();

            if (quests != null)
            {
                foreach (var quest in quests["results"])
                {
                    Question question = new();
                    question.Category = quest.category;
                    question._Question = quest.question;
                    question.Type = quest.type;
                    question.Difficulty = quest.difficulty;
                    question.CorrectAnswer = quest.correct_answer;
                    question.Answers = quest.incorrect_answers;
                    Questions.Add(question);
                }
            }
            return Questions;
        }
        #endregion

        #region GET MEME URL
        public static string GetMemeUrl(string query)
        {
            var rnd = new Random();
            string url = "http://results.dogpile.com/serp?qc=images&q=" + query + "meme";
            HtmlWeb page = new();
            HtmlDocument doc = page.Load(url);

            HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//div[@class='image']/a/img");

            int index = rnd.Next(urls.Count);
            var memeUrl = urls[index].Attributes["src"].Value;

            return memeUrl;
        }
        #endregion
    }
}
