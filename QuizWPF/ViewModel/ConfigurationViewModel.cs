using System.Collections.ObjectModel;
using QuizWPF.Command;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand RemoveQuestionCommand { get; }

    private readonly MainWindowViewModel? _mainWindowViewModel;
    public ObservableCollection<QuestionPackViewModel>? Packs => _mainWindowViewModel?.Packs;
    public ObservableCollection<Difficulty> Difficulties { get; }
    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;

    private Question? _selectedQuestion;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        AddQuestionCommand = new DelegateCommand(AddQuestion);
        RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

        this._mainWindowViewModel = mainWindowViewModel;
        Difficulties = new ObservableCollection<Difficulty>(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());
    }

    public void AutoSelectFirstQuestion()
    {
        if (ActivePack != null)
        {
            SelectedQuestion = ActivePack.Questions.FirstOrDefault();
        }
    }

    public Question? SelectedQuestion
    {
        get { return _selectedQuestion; }
        set
        {
            _selectedQuestion = value;
            RaisePropertyChanged();
        }
    }

    public void AddQuestion(object obj)
    {
        ActivePack.Questions.Insert(0, new Question("New Question", "", "", "", ""));
        SelectedQuestion = ActivePack.Questions.FirstOrDefault();
        _mainWindowViewModel.ReloadCurrentView();
    }

    public void RemoveQuestion(object obj)
    {
        ActivePack.Questions.Remove(SelectedQuestion);
        SelectedQuestion = ActivePack.Questions.FirstOrDefault();
    }
}