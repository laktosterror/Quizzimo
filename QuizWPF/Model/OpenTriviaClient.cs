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

namespace QuizWPF.Model
{
    public class OpenTriviaClient
    {
        readonly HttpClient httpClient = new();
        readonly string OpenTriviaUriBase = "https://opentdb.com/";
        readonly string OpenTriviaUriTokenPath = "api_token.php?command=request";
        readonly string OpenTriviaUriCategoryPath = "api_category.php";
        string Token = string.Empty;
        public Trivia_Categories[] Categories;
        public int SelectedAmount;
        public string SelectedCategory;
        public string SelectedDifficulty;


        public async Task LoadTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, (OpenTriviaUriBase + OpenTriviaUriTokenPath));
            //request.Headers.Add("Cookie", "PHPSESSID=2ff588d198ce1cae1baf38d6ad3869b9");
            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var openTriviaToken = JsonSerializer.Deserialize<OpenTriviaToken>(responseString);
            Token = openTriviaToken.token;

            await Task.Delay(10000);

        }

        public async Task LoadCategoriesAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, (OpenTriviaUriBase + OpenTriviaUriCategoryPath));
            //request.Headers.Add("Cookie", "PHPSESSID=2ff588d198ce1cae1baf38d6ad3869b9");
            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var openTriviaCategories = JsonSerializer.Deserialize<OpenTriviaCategories>(responseString);

            Categories = openTriviaCategories.trivia_categories;

        }

        public async Task<OpenTriviaQuestions> GetQuestions()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, (OpenTriviaUriBase + $"api.php?amount={SelectedAmount}&category={SelectedCategory}&difficulty={SelectedDifficulty}&type=multiple&token={Token}"));
            //request.Headers.Add("Cookie", "PHPSESSID=2ff588d198ce1cae1baf38d6ad3869b9");
            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var openTriviaquestions = JsonSerializer.Deserialize<OpenTriviaQuestions>(responseString);
            return openTriviaquestions;
        }
    }
}