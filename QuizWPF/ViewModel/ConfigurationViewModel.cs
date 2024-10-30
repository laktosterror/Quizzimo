using System.Collections.ObjectModel;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public ObservableCollection<QuestionPackViewModel>? Packs => _mainWindowViewModel.Packs;
    public ObservableCollection<Difficulty> Difficulties { get; }
    public QuestionPackViewModel? ActivePack => _mainWindowViewModel?.ActivePack;
    private Question? _selectedQuestion;
    
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this._mainWindowViewModel = mainWindowViewModel;
        Difficulties = new ObservableCollection<Difficulty>(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());

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
}