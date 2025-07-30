/*
 * File:        App.js
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Main client app file.
 */

import React, { useEffect, useState } from 'react';
import './colors.css';
import { Container, Row, Col } from 'react-grid-system';
import { setConfiguration } from 'react-grid-system';
import { ScoreBoard } from "./components/ScoreBoard";
import { QuestionCard } from "./components/QuestionCard";
import GomokuButtonGrid from "./components/GomokuButtonGrid";

setConfiguration({ gridColumns: 16 });

function App() {
    // game state variables
    const [gomokuField, setGomokuField] = useState(Array(256).fill(0));
    const [question, setQuestion] = useState();
    const [playerTurn, setPlayerTurn] = useState();
    // button stays visibly selected until a new button is selected (so the players can see the last played button)
    const [selectedFieldButton, setSelectedFieldButton] = useState(-1);
    // isButtonSelected is true if a button was already selected (a player has to answer a question)
    // and false if it's possible to select a new one
    const [isButtonSelected, setIsButtonSelected] = useState(false);
    const [winRow, setWinRow] = useState(null);
    const [player1Score, setPlayer1Score] = useState(0);
    const [player2Score, setPlayer2Score] = useState(0);

    /**
     * Helper function to set one value in a field.
     * @param index of the button
     * @param value to be set
     */
    function setOneInGomokuField(index, value) {
        const temp = gomokuField;
        temp[index] = value;
        setGomokuField(prev => ({
            ...prev, buttons: temp
        }));
    }

    /**
     * Wrapper function to set selectedFieldButton and update isButtonSelected at the same time.
     * @param index of the newly selected button
     */
    function updateSelectedFieldButton(index)
    {
        if (index >= 0) { setIsButtonSelected(true); }
        setSelectedFieldButton(index);
    }

    /**
     * Fetch the entire game state information, synchronise FE with BE
     * @param reset 1 if the game should reset
     * @returns {Promise<void>}
     */
    async function fetchGamestate(reset)
    {
        if (reset === 1)
        {
            setIsButtonSelected(false);
            setWinRow(null);
        }
        const url = 'api/gamestate/' + reset;
        const response = await fetch(url);
        if (response.ok) {
            const data = await response.json();
            console.log(data);
            setGomokuField(data.gomokuField)
            if (data.currentQuestion !== undefined) { setQuestion(data.currentQuestion) }
            setPlayerTurn(data.playerTurn);
            updateSelectedFieldButton(data.currentButtonId);
            setPlayer1Score(data.player1Score);
            setPlayer2Score(data.player2Score);
        }
    }
    
    useEffect(() => {
        fetchGamestate(0);
    }, []);
    
    return (
        <Container>
            <Row>
                <ScoreBoard currentPlayer={playerTurn} player1Score={player1Score} player2Score={player2Score}>
                </ScoreBoard>
            </Row>
            <Row>
                <Col xs={8}>
                        <GomokuButtonGrid
                            gomokuField={gomokuField}
                            setGomokuField={setGomokuField}
                            selectedFieldButton={selectedFieldButton}
                            updateSelectedFieldButton={updateSelectedFieldButton}
                            isSelected={isButtonSelected}
                            winRow={winRow}
                            player={playerTurn}
                        />
                </Col>
                <Col xs={8}>
                    <QuestionCard
                        question={question}
                        setQuestion={setQuestion}
                        setOneInGomokuField={setOneInGomokuField}
                        selectedFieldButton={selectedFieldButton}
                        setSelectedFieldButton={setSelectedFieldButton}
                        isSelected={isButtonSelected}
                        setIsSelected={setIsButtonSelected}
                        playerTurn={playerTurn}
                        setPlayerTurn={setPlayerTurn}
                        winRow={winRow}
                        setWinRow={setWinRow}
                        player1Score={player1Score}
                        setPlayer1Score={setPlayer1Score}
                        player2Score={player2Score}
                        setPlayer2Score={setPlayer2Score}
                        fetchGamestate={fetchGamestate}
                    />
                </Col>
            </Row>
            <Row>
                
            </Row>
        </Container>
    );
}

export default App;
// end of App.js