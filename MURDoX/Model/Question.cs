using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Model
{
   public class Question
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string _Question { get; set; }
        public string CorrectAnswer { get; set; }
        public JArray Answers { get; set; }
        
    }
}
