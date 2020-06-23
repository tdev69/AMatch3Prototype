using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lateral Moves", menuName = "SO/Lateral Moves")]
public class SOLateralMove : SOState
{
    private void MoveLine(ArrowDirection aDirection, int aLineNumber)
    {
        this.tokenMovement.MoveLineHorizontal(aDirection, aLineNumber);
    }

    public override void StateAction(ArrowDirection aDirection, int aLineNumber)
    {
        EventManager.onTokenHorizontalMoveEnd += NextStateTransition;
        MoveLine(aDirection, aLineNumber);
    }

    private void NextStateTransition()
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(StateNames.refill));
        nextState.StateAction();
        StopListeningToEvents();
    }

    private void StopListeningToEvents()
    {
        EventManager.onTokenHorizontalMoveEnd -= NextStateTransition;
    }

    private void OnDisable()
    {
        StopListeningToEvents();
    }
}
