/*
 * File:        GameState.cs
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: This static class holds data about the current game.
 */

namespace Brainmoku.Models;
public static class GameState
{
    //public static int[] Buttons { get; set; } = new int[256];
    public static GomokuField GomokuField = new GomokuField();
    public static GomokuField.Value PlayerTurn { get; set; }
    public static QuestionModel? CurrentQuestion { get; set; }
    public static int CurrentButtonId { get; set; } // -1 = none selected
    public static int CorrectAnswerPosition { get; set; } // 0...3
    public static int PlayerXScore { get; set; }
    public static int PlayerOScore { get; set; }

    static GameState()
    {
        ResetGame();
        PlayerXScore = 0;
        PlayerOScore = 0;
    }

    /**
     * Reset game - clear the field, randomly choose the starting player.
     */
    public static void ResetGame()
    {
        PlayerTurn = (GomokuField.Value)new Random().Next(1, 3);
        
        GomokuField.Clear();

        CurrentQuestion = null;
        CurrentButtonId = -1;
    }
    
    /**
     * Return game state.
     */
    public static GameStateDTO GetGameState()
    {
        GameStateDTO result = new GameStateDTO
        {
            GomokuField = GomokuField.GetField(),
            PlayerTurn = (int) PlayerTurn,
            CurrentQuestion = CurrentQuestion,
            CurrentButtonId = CurrentButtonId,
            Player1Score = PlayerXScore,
            Player2Score = PlayerOScore
        };
        return result;
    }
}

/**
 * This is sent to client when a reset or reload occurs.
 * (this probably could have been the same class)
 */
public class GameStateDTO
{
    public int[] GomokuField { get; set; } = new int[0];
    // 0... available normal question (empty)
    // 1... player1 owns (x)
    // 2... player2 owns (o)
    // 3... available yes/no question (black)
    public int PlayerTurn { get; set; } // 1.. 2..
    public QuestionModel? CurrentQuestion { get; set; }
    public int CurrentButtonId {  get; set; }
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
}

// end of GameState.cs