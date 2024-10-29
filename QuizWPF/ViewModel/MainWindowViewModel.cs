using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuizWPF.Command;
using QuizWPF.Model;
using QuizWPF.View;


namespace QuizWPF.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public DelegateCommand ShowHomeViewCommand { get; }
    public DelegateCommand ShowPlayViewCommand { get; }
    public DelegateCommand ShowConfigurationViewCommand { get; }
    public DelegateCommand ExitApplicationCommand { get; }
    public DelegateCommand ShowMenuCommand { get; }
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
    public FileReader FileReader {get; set;}
    public ConfigurationViewModel ConfigurationViewModel { get; }
    public PlayerViewModel? PlayerViewModel { get; }
    private UserControl _currentView;
    private QuestionPackViewModel? _activePack;
    private Visibility _collapsibleMenuVisibility;

    public MainWindowViewModel()
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);
        PlayerViewModel = new PlayerViewModel(this);
        ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
        CurrentView = new PlayerView();
        CollapsibleMenuVisibility = Visibility.Collapsed;
        FileReader = new FileReader(@"./data.json");
        Packs = FileReader.ReadFromFile();
        ShowHomeViewCommand = new DelegateCommand(ShowHomeView);
        ShowPlayViewCommand = new DelegateCommand(ShowPlayView);
        ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);
        ExitApplicationCommand = new DelegateCommand(ExitApplication);
        ShowMenuCommand = new DelegateCommand(ShowMenu);
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

    // Method to hide the menu
    public void HideMenu()
    {
        CollapsibleMenuVisibility = Visibility.Collapsed;
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

    private void ShowHomeView(object obj)
    {
        CurrentView = new HomeView();
    }
    private void ShowPlayView(object obj)
    {
        CurrentView = new PlayerView();
    }
    private void ShowConfigurationView(object obj)
    {
        CurrentView = new ConfigurationView();
    }
    private void ExitApplication(object obj)
    {
        Application.Current.Shutdown(); // This will close the application
    }
}