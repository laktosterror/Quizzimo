﻿namespace QuizWPF.Model;

public class OpenTriviaQuestions
{
    public int response_code { get; set; }
    public Result[] results { get; set; }
}

public class Result
{
    public string type { get; set; }
    public string difficulty { get; set; }
    public string category { get; set; }
    public string question { get; set; }
    public string correct_answer { get; set; }
    public string[] incorrect_answers { get; set; }
}