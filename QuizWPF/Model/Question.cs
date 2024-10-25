namespace QuizWPF.Model;

public class Question (string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string correctAnswer3)
{
    public string Query { get; set; } = query;
    public string CorrectAnswer { get; set; } = correctAnswer;
    public string[] IncorrectAnswers { get; set; } = [incorrectAnswer1, incorrectAnswer2, correctAnswer3];
}