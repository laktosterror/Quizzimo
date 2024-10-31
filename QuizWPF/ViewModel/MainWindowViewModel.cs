using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using QuizWPF.Command;
using QuizWPF.Model;
using QuizWPF.View;


namespace QuizWPF.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public DelegateCommand ShowPlayViewCommand { get; }
    public DelegateCommand ShowConfigurationViewCommand { get; }
    public DelegateCommand ExitApplicationCommand { get; }
    public DelegateCommand ShowMenuCommand { get; }
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
    public FileReader FileReader {get; set;}
    public ConfigurationViewModel ConfigurationViewModel { get; }
    public PlayerViewModel? PlayerViewModel { get; }
    private UserControl? _currentView;
    private QuestionPackViewModel? _activePack;
    private Visibility _collapsibleMenuVisibility;

    public MainWindowViewModel()
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);
        PlayerViewModel = new PlayerViewModel(this);

        FileReader = new FileReader(@"./data.json");
        Packs = FileReader.ReadFromFile();

        CollapsibleMenuVisibility = Visibility.Collapsed;
        CurrentView = new PlayerView();

        ActivePack = Packs.FirstOrDefault();
        
        ShowPlayViewCommand = new DelegateCommand(ShowPlayView);
        ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);
        ExitApplicationCommand = new DelegateCommand(ExitApplication);
        ShowMenuCommand = new DelegateCommand(ShowMenu);
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
    public Visibility CollapsibleMenuVisibility
    {
        get => _collapsibleMenuVisibility;
        set
        {
            _collapsibleMenuVisibility = value;
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

            ResetView();
        }
    }
    private void ShowPlayView(object obj)
    {
        CurrentView = new PlayerView();
    }
    private void ShowConfigurationView(object obj)
    {
        CurrentView = new ConfigurationView();
    }
    private void ShowMenu(object obj)
    {
        if (CollapsibleMenuVisibility == Visibility.Visible)
        {
            CollapsibleMenuVisibility = Visibility.Collapsed;
        }
        else
        {
            CollapsibleMenuVisibility = Visibility.Visible;
        }
    }

    public void HideMenu()
    {
        CollapsibleMenuVisibility = Visibility.Collapsed;
    }

    public void ResetView()
    {
        if (CurrentView is ConfigurationView)
        {
            CurrentView = new ConfigurationView();
        }
        else
        {
            CurrentView = new PlayerView();
        }
    }

    private void ExitApplication(object obj)
    {
        FileReader.WriteToFile(Packs);
        Application.Current.Shutdown();
    }
}