/*
 * File:        GameStateController.cs
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Responds when the page is reloaded or game is to reset.
 *              Resets the game if the input value is 1.
 */

using Brainmoku.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brainmoku.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class GameStateController : ControllerBase
{
    [HttpGet("{reset:int}")] 
    public GameStateDTO Get(int reset)
    {
        if (reset == 1) { GameState.ResetGame(); }
        return GameState.GetGameState();
    }
}

// end of GameStateController.cs