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
        ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
        ActivePack.Questions.Add(new Question("Whats a question?", "correct", "wrong", "wrong", "wrong"));
        ActivePack.Questions.Add(new Question("test2", "correct2", "wrong2", "wrong2", "wrong2"));
        ActivePack.Questions.Add(new Question("test3", "correct3", "wrong3", "wrong3", "wrong3"));
        Packs = [];
        Packs.Add(ActivePack);
        ConfigurationViewModel = new ConfigurationViewModel(this);
        PlayerViewModel = new PlayerViewModel(this);

        CurrentView = new PlayerView();
        CollapsibleMenuVisibility = Visibility.Collapsed;

        FileReader = new FileReader(@"./data.json");
        //Packs = FileReader.ReadFromFile();
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

    private void ExitApplication(object obj)
    {
        Application.Current.Shutdown(); // This will close the application
    }
}