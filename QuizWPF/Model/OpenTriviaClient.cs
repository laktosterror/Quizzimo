using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Net;
using System.Text.Json.Serialization;
using System.CodeDom;
using System.Net.Mail;

namespace QuizWPF.Model
{
    public class OpenTriviaClient
    {
        readonly HttpClient httpClient = new();
        readonly string OpenTriviaUriBase = "https://opentdb.com/";
        readonly string OpenTriviaUriCategoryPath = "api_category.php";

        public async Task<OpenTriviaCategories> LoadCategoriesAsync()
        {
            var response = await httpClient.GetStringAsync("https://opentdb.com/api_category.php");
            var openTriviaCategories = JsonSerializer.Deserialize<OpenTriviaCategories>(response);
            return openTriviaCategories;
        }

        public async Task<OpenTriviaQuestions> GetQuestionsAsync(int SelectedAmount, int SelectedCategory, string SelectedDifficulty)
        {
            var response = await httpClient
                .GetStringAsync(OpenTriviaUriBase + $"api.php?amount={SelectedAmount}&category={SelectedCategory}&difficulty={SelectedDifficulty.ToLower()}&type=multiple");

            var openTriviaquestions = JsonSerializer.Deserialize<OpenTriviaQuestions>(response);
            return openTriviaquestions;
        }
    }
}