/*
 * File:        GomokuButtonGrid.js
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: Gomoku field component.
 */

import { ButtonGroup } from 'react-bootstrap';
import { GomokuButton } from './GomokuButton'; 

const GomokuButtonGrid = ({ gomokuField, 
                            selectedFieldButton, updateSelectedFieldButton, 
                            isSelected, 
                            winRow, 
                            player }) => {

    /**
     * Update the selected button to the input.
     * @param ButtonId clicked button
     */
    const onClickGomokuField = (ButtonId) => {
        if ((gomokuField[ButtonId] === 0 ||
                gomokuField[ButtonId] === 3)
            && !isSelected && winRow === null)
        {
            updateSelectedFieldButton(ButtonId);
        }
    }
    
    // avoid crashing in case the BE is not functioning
    if (gomokuField === undefined) { return(<div> loading...</div>); }
       
    // Create the grid of buttons 16x16
    const buttonGrid = [];
    let hoverClass; // hover in player colors
    if (!isSelected && winRow === null) 
    {
        if (player === 1) { hoverClass = 'currentRed'; } 
        else { hoverClass = 'currentBlue'; }
    }
    for (let j = 0; j < 16; j++) {
        const buttonRow = [];
        for (let i = 0; i < 16; i++) {
            const index = j * 16 + i;
            const isSelected = index === selectedFieldButton;
            const value = gomokuField[index];
            const isGray = winRow != null && !winRow.includes(index);
            buttonRow.push(
                <GomokuButton
                    key={index} 
                    value={value}
                    onClick={() => onClickGomokuField(index)}
                    isSelected={isSelected}
                    isGray={isGray}
                />
            );
        }
        buttonGrid.push(<ButtonGroup key={j}>{buttonRow}</ButtonGroup>);
    }

    return (
        <div className={hoverClass}>
            {buttonGrid} 
        </div>
    );
};

export default GomokuButtonGrid;
// end of GomokuButtonGrid.js