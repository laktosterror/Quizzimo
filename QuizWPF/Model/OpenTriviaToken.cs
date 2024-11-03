using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWPF.Model
{
    public class OpenTriviaToken
    {
        public int response_code { get; set; }
        public string response_message { get; set; }
        public string token { get; set; }
    }
}
