using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using QuizWPF.Command;
using QuizWPF.Model;

namespace QuizWPF.ViewModel
{
    public class ImporterViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;
        public DispatcherTimer Timer { get; }


        public DelegateCommand ImportQuestionsCommand { get; }
        public ObservableCollection<Difficulty> Difficulties { get; }

        OpenTriviaClient OpenTriviaClient { get; }



        public ImporterViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;

            OpenTriviaClient = new OpenTriviaClient();
            CheckInternetConnection();
            LoadCategoriesAsync();

            Difficulties = new ObservableCollection<Difficulty>(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());
            SelectedAmountOfQuestions = 5;
            ImportQuestionsCommand = new DelegateCommand(ImportQuestions);

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(10);
            Timer.Tick += ConnectionTick;
            Timer.Start();
        }

        private async void ImportQuestions(object obj)
        {
            if (IsOnline)
            {
                var downloadedQuestions = await OpenTriviaClient.GetQuestionsAsync(SelectedAmountOfQuestions, SelectedTriviaCategory.id, SelectedDifficulty.ToString());

                foreach (var question in downloadedQuestions.results)
                {
                    // Decode the HTML encoded strings
                    string decodedQuestion = WebUtility.HtmlDecode(question.question);
                    string decodedCorrectAnswer = WebUtility.HtmlDecode(question.correct_answer);
                    string[] decodedIncorrectAnswers = question.incorrect_answers.Select(WebUtility.HtmlDecode).ToArray();

                    // Add the decoded question and answers to the ActivePack
                    ActivePack?.Questions.Add(new Question(decodedQuestion, decodedCorrectAnswer, decodedIncorrectAnswers));
                }
            }
        }

        private async void LoadCategoriesAsync()
        {
            if (IsOnline)
            {
                TriviaCategories = await Task.Run(() => OpenTriviaClient.LoadCategoriesAsync());
                SelectedTriviaCategory = TriviaCategories.trivia_categories.FirstOrDefault();
            }
        }

        private void ConnectionTick(object sender, EventArgs e)
        {
            CheckInternetConnection();
        }


        private void CheckInternetConnection()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set a timeout for the request
                    client.Timeout = TimeSpan.FromSeconds(5);

                    // Send a GET request to a reliable website
                    HttpResponseMessage response = client.GetAsync("http://www.google.com").Result;

                    IsOnline = true;
                }
            }
            catch
            {
                IsOnline = false;
            }
        }

        private bool _isOnline;
        public bool IsOnline
        {
            get => _isOnline;
            set 
            { 
                _isOnline = value;
                RaisePropertyChanged();
            }
        }

        private int _selectedAmountOfQuestions;
        public int SelectedAmountOfQuestions
        {
            get => _selectedAmountOfQuestions;
            set 
            { 
                _selectedAmountOfQuestions = value;
                RaisePropertyChanged();
            }
        }

        private OpenTriviaCategories _triviaCategories;
        public OpenTriviaCategories TriviaCategories
        {
            get => _triviaCategories;
            set
            {
                _triviaCategories = value;
                RaisePropertyChanged();
            }
        }

        private Trivia_Categories _selectedTriviaCategory;

        public Trivia_Categories SelectedTriviaCategory
        {
            get => _selectedTriviaCategory;
            set
            { 
                _selectedTriviaCategory = value;
                RaisePropertyChanged();
            }
        }

        private Difficulty _selectedDifficulty;

        public Difficulty SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                _selectedDifficulty = value;
                RaisePropertyChanged();
            }
        }
    }
}
