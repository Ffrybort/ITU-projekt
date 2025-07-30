/*
 * File:        QuestionCard.js
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Question and answers component.
 */

import Card from "react-bootstrap/Card";
import Button from "react-bootstrap/Button";
import React, { useEffect, useState } from 'react';
import "./QuestionCard.css";

// Function to decode text (works on JSON)
function htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
}

export const QuestionCard = ({
                                 question, setQuestion,
                                 setOneInGomokuField,
                                 selectedFieldButton,
                                 isSelected,
                                 setIsSelected,
                                 playerTurn,
                                 setPlayerTurn,
                                 setWinRow,
                                 player1Score,
                                 setPlayer1Score,
                                 player2Score,
                                 setPlayer2Score,
                                 fetchGamestate
                             }) => {
    
    // to display the correct and incorrect answer after the question is answered
    const [correctAnswer, setCorrectAnswer] = useState(-1);
    const [incorrectAnswer, setIncorrectAnswer] = useState(-1);
    
    // to display an end of game message if a player won
    const [won, setWon] = useState(false);

    /**
     * Fetch and update the current question
     * @returns {Promise<void>}
     */
    async function fetchQuestion() {
        if (!isSelected) { return; } // should not happen
        setCorrectAnswer(-1); // unset
        setIncorrectAnswer(-1);
        const url = `api/question/${selectedFieldButton}`;
        const response = await fetch(url);
        if (response.ok) {
            const data = await response.json();
            setQuestion(data);  // set the question in App.js
        }
    }

    useEffect(() => {
        fetchQuestion();
    }, [isSelected]);

    /**
     * Wrapper function to set the setWon and call fetchGamestate().
     */
    function newGame()
    {
        setWon(false);
        fetchGamestate(1);
    }

    /**
     * Helper function for a short delay.
     * @param delay how long to sleep in ms.
     * @returns {Promise<unknown>}
     */
    const sleep = (delay) => new Promise((resolve) => setTimeout(resolve, delay))

    /**
     * On click function to determine if the answer was correct, 
     * to update the selected button value and to end the game if necessary
     * @param selectedAnswerPosition
     * @returns {Promise<void>}
     */
    const onClickAnswer = async (selectedAnswerPosition) => {
        if (correctAnswer >= 0) { // Already answered
            return;
        }

        const url = 'api/answer/' + selectedAnswerPosition;
        const response = await fetch(url);
        if (!response.ok) {
            console.log("fetch failed");
            return;
        }

        const data = await response.json();
        console.log(data);

        // Show correct answer
        setCorrectAnswer(data.correctAnswerPosition);

        // Show incorrect answer if selected
        if (selectedAnswerPosition !== data.correctAnswerPosition && incorrectAnswer < 0) {
            setIncorrectAnswer(selectedAnswerPosition);
        }

        // Update the gomoku field with the selected answer
        setOneInGomokuField(selectedFieldButton, data.value);
        setIsSelected(false); // unselect
        if (data.nextPlayer < 0) // win
        {
            // update the score based of the last played button
            // not entirely correct, the FE should ask BE for the updated score
            if (data.value === 1) { setPlayer1Score(player1Score + 1); }
            else { setPlayer2Score(player2Score + 1); }
            setWinRow(data.winRow);
            // wait to let the players reed the answers
            await sleep(3000);
            // this will offer a new game button
            setWon(true);
        }
        else // update the next player
        {
            setPlayerTurn(data.nextPlayer);
        }
    };

    const answerButtons = [];
    let questionText = "";
    let cardClass = "";
    if (!question) {
        questionText = "Please select a field.";
    }
    else if (won)
    {   // game has ended, it's possible to start a new game
        // the question card is used instead of a new window/new page, so the field stays visible
        questionText = "Congratulations, player " + playerTurn + " !";
        answerButtons.push(<Button className="btn-new-game" onClick={() => newGame()}>New game</Button>)
    }
    else { // display the question
        questionText = question.question;
        const numOfButtons = question.type === "multiple" ? 4 : 2;
        if (correctAnswer < 0) { // hover if the question was not answered yet
            cardClass = "btn-answer-hover"
        }

        for (let i = 0; i < numOfButtons; i++) {
            let answerClass = "btn-answer ";
            if (i === correctAnswer) {
                answerClass += " btn-answer-correct ";
            } else if (i === incorrectAnswer) {
                answerClass += " btn-answer-incorrect ";
            }
            answerButtons.push(
                <Button
                    key={i}
                    className={answerClass}
                    onClick={() => onClickAnswer(i)}
                >
                    {htmlDecode(question.answers[i])}
                </Button>
            );
        }
    }

    return (
        <Card className="question-card border">
            <Card.Body>
                <Card.Title className="question-text">
                    {htmlDecode(questionText)}
                </Card.Title>
                <Card.Text className={cardClass}>
                    {answerButtons}
                </Card.Text>
            </Card.Body>
        </Card>
    );
};

// end of QuestionCard.js