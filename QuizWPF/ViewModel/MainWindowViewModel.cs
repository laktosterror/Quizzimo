using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using QuizWPF.Command;
using QuizWPF.Model;
using QuizWPF.View;

namespace QuizWPF.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public DelegateCommand HomeButtonCommand { get; }
    public DelegateCommand PlayButtonCommand { get; }
    public DelegateCommand ExitButtonCommand { get; }
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
    public ConfigurationViewModel ConfigurationViewModel { get; }
    public PlayerViewModel? PlayerViewModel { get; }
    private UserControl _currentView;
    private QuestionPackViewModel? _activePack;
    public MainWindowViewModel()
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);
        PlayerViewModel = new PlayerViewModel(this);
        ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
        CurrentView = new HomeView();
        HomeButtonCommand = new DelegateCommand(HomeButton);
        PlayButtonCommand = new DelegateCommand(PlayButton);
        ExitButtonCommand = new DelegateCommand(ExitButton);
    }
    public UserControl CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            RaisePropertyChanged();
        }
    }

    public QuestionPackViewModel? ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();
        }
    }

    private bool CanUpdateButton()
    {
        return true;
    }

    private void HomeButton(object obj)
    {
        CurrentView = new HomeView();
    }  
    private void PlayButton(object obj)
    {
        CurrentView = new PlayerView();
    }

    private void ExitButton(object obj)
    {
        Application.Current.Shutdown(); // This will close the application
    }
}