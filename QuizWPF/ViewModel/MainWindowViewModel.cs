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
    public DelegateCommand AddPackCommand { get; }
    public DelegateCommand RemovePackCommand { get; }
    public DelegateCommand SetActivePackCommand { get; }
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
    public FileReader FileReader {get; set;}
    public OpenTriviaClient OpenTriviaClient { get; set;}
    public ConfigurationViewModel ConfigurationViewModel { get; set; }
    public ConfigurationView ConfigurationView { get; }
    public PlayerViewModel? PlayerViewModel { get; set; }
    public PlayerView PlayerView { get; }
    private UserControl? _currentView;
    private QuestionPackViewModel? _activePack;
    private Visibility _collapsibleMenuVisibility;

    public MainWindowViewModel()
    {
        FileReader = new FileReader(@"./data.json");
        var loadedPacks = FileReader.ReadFromFileAsync();

        //OpenTriviaClient = new OpenTriviaClient();
        //OpenTriviaClient.LoadTokenAsync();
        //OpenTriviaClient.LoadCategoriesAsync();

        ShowPlayViewCommand = new DelegateCommand(ShowPlayView);
        ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);
        ExitApplicationCommand = new DelegateCommand(ExitApplication);
        ShowMenuCommand = new DelegateCommand(ToggleMenu);
        AddPackCommand = new DelegateCommand(AddPack);
        RemovePackCommand = new DelegateCommand(RemovePack);
        SetActivePackCommand = new DelegateCommand(SetActivePack);

        Packs = loadedPacks.GetAwaiter().GetResult();
        ActivePack = Packs.FirstOrDefault();

        ConfigurationViewModel = new ConfigurationViewModel(this);
        ConfigurationView = new ConfigurationView(ConfigurationViewModel);

        PlayerViewModel = new PlayerViewModel(this);
        PlayerView = new PlayerView(PlayerViewModel);

        CollapsibleMenuVisibility = Visibility.Collapsed;
        CurrentView = PlayerView;
    }

    private void SetActivePack(object obj)
    {
        if (obj is QuestionPackViewModel selectedPack)
        {
            ActivePack = selectedPack; 
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

            ReloadCurrentView();
        }
    }
    private void ShowPlayView(object obj)
    {
        PlayerViewModel = new PlayerViewModel(this);
        CurrentView = new PlayerView(PlayerViewModel);
    }

    public void ShowResultsView()
    {
        CurrentView = new ResultsView(PlayerViewModel);
    }
    private void ShowConfigurationView(object obj)
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);
        CurrentView = new ConfigurationView(ConfigurationViewModel);
        ConfigurationViewModel.AutoSelectFirstQuestion();
    }
    private void ToggleMenu(object obj)
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

    public void ReloadCurrentView()
    {
        if (CurrentView is ConfigurationView)
        {
            ConfigurationViewModel = new ConfigurationViewModel(this);
            CurrentView = new ConfigurationView(ConfigurationViewModel);
            ConfigurationViewModel.AutoSelectFirstQuestion();
        }
        else
        {
            PlayerViewModel = new PlayerViewModel(this);
            CurrentView = new PlayerView(PlayerViewModel);
        }
    }

    private void ExitApplication(object obj)
    {
        FileReader.WriteToFileAsync(Packs).GetAwaiter();
        Application.Current.Shutdown();
    }

    private void RemovePack(object obj)
    {
        Packs.Remove(ActivePack);
        ActivePack = Packs.FirstOrDefault();
    }

    private void AddPack(object obj)
    {
        var newPackModel = new QuestionPack("New Question Pack");
        var newPack = new QuestionPackViewModel(newPackModel);
        newPack.Questions.Add(new Question("Why is the sky so blue?", "Dont worry about it!", "Blue is not a color!", "What about the colorblind?", "Something with light."));
        Packs.Insert(0, newPack);
        ActivePack = Packs.FirstOrDefault();

    }
}