/*
 * File:        QuestionModel.cs
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Question data in a better format.
 */
namespace Brainmoku.Models;

public class QuestionModel
{
    public string Type { get; set; } = string.Empty;
    public string? Question { get; set; } = string.Empty;
    public string[] Answers { get; set; } = {string.Empty, string.Empty, 
        string.Empty, string.Empty};
}

// end of QuestionModel.cs

