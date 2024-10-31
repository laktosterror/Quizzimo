using System.Text.Json.Serialization;

namespace QuizWPF.Model;

public class Question
{
    public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string correctAnswer3)
    {
        Query = query;
        CorrectAnswer = correctAnswer;
        IncorrectAnswers = [incorrectAnswer1, incorrectAnswer2, correctAnswer3];
    }

    [JsonConstructor]
    public Question()
    {
        Query = string.Empty;
        CorrectAnswer = string.Empty;
        IncorrectAnswers = [];
    }
    public string Query { get; set; }
    public string CorrectAnswer { get; set; }
    public string[] IncorrectAnswers { get; set; }
}