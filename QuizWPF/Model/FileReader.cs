using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using QuizWPF.ViewModel;

namespace QuizWPF.Model;

public class FileReader(string dataPath)
{
    private readonly string DataPath = dataPath;

    public async Task WriteToFileAsync(ObservableCollection<QuestionPackViewModel> packs)
    {
        try
        {
            var json = JsonSerializer.Serialize(packs, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(DataPath, json);
        }
        catch
        {
            Console.WriteLine("Error: Could not save data to file!");
        }
    }

    public async Task<ObservableCollection<QuestionPackViewModel>> ReadFromFileAsync()
    {
        if (!File.Exists(DataPath))
        {
            ObservableCollection<QuestionPackViewModel> newPackCollection = [];
            var newPack = new QuestionPackViewModel(new QuestionPack());
            newPack.Questions.Add(new Question("Why is the sky so blue?", "Dont worry about it!",
                "Blue is not a color!", "What about the colorblind?", "Something with light."));
            newPackCollection.Add(newPack);

            await WriteToFileAsync(newPackCollection);
        }

        try
        {
            var json = File.ReadAllTextAsync(DataPath);
            var packs =
                JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json.GetAwaiter().GetResult());
            return packs;
        }
        catch
        {
            Console.WriteLine("Error: Could not read data from file!");
        }

        return null;
    }
}