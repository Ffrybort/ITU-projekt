/*
 * File:        QuestionTable.cs
 * Author:      Ondrej Horak, Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: This class loads and parses data (questions).
 */

using System.Text.Json;

namespace Brainmoku.Models.MockDB;

/**
 * 
 */
public class QuestionData
{
    public string? Type { get; set; }
    public string? Difficulty { get; set; }
    public string? Category { get; set; }
    public string? Question { get; set; }
    public string Correct_answer { get; set; } = "";
    public string[] Incorrect_answers { get; set; } = {"","",""};

}
public static class QuestionsTable
{
    //list of all multiple-choice questions
    private static List<QuestionData> mcQuestions = new List<QuestionData>();

    //list of all true/false questions
    private static List<QuestionData> tfQuestions = new List<QuestionData>();
    static QuestionsTable()
    {
        LoadAllQuestions();
    }

    /**
     * Load questions if there are none.
     * Return a random multi-choice question and delete it.
     */
    public static QuestionModel GetRandomMCQuestion() 
    {
        // loading all questions again if there are none
        // not the best solution, but works
        if (tfQuestions.Count == 0) { LoadAllQuestions(); } 
        Random random = new Random();
        int rndInt = random.Next(0, mcQuestions.Count - 1);
        int[] randomizedArray = { 0, 1, 2, 3};
        for (int i = 0; i < 4; i++)
        {
            int r = random.Next(i, randomizedArray.Length);
            (randomizedArray[r], randomizedArray[i]) = (randomizedArray[i], randomizedArray[r]);
        }

        QuestionModel question = new QuestionModel
        {
            Type = mcQuestions[rndInt].Type,
            Question = mcQuestions[rndInt].Question
        };
        question.Answers[randomizedArray[0]] = mcQuestions[rndInt].Incorrect_answers[0];
        question.Answers[randomizedArray[1]] = mcQuestions[rndInt].Incorrect_answers[1];
        question.Answers[randomizedArray[2]] = mcQuestions[rndInt].Incorrect_answers[2];
        question.Answers[randomizedArray[3]] = mcQuestions[rndInt].Correct_answer;

        GameState.CorrectAnswerPosition = randomizedArray[3];
        
        GameState.CurrentQuestion = question;
        mcQuestions.RemoveAt(rndInt);
        return question;
    }
    
    /**
     * Load questions if there are none.
     * Return a random true-false question and delete it.
     */
    public static QuestionModel GetRandomTFQuestion() 
    {
        // loading all questions again if there are none
        // not the best solution, but works
        if (tfQuestions.Count == 0) { LoadAllQuestions(); } 
        Random rnd = new Random(); 
        int rndInt = rnd.Next(0, tfQuestions.Count - 1);
        QuestionModel question = new QuestionModel
        {
            Question = tfQuestions[rndInt].Question
        };
        question.Answers[0] = "True";
        question.Answers[1] = "False";
        question.Answers[2] = "";
        question.Answers[3] = "";
        
        if (tfQuestions[rndInt].Correct_answer == "True")
        { GameState.CorrectAnswerPosition = 0; }
        else { GameState.CorrectAnswerPosition = 1; }
        
        GameState.CurrentQuestion = question;
          
        tfQuestions.RemoveAt(rndInt);
        return question;
    }
    
    /**
    * Load all questions into mcQuestions and tfQuestions.
    */
    public static void LoadAllQuestions()
    {
        string projectRoot = Directory.GetCurrentDirectory();
        
        //path to the questions folder
        string basePath = Path.Combine(projectRoot, "Models", "MockDB", "Data", "questions");

        string mcPath = Path.Combine(basePath, "multiple");
        string tfPath = Path.Combine(basePath, "boolean");

        LoadQuestionsType(mcPath, mcQuestions);
        LoadQuestionsType(tfPath, tfQuestions);
    }

    /**
     * Helper function to load questions.
     */
    private static void LoadQuestionsType(string dirPath, List<QuestionData> questionList)
    { 
        string[] directories = Directory.GetDirectories(dirPath);

        foreach (string directory in directories)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                string json = File.ReadAllText(file);

                JsonSerializerOptions opts = new JsonSerializerOptions();
                opts.PropertyNameCaseInsensitive = true;

                List<QuestionData>? questions = new List<QuestionData>();
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    JsonElement jsonElement = document.RootElement.GetProperty("results");

                    string results = jsonElement.ToString();
                    questions = JsonSerializer.Deserialize<List<QuestionData>>(results, opts);
                }

                if (questions != null) { questionList.AddRange(questions); }
            }
        }
    }
}

// end of QuestionTable.cs