using MURDoX.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            return responseValue;
        }
        #endregion

        #region HANDLE QUESTION RESPONSE
        public static List<Question> HandleQuestionResponse(string response)
        {
            dynamic quests = JsonConvert.DeserializeObject(response);
            List<Question> Questions = new List<Question>();

            if (quests != null)
            {
                foreach (var quest in quests["results"])
                {
                    Question question = new Question();
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
    }
}
