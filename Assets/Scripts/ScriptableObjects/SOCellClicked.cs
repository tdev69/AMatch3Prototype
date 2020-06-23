using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell Clicked State", menuName = "SO/Cell Clicked State")]
public class SOCellClicked : SOState
{    
    private void DisplayHighlight(Vector2 cellCoordinates)
    {
        Vector3 coordinates = this.gridManager.GetCellWorldCoordinates((int)cellCoordinates.x, (int)cellCoordinates.y);
        this.highlighter.GrowHighlights(coordinates);
        NextStateTransition(StateNames.wait, cellCoordinates);
    }

    public override void StateAction(Vector2 cellCoordinates, bool fromRefill = false)
    {
        this.highlighter.ResetHighlight();

        if (cellCoordinates.y == 0)
        {
            DisplayHighlight(cellCoordinates);
        }

        else
        {
            NextStateTransition(StateNames.verticalMove, cellCoordinates);
        }
    }

    private void NextStateTransition(StateNames aStateName, Vector2 cellCoordinates)
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(aStateName));
        nextState.StateAction(cellCoordinates);
    }
}
