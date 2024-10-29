using System.Collections.ObjectModel;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public ObservableCollection<QuestionPackViewModel> Packs => _mainWindowViewModel.Packs;


    private QuestionPackViewModel? _selectedPack;
    private Question? _selectedQuestion;
    
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this._mainWindowViewModel = mainWindowViewModel;
    }

    public QuestionPackViewModel SelectedPack
    {
        get { return _selectedPack; }
        set
        {
            _selectedPack = value;
            RaisePropertyChanged();
        }
    }
    public Question SelectedQuestion
    {
        get { return _selectedQuestion; }
        set
        {
            _selectedQuestion = value;
            RaisePropertyChanged();
        }
    }
}