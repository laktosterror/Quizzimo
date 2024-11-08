using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using QuizWPF.Command;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;
    private Question _activeQuestion;
    private bool hasAnswered;

    private ObservableCollection<string> _buttonMouseOverBackgroundColors;
    public ObservableCollection<string> ButtonMouseOverBackgroundColors
    {
        get => _buttonMouseOverBackgroundColors;
        set
        { 
            _buttonMouseOverBackgroundColors = value;
            RaisePropertyChanged();
        }
    }


    private ObservableCollection<string> _buttonBackgroundColors;
    public ObservableCollection<string> ButtonBackgroundColors
    {
        get => _buttonBackgroundColors;
        set 
        { 
            _buttonBackgroundColors = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<string> _shuffeledAnswers;
    public ObservableCollection<string> ShuffeledAnswers
    { 
        get => _shuffeledAnswers;
        set
        {
            _shuffeledAnswers = value;
            RaisePropertyChanged();
        }
    }

    private string _playerBackground;
    public string PlayerBackground
    {
        get => _playerBackground;
        set
        {
            _playerBackground = value;
            RaisePropertyChanged();
        }
    }

    private int _timeLeft;
    private int _indexOfActiveQuestion;
    private int _amountOfCorrectAnswers;

    private DispatcherTimer timer;
    private int _CorrectQuestions;

    public DelegateCommand AnswerButtonCommand { get; }
    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        ButtonMouseOverBackgroundColors = ["DarkOrange", "DarkOrange", "DarkOrange", "DarkOrange"]; 
        PlayerBackground = "#202937";
        ButtonBackgroundColors = ["WhiteSmoke", "WhiteSmoke", "WhiteSmoke", "WhiteSmoke"];
        ShuffeledAnswers = [];
        IndexOfActiveQuestion = 0;
        this._mainWindowViewModel = mainWindowViewModel;

        AnswerButtonCommand = new DelegateCommand(AnswerButton);

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();

        if (ActivePack != null)
        {
            LoadFirstQuestion();
        }
    }

    private async void AnswerButton(object obj)
    {
        if (!hasAnswered)
        {
            hasAnswered = true;

            var index = Convert.ToInt32(obj);
            if (ShuffeledAnswers[index] == ActiveQuestion.CorrectAnswer)
            {
                AmountOfCorrectAnswers++;
            }

            await SetButtonBackgroundColors();
            LoadNextQuestion();
            hasAnswered = false;
        }
    }

    public void Timer_Tick(object sender, EventArgs e)
    {
        TimeLeft--;

        if (TimeLeft <= 0)
        {
            LoadNextQuestion();
        }
    }

    private async Task SetButtonBackgroundColors()
    {
        var indexOfCorrectAnswer = ShuffeledAnswers.IndexOf(ActiveQuestion.CorrectAnswer);

        for (int i = 0; i < ShuffeledAnswers.Count; i++)
        {
            ButtonMouseOverBackgroundColors[i] = "Red";
            ButtonBackgroundColors[i] = "Red";
        }

        ButtonMouseOverBackgroundColors[indexOfCorrectAnswer] = "Green";
        ButtonBackgroundColors[indexOfCorrectAnswer] = "Green";
        await Task.Delay(2000);

        for (int i = 0; i < ShuffeledAnswers.Count; i++)
        {
            ButtonMouseOverBackgroundColors[i] = "DarkOrange";
            ButtonBackgroundColors[i] = "WhiteSmoke";
        }

        await Task.Delay(50);
    }

    public void ShuffleAnswersForActiveQuestion(Question question)
    {
        Random random = new();
        var allAnswers = question.IncorrectAnswers.ToList();
        allAnswers.Add(question.CorrectAnswer);
        var randomizedList = allAnswers.OrderBy(x => random.Next()).ToList();
        ShuffeledAnswers = new ObservableCollection<string>(randomizedList);
    }

    public void LoadFirstQuestion()
    {
        if (IndexOfActiveQuestion < ActivePack.Questions.Count)
        {
            TimeLeft = ActivePack.TimeLimitSeconds;
            ActiveQuestion = ActivePack.Questions[IndexOfActiveQuestion];
            ShuffleAnswersForActiveQuestion(ActiveQuestion);
            IndexOfActiveQuestion++;
        }
    }

    public void LoadNextQuestion()
    {
        if (IndexOfActiveQuestion < ActivePack.Questions.Count)
        {
            TimeLeft = ActivePack.TimeLimitSeconds;
            ActiveQuestion = ActivePack.Questions[IndexOfActiveQuestion -1];
            ShuffleAnswersForActiveQuestion(ActiveQuestion);
            IndexOfActiveQuestion++;
        }
        else
        {
            _mainWindowViewModel.ShowResultsView();
            timer.Stop();
        }
    }

    public int AmountOfCorrectAnswers
    {
        get => _amountOfCorrectAnswers;
        set
        {
            _amountOfCorrectAnswers = value;
            RaisePropertyChanged();
        }
    }

    public int IndexOfActiveQuestion
    {
        get => _indexOfActiveQuestion;
        set
        {
            _indexOfActiveQuestion = value;
            RaisePropertyChanged();
        }
    }


    public int TimeLeft
    {
        get => _timeLeft;
        set
        {
            _timeLeft = value;
            RaisePropertyChanged();
        }
    }

    public Question ActiveQuestion
    {
        get => _activeQuestion;
        set
        {
            _activeQuestion = value;
            RaisePropertyChanged();
        }
    }
}