﻿namespace QuizWPF.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    
    
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this._mainWindowViewModel = mainWindowViewModel;
    }
}