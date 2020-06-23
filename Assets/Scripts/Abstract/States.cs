using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When making changes to the enum, make sure to go back in the editor and make sure all states are properly set on State Dictionary SO.
///Use empty when in need of a no-state or null
public enum StateNames {clickCell, empty, lateralMove, matching, refill, switchToken, verticalMove, wait}; //MatchMove removed as I don't find what it stands for.

[System.Serializable]
public class States
{
    public StateNames nameOfState;
    public SOState aState;

    public StateNames GetStateNames()
    {
        return this.nameOfState;
    }

    public SOState GetState()
    {
        return this.aState;
    }
}
