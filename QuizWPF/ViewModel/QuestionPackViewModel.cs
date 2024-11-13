using System.Collections.ObjectModel;
using QuizWPF.Model;

namespace QuizWPF.ViewModel;

public class QuestionPackViewModel : ViewModelBase
{
    public QuestionPackViewModel(QuestionPack model)
    {
        Model = model;
    }

    public QuestionPack Model { get; set; }
    public ObservableCollection<Question> Questions { get; set; } = [];

    //[JsonConstructor]
    //public QuestionPackViewModel()
    //{
    //    Model = new QuestionPack() { Questions = [] };
    //    
    //}

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