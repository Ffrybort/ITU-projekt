/*
 * File:        GomokuButton.js
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Single gomoku button component.
 */

import React from 'react';
import Button from 'react-bootstrap/Button';
import './GomokuButton.css';

// the React.memo() should avoid rendering the entire field if one button is updated 
// (but only God and React developers know what is actually happening)
export const GomokuButton = React.memo(({ value, onClick, isSelected, isGray }) => {
    let buttonClass = "btn-gomoku ";

    // Update class based on button state
    if (value === 3) { buttonClass += " btn-black "; }
    if (value === 1) { buttonClass += " btn-playerX "; }
    else if (value === 2) { buttonClass += " btn-playerO "; }
    else { buttonClass += "btn-gomoku-hover "}
    
    if (isSelected) { buttonClass += " btn-selected "; }
    if (isGray) 
    {
        if (value === 1) { buttonClass += " btn-playerX-grey"; }
        else if (value === 2) { buttonClass += " btn-playerO-grey"; }
        else if (value === 3) { buttonClass += " btn-grey"; }
    }

    return (
        <Button
            className={buttonClass}
            onClick={onClick}
        >
        </Button>
    );
});

// end of GomokuButton.js