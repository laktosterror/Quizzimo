using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWPF.Model
{
    public class OpenTriviaCategories
    {
        public Trivia_Categories[] trivia_categories { get; set; }
    }

    public class Trivia_Categories
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
