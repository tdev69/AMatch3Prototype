using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Refill", menuName = "SO/Refill")]
public class SORefill : SOState
{
    private List<MatchTypes> matchTypesByPriority = new List<MatchTypes>(){MatchTypes.totalWipe,
                                                                            MatchTypes.tripleColumnWipe,
                                                                            MatchTypes.tripleLineWipe,
                                                                            MatchTypes.scissors,
                                                                            MatchTypes.bomb,
                                                                            MatchTypes.columnWipe,
                                                                            MatchTypes.lineWipe,
                                                                            MatchTypes.normal
                                                                            };
    
    private Dictionary<MatchTypes, List<Vector2>> detectedMatches = new Dictionary<MatchTypes, List<Vector2>>();
    private List<Vector2> matchesByPriority = new List<Vector2>();

    private void ClearAll()
    {
        detectedMatches.Clear();
        matchesByPriority.Clear();
    }

    public override void StateAction()
    {
        EventManager.onTokenVerticalMoveEnd += CheckNewGridForMatches;
        SetNewTokensToCells();
    }

    private void CheckNewGridForMatches()
    {
        CreateMatchesDictionary();
        CheckForNewMatches(this.gridManager.GetNumberOfDisplayedRows(), aMinimumY: 1);
        ResolveMatchesByPriority();
        CheckForNewMatches(aMaximumY: 1, aMinimumY: 0); //ensures that bottom line matches are at the end of the list, thus treated after grid matches.
        ResolveMatchesByPriority();

        if (this.matchesByPriority.Count > 0)
        {
            NextStateTransition(StateNames.matching, this.matchesByPriority[0]);
        }

        else
        {
            UpdateHiddenTokens(); //Makes sure lateral tokens on bottomline are changed properly.
            NextStateTransition(StateNames.wait);
        }
    }

    private void SetNewTokensToCells()
    {
        this.gridManager.RefillGridWithTokens();
        this.tokenMovement.MoveAllTokensToPosition();
    }

    private void CreateMatchesDictionary()
    {
        this.detectedMatches = new Dictionary<MatchTypes, List<Vector2>>();

        foreach(MatchTypes type in matchTypesByPriority)
        {
            detectedMatches.Add(type, new List<Vector2>());
        }
    }

    ///minimum y is inclusive while maximum y is exclusive
    ///Used for coordinates in grid
    private void CheckForNewMatches(int aMaximumY, int aMinimumY)
    {
        for (int y = aMinimumY; y < aMaximumY; y++)
        {
            for (int x = 1; x <= this.gridManager.GetNumberOfDisplayedColumns(); x++)
            {
                MatchTypes match = this.matchManager.GetCompleteMatchInfo(new Vector2(x, y)).Item1;
                
                if (matchTypesByPriority.Contains(match) == true)
                {
                    detectedMatches[match].Add(new Vector2(x, y));
                }
            }
        }
    }


    private void ResolveMatchesByPriority()
    {
        foreach(MatchTypes type in this.matchTypesByPriority)
        {
            if (this.detectedMatches[type].Count > 0)
            {
                this.matchesByPriority.AddRange(detectedMatches[type]);
            }
        }
    }

    private void UpdateHiddenTokens()
    {
        TokenTypes hiddenTokenLeft = this.gridManager.GetTokenTypeOfCell(0, 0);
        TokenTypes hiddenTokenRight = this.gridManager.GetTokenTypeOfCell(this.gridManager.GetGridNumberOfColumns() - 1, 0);
        TokenTypes tokenLeft = this.gridManager.GetTokenTypeOfCell(1, 0);
        TokenTypes tokenRight = this.gridManager.GetTokenTypeOfCell(this.gridManager.GetNumberOfDisplayedColumns(), 0);

        if (hiddenTokenLeft != tokenRight)
        {
            SetHiddenToken(0, 0, tokenRight);
        }

        if (hiddenTokenRight != tokenLeft)
        {
            SetHiddenToken(this.gridManager.GetGridNumberOfColumns() - 1, 0, tokenLeft);
        }
    }

    private void SetHiddenToken(int gridXPos, int gridYPos, TokenTypes aTokenType)
    {
        this.gridManager.SetCellToEmpty(gridXPos, gridYPos, returnTokenToPool: true);
        this.gridManager.SetTokenTypeOfCell(gridXPos, gridYPos, aTokenType);
        this.gridManager.SetTokenDisplay(gridXPos, gridYPos);
    }

    private void NextStateTransition(StateNames aStateName)
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(aStateName));
        StopListening();
        ClearAll();
        nextState.StateAction();
    }

    private void NextStateTransition(StateNames aStateName, Vector2 aCellCoordinate)
    {
        SOState nextState = SetNextState(this.statesDictionary.GetState(aStateName));
        StopListening();
        ClearAll();
        nextState.StateAction(aCellCoordinate, false);
    }

    private void StopListening()
    {
        EventManager.onTokenVerticalMoveEnd -= CheckNewGridForMatches;
    }

    private void OnDisable()
    {
        StopListening();
    }

    private void OnDestroy()
    {
        StopListening();
    }
}

