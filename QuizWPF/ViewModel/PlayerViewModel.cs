﻿using QuizWPF.Command;

namespace QuizWPF.ViewModel;

public class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;
    private string _testData = "Start data: ";

    public string TestData
    {
        get => _testData;
        set
        {
            _testData = value;
            RaisePropertyChanged();
        }
    }
    public DelegateCommand UpdateButtonCommand { get; }

    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        UpdateButtonCommand = new DelegateCommand(UpdateButton);
    }

    private bool CanUpdateButton()
    {
        return true;
    }

    private void UpdateButton(object obj)
    {
        TestData += "x";
    }
}