using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vertical Move", menuName = "SO/Vertical Move")]
public class SOVerticalMove : SOState
{
    private Vector2 cellCoordinates = Vector2.zero;

    public override void StateAction(Vector2 cellCoordinates, bool fromRefill = false)
    {
        EventManager.onTokenInvalidMoveEnd += InvalidMoveEventListener;
        EventManager.onTokenValidMoveEnd += ValidMoveEndListener;
        this.cellCoordinates = cellCoordinates;
        CheckForMatch();
    }

    private void CheckForMatch()
    {
        TokenTypes lineToken = this.gridManager.GetTokenTypeOfCell((int)this.cellCoordinates.x, 0);
        
        if (this.matchManager.MatchDetector((int)this.cellCoordinates.x, (int)this.cellCoordinates.y, typeToCheck: lineToken) == true)
        {
            GridUpdate();
        }

        else
        {
            this.tokenMovement.TokenInvalidVerticalMove(this.cellCoordinates);
        }
    }

    public void GridUpdate()
    {
        this.gridManager.SwapCellsInfoGridLine(this.cellCoordinates);
        this.tokenMovement.TokenValidMove(this.cellCoordinates);
    }

    private void InvalidMoveEventListener()
    {
        NextStateTransition(StateNames.wait, this.cellCoordinates);
    }

    private void ValidMoveEndListener()
    {
        NextStateTransition(StateNames.matching, this.cellCoordinates);
    }

    private void NextStateTransition(StateNames aStateName, Vector2 cellCoordinates)
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(aStateName));
        StopListeningToEvents();
        nextState.StateAction(cellCoordinates); 
    }

    private void OnDisable()
    {
        StopListeningToEvents();
    }

    private void StopListeningToEvents()
    {
        EventManager.onTokenInvalidMoveEnd -= InvalidMoveEventListener;
        EventManager.onTokenValidMoveEnd -= ValidMoveEndListener;
    }


}
