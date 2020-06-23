using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOState : ScriptableObject
{
    [SerializeField] protected SOStatesDictionary statesDictionary = null;
    protected GridManager gridManager = null;
    protected MatchManager matchManager = null;
    protected TokenMovementManager tokenMovement = null;
    protected Highlighter highlighter = null;
    protected GameMaster theGameMaster = null;
    protected EffectsPool effectsPool = null;


    public virtual void CellClicked(Vector2 cellCoordinates)
    {return;}

    public virtual void ArrowClicked(ArrowDirection aDirection, int aLineNumber)
    {return;}

    public virtual void ReturnToIdle()
    {return;}

    public virtual void MoveTokens()
    {return;}

    public virtual SOState SetNextState(SOState aState)
    {
        return this.theGameMaster.SetCurrentState(aState);
    }

    public virtual void StateAction()
    {return;}

    public virtual void StateAction(Vector2 cellCoordinates, bool fromRefill = false)
    {return;}

    public virtual void StateAction(ArrowDirection aDirection, int aLineNumber)
    {return;}

    public void SetMainComponents(GameMaster aGameMaster,
                                    GridManager aGridManager = null, 
                                    MatchManager aMatchManager = null,
                                    TokenMovementManager aTokenMovementManager = null,
                                    Highlighter aHighlighter = null,
                                    EffectsPool anEffectsPool = null
                                    )
    {   
        this.theGameMaster = aGameMaster;
        this.gridManager = aGridManager;
        this.matchManager = aMatchManager;
        this.tokenMovement = aTokenMovementManager;
        this.highlighter = aHighlighter;
        this.effectsPool = anEffectsPool;
    }

    public void SetMainComponentsInAllStates(GameMaster aGameMaster,
                                                GridManager aGridManager = null, 
                                                MatchManager aMatchManager = null,
                                                TokenMovementManager aTokenMovementManager = null,
                                                Highlighter aHighlighter = null,
                                                EffectsPool anEffectsPool = null 
                                                )
    {
        foreach(StateNames state in Enum.GetValues(typeof(StateNames)))
        {
            if (this.statesDictionary.GetState(state) != null)
            {
                SOState aStateInDictionary = this.statesDictionary.GetState(state);
                aStateInDictionary.SetMainComponents(aGameMaster, aGridManager, aMatchManager, aTokenMovementManager, aHighlighter, anEffectsPool);
            }
            
        }
    }


    public SOState GetStateFromDictionary(StateNames aState)
    {
        return this.statesDictionary.GetState(aState);
    }
}
