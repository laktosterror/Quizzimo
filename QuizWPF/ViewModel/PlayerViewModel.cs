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
        PlayerBackground = "#202937";
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
            LoadNextQuestion();
        }
    }

    private async void AnswerButton(object obj)
    {
        if ((string)obj == ActiveQuestion.CorrectAnswer)
        {
            AmountOfCorrectAnswers++;
            await FlashBackgroundGreen();
        }
        else
        {
            await FlashBackgroundRed();
        }

        LoadNextQuestion();
    }

    public void Timer_Tick(object sender, EventArgs e)
    {
        TimeLeft--;

        if (TimeLeft <= 0)
        {
            LoadNextQuestion();
        }
    }

    private async Task FlashBackgroundGreen()
    {
        for (int i = 0; i < 1; i++)
        {
            PlayerBackground = "Green";
            await Task.Delay(2000);
            PlayerBackground = "#202937";
            await Task.Delay(50);
        }
    }

    private async Task FlashBackgroundRed()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerBackground = "Red";
            await Task.Delay(150);
            PlayerBackground = "#202937";
            await Task.Delay(150);
        }
    }

    public void ShuffleAnswersForActiveQuestion(Question question)
    {
        Random random = new();
        var allAnswers = question.IncorrectAnswers.ToList();
        allAnswers.Add(question.CorrectAnswer);
        var randomizedList = allAnswers.OrderBy(x => random.Next()).ToList();
        ShuffeledAnswers = new ObservableCollection<string>(randomizedList);
    }

    public void LoadNextQuestion()
    {
        if (IndexOfActiveQuestion < ActivePack.Questions.Count)
        {
            IndexOfActiveQuestion++;
            TimeLeft = ActivePack.TimeLimitSeconds;
            ActiveQuestion = ActivePack.Questions[IndexOfActiveQuestion -1];
            ShuffleAnswersForActiveQuestion(ActiveQuestion);
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