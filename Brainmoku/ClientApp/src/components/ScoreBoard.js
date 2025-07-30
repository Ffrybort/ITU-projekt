/*
 * File:        ScoreBoard.js
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Component to display the score and to highlight the current player.
 */

import React, { Component } from 'react';
import playerX from '../assets/x-lg.svg';
import playerXGrey from '../assets/x-lg-grey.svg';
import playerO from'../assets/circle.svg';
import playerOGrey from'../assets/circle-gray.svg';
import "./ScoreBoard.css";

export class ScoreBoard extends Component {
  static displayName = ScoreBoard.name;

  constructor(props) {
    super(props);
    this.state = { currentPlayer: props.currentPlayer,
                   player1Score: props.player1Score,
                   player2Score: props.player2Score,
                    };
  }

  render() {
    return (
        <div className='center'>
          {/* player 1 */}
          {this.props.currentPlayer === 1 ? 
              <div className="scoreboard-text player player-x">Player 1 
                <img className="player-icon " src={playerX} alt="X"/> </div> :
              <div className="scoreboard-text player">Player 1
                <img className="player-icon " src={playerXGrey} alt="X"/> </div>
          }
          {/* score */}
          <div className="scoreboard-text">{this.props.player1Score + " : " + this.props.player2Score}</div>
          
          {/* player 2 */}
          {this.props.currentPlayer === 2 ? 
              <div className="scoreboard-text player player-o">
                <img className="player-icon player-icon-o" src={playerO} alt="O"/> Player 2 </div> :
              <div className="scoreboard-text player">
                <img className="player-icon player-icon-o" src={playerOGrey} alt="O"/> Player 2
              </div>
          }
        </div>
    );
  }
}

// end of ScoreBoard.js