using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait", menuName = "SO/Wait")]
public class SOWait : SOState
{
    public override void CellClicked(Vector2 coordinates)
    {
        NextStateTransition(coordinates);
    }

    public override void ArrowClicked(ArrowDirection aDirection, int aLineNumber)
    {
        NextStateTransition(aDirection, aLineNumber);
    }

    private void NextStateTransition(Vector2 coordinates)
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(StateNames.clickCell));
        nextState.StateAction(coordinates);
    }

    private void NextStateTransition(ArrowDirection aDirection, int aLineNumber)
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(StateNames.lateralMove));
        nextState.StateAction(aDirection, aLineNumber);
    }
}
