using System.Collections.ObjectModel;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class QuestionPackViewModel(QuestionPack model) : ViewModelBase
{
    private QuestionPack Model { get; set; } = model;
    public ObservableCollection<Question> Questions { get; set; } = model.Questions;

    public string Name
    {
        get => Model.Name;
        set
        {
            Model.Name = value;
            RaisePropertyChanged();
        }
    }

    public Difficulty Difficulty
    {
        get => Model.Difficulty;
        set
        {
            Model.Difficulty = value;
            RaisePropertyChanged();
        }
    }

    public int TimeLimitSeconds
    {
        get => Model.TimeLimitSeconds;
        set
        {
            Model.TimeLimitSeconds = value;
            RaisePropertyChanged();
        }
    }
}