using System;
using System.Text.Json;
using System.IO;
using System.Collections.ObjectModel;
using QuizWPF.ViewModel;

namespace QuizWPF;

public class FileReader(string dataPath)
{
    readonly string DataPath = dataPath;

    public void WriteToFile(ObservableCollection<QuestionPackViewModel> packs)
    {
        try
        {
            var json = JsonSerializer.Serialize(packs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DataPath, json);
        }
        catch
        {
            Console.WriteLine("Error: Could not save data to file!");
        }
    }

    public ObservableCollection<QuestionPackViewModel> ReadFromFile()
    {
        if (!File.Exists(DataPath))
        {

        }
        try
        {
            var json = File.ReadAllText(DataPath);
            var packs = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json);
            return packs;
        }
        catch
        {
            Console.WriteLine("Error: Could not read data from file!");
        }
        return null;
    }
}
