/*
 * File:        QuestionController.cs
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Responds when a field is selected (a question needs to be displayed).
 */

using Brainmoku.Models.MockDB;
using Brainmoku.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brainmoku.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class QuestionController : ControllerBase
{
    [HttpGet("{buttonid:int}")]
    public QuestionModel? Get(int buttonId) // Id button - coordinates
    {
        if (GameState.CurrentQuestion != null)
        { 
            return GameState.CurrentQuestion;
        }

        if (buttonId == -1) { return GameState.CurrentQuestion; }

        GameState.CurrentButtonId = buttonId;
        
        if (GameState.GomokuField.IsEmpty(GameState.CurrentButtonId))
        {
            return QuestionsTable.GetRandomMCQuestion();
        }

        if (GameState.GomokuField.IsBlack(GameState.CurrentButtonId))
        {
            return QuestionsTable.GetRandomTFQuestion();
        }
        return GameState.CurrentQuestion;
    }
}

// end of QuestionController.cs