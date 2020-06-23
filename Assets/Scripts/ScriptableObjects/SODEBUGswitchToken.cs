using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DEBUG SWITCH TOKEN", menuName = "SO/DEBUG SWITCH TOKEN")]
public class SODEBUGswitchToken : SOState
{
    [SerializeField] TokenTypes wantedToken = TokenTypes.empty;
    [SerializeField] bool finished = false;

    public override void CellClicked(Vector2 cellCoordinates)
    {
        TokenPool tokenPool = this.theGameMaster.GetComponent<TokenPool>();
        GameObject newToken = tokenPool.GetTokenFromPool(wantedToken);
        this.gridManager.SetCellToEmpty((int)cellCoordinates.x, (int)cellCoordinates.y);
        this.gridManager.SetTokenOfCell((int)cellCoordinates.x, (int)cellCoordinates.y, aTokenType: this.wantedToken, newToken);
        
        if (finished == true)
        {
            NextStateTransition(StateNames.wait);
        }
    }

    private void NextStateTransition(StateNames aStateName)
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(aStateName));
        nextState.StateAction(); 
    }
}
