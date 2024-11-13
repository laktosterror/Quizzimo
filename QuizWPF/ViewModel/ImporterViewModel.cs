using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Windows.Threading;
using QuizWPF.Command;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class ImporterViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    private bool _isOnline;

    private int _selectedAmountOfQuestions;

    private Difficulty _selectedDifficulty;

    private Trivia_Categories _selectedTriviaCategory;

    private OpenTriviaCategories _triviaCategories;

    private bool PreviousIsOnlineState;


    public ImporterViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

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

    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;
    public DispatcherTimer Timer { get; }

    public DelegateCommand ImportQuestionsCommand { get; }
    public ObservableCollection<Difficulty> Difficulties { get; }

    private OpenTriviaClient OpenTriviaClient { get; }

    public bool IsOnline
    {
        get => _isOnline;
        set
        {
            _isOnline = value;
            RaisePropertyChanged();
        }
    }

    public int SelectedAmountOfQuestions
    {
        get => _selectedAmountOfQuestions;
        set
        {
            _selectedAmountOfQuestions = value;
            RaisePropertyChanged();
        }
    }

    public OpenTriviaCategories TriviaCategories
    {
        get => _triviaCategories;
        set
        {
            _triviaCategories = value;
            RaisePropertyChanged();
        }
    }

    public Trivia_Categories SelectedTriviaCategory
    {
        get => _selectedTriviaCategory;
        set
        {
            _selectedTriviaCategory = value;
            RaisePropertyChanged();
        }
    }

    public Difficulty SelectedDifficulty
    {
        get => _selectedDifficulty;
        set
        {
            _selectedDifficulty = value;
            RaisePropertyChanged();
        }
    }

    private async void ImportQuestions(object obj)
    {
        if (IsOnline)
            try
            {
                var openTriviaResponse = await OpenTriviaClient.GetQuestionsAsync(SelectedAmountOfQuestions,
                    SelectedTriviaCategory.id, SelectedDifficulty.ToString());

                switch (openTriviaResponse.response_code)
                {
                    case 0:
                        foreach (var question in openTriviaResponse.results)
                        {
                            // Decode the HTML encoded strings
                            var decodedQuestion = WebUtility.HtmlDecode(question.question);
                            var decodedCorrectAnswer = WebUtility.HtmlDecode(question.correct_answer);
                            string[] decodedIncorrectAnswers =
                                question.incorrect_answers.Select(WebUtility.HtmlDecode).ToArray();

                            // Add the decoded question and answers to the ActivePack
                            ActivePack?.Questions.Add(new Question(decodedQuestion, decodedCorrectAnswer,
                                decodedIncorrectAnswers));
                        }

                        _mainWindowViewModel.ShowSuccessSnackbarMessage("Success!", "Questions imported successfully!");

                        break;
                    case 1:
                        _mainWindowViewModel.ShowWarningSnackbarMessage("Warning",
                            "Could not return results. The API doesn't have enough questions for your query.");
                        break;
                    case 2:
                        _mainWindowViewModel.ShowWarningSnackbarMessage("Warning",
                            "Contains an invalid parameter. Arguments passed in aren't valid.");
                        break;
                    case 3:
                        _mainWindowViewModel.ShowWarningSnackbarMessage("Warning", "Session Token does not exist.");
                        break;
                    case 4:
                        _mainWindowViewModel.ShowWarningSnackbarMessage("Warning",
                            "Session Token has returned all possible questions for the specified query. Resetting the Token is necessary.");
                        break;
                    case 5:
                        _mainWindowViewModel.ShowWarningSnackbarMessage("Warning",
                            "Too many requests have occurred. Each IP can only access the API once every 5 seconds.");
                        break;
                    default:
                        _mainWindowViewModel.ShowErrorSnackbarMessage("Error", "An unknown error occurred.");
                        break;
                }
            }
            catch (Exception e)
            {
                _mainWindowViewModel.ShowErrorSnackbarMessage("Error", e.Message);
            }
        else
            _mainWindowViewModel.ShowErrorSnackbarMessage("Failure",
                "Failed to import question from OpenTDB! You are not online!");
    }

    private async void LoadCategoriesAsync()
    {
        if (IsOnline)
        {
            TriviaCategories = await Task.Run(() => OpenTriviaClient.LoadCategoriesAsync());
            SelectedTriviaCategory = TriviaCategories.trivia_categories.FirstOrDefault();
        }
        else
        {
            _mainWindowViewModel.ShowErrorSnackbarMessage("Failure",
                "Failed to get categories from OpenTDB! You are not online!");
        }
    }

    private void ConnectionTick(object sender, EventArgs e)
    {
        CheckInternetConnection();

        if (IsOnline && SelectedTriviaCategory == null) LoadCategoriesAsync();

        if (IsOnline && !PreviousIsOnlineState)
            _mainWindowViewModel.ShowSuccessSnackbarMessage("Success", "You are online again!");

        if (!IsOnline && PreviousIsOnlineState)
            _mainWindowViewModel.ShowErrorSnackbarMessage("Failure", "You are offline!");
    }


    private void CheckInternetConnection()
    {
        try
        {
            using (var client = new HttpClient())
            {
                // Set a timeout for the request
                client.Timeout = TimeSpan.FromSeconds(5);

                // Send a GET request to a reliable website
                var response = client.GetAsync("http://www.google.com").Result;

                PreviousIsOnlineState = IsOnline;
                IsOnline = true;
            }
        }
        catch
        {
            PreviousIsOnlineState = IsOnline;
            IsOnline = false;
        }
    }
}