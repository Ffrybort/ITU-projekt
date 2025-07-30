/*
 * File:        AnswerController.cs
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Responds when an answer is selected.
 */

using Microsoft.AspNetCore.Mvc;
using Brainmoku.Models;
using static Brainmoku.Models.GomokuField.Value;

namespace Brainmoku.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AnswerController : ControllerBase

{
    public struct AnswerResponse
    {
        public int CorrectAnswerPosition { get; set; }
        public int NextPlayer { get; set; }
        public int Value { get; set; }
        public List<int> WinRow { get; set; }

        public AnswerResponse(int correctAnswerPosition, int nextPlayer, int value, List<int> winRow)
        {
            CorrectAnswerPosition = correctAnswerPosition;
            NextPlayer = nextPlayer;
            Value = value;
            WinRow = winRow ?? new List<int>();
        }
    }

    private void SwitchPlayer()
    {
        GameState.PlayerTurn = GameState.PlayerTurn == PlayerX ? PlayerO : PlayerX;
    }
    
    /**
     * Send information about answer correctness, next player (-1 the game ended),
     * value to be written in the selected field, win row if it occured
     */
    [HttpGet("{answernum:int}")]
    public AnswerResponse Get(int answerNum)
    {
        if (GameState.CurrentQuestion == null) // should not happen
        {
            Console.WriteLine("Current question is null");
            return new AnswerResponse( -1, 0, 0, new List<int>());
        }
        
        List<int>? winRow = null;

        // answer is correct
        if (GameState.CorrectAnswerPosition == answerNum)
        {
            winRow = GameState.GomokuField.SetAndCheck(
                GameState.CurrentButtonId, GameState.PlayerTurn);
        }
        else // answer is incorrect
        {
            if (!GameState.GomokuField.IsBlack(GameState.CurrentButtonId))
            {
                GameState.GomokuField.SetBlack(GameState.CurrentButtonId);
            }
            else
            {
                SwitchPlayer();
                winRow = GameState.GomokuField.SetAndCheck(
                    GameState.CurrentButtonId, GameState.PlayerTurn);
            }
        }
        if (winRow != null) // win
        {
            // score update
            if (GameState.PlayerTurn == PlayerX) { GameState.PlayerXScore++; }
            else { GameState.PlayerOScore++; }
            
            int tmpValue = GameState.GomokuField.GetValue(GameState.CurrentButtonId);
            GameState.ResetGame();
            
            return new AnswerResponse
            {
                CorrectAnswerPosition = GameState.CorrectAnswerPosition,
                NextPlayer = -1,
                Value = tmpValue,
                WinRow = winRow
            };
        }
        
        GameState.CurrentQuestion = null;
        int tmpButtonId = GameState.CurrentButtonId;
        GameState.CurrentButtonId = -1;
        SwitchPlayer();
        
        return new AnswerResponse
        {
            CorrectAnswerPosition = GameState.CorrectAnswerPosition,
            NextPlayer = (int) GameState.PlayerTurn,
            Value = GameState.GomokuField.GetValue(tmpButtonId)
        };
    }
}

// end of AnswerController.cs