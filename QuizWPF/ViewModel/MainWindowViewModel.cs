using System.Collections.ObjectModel;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

    public ConfigurationViewModel ConfigurationViewModel { get; }
    public PlayerViewModel? PlayerViewModel { get; }

    private QuestionPackViewModel? _activePack;
    
    public QuestionPackViewModel? ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);
        
        PlayerViewModel = new PlayerViewModel(this);
        
        ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
    }
}