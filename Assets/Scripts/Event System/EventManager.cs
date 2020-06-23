using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnTokenHorizontalMoveEnd();
    public static event OnTokenHorizontalMoveEnd onTokenHorizontalMoveEnd;
    public static void OnTokenHorizontalMoveEndSignal()
    {
        if (onTokenHorizontalMoveEnd != null)
        {
            onTokenHorizontalMoveEnd();
        }
    }
    

    public delegate void OnTokenVerticalMoveEnd();
    public static event OnTokenVerticalMoveEnd onTokenVerticalMoveEnd;
    public static void OnTokenVerticalMoveEndSignal()
    {
        if (onTokenVerticalMoveEnd != null)
        {
            onTokenVerticalMoveEnd();
        }
    }


    public delegate void OnTokenInvalidMoveEnd();
    public static event OnTokenInvalidMoveEnd onTokenInvalidMoveEnd;
    public static void OnTokenInvalidMoveEndSignal()
    {
        if (onTokenInvalidMoveEnd != null)
        {
            onTokenInvalidMoveEnd();
        }
    }


    public delegate void OnTokenValidMoveEnd();
    public static event OnTokenValidMoveEnd onTokenValidMoveEnd;
    public static void OnTokenValidMoveEndSignal()
    {
        if (onTokenValidMoveEnd != null)
        {
            onTokenValidMoveEnd();
        }
    }


    public delegate void OnTokenDestructionEnd(List<Vector2> aListOfCells);
    public static event OnTokenDestructionEnd onTokenDestructionEnd;
    public static void OnTokenDestructionEndSignal(List<Vector2> aListOfCells)
    {
        if (onTokenDestructionEnd != null)
        {
            onTokenDestructionEnd(aListOfCells);
        }
    }


    public delegate void OnEffectDisplayEnd();
    public static OnEffectDisplayEnd onEffectDisplayEnd;
    public static void OnEffectDisplayEndSignal()
    {
        if (onEffectDisplayEnd != null)
        {
            onEffectDisplayEnd();
        }
    }


    public delegate void OnTokenGatherEnd();
    public static OnTokenGatherEnd onTokenGatherEnd;
    public static void OnTokenGatherEndSignal()
    {
        if (onTokenGatherEnd != null)
        {
            onTokenGatherEnd();
        }
    }


    public delegate void OnLaserShotHit();
    public static OnLaserShotHit onLaserShotHit;
    public static void OnLaserShotHitSignal()
    {
        if (onLaserShotHit != null)
        {
            onLaserShotHit();
        }
    }

    public delegate void OnLaserShotComplete(LaserBeams aLaserBeam);
    public static OnLaserShotComplete onLaserShotComplete;
    public static void OnLaserShotCompleteSignal(LaserBeams aLaserBeam)
    {
        if (onLaserShotComplete != null)
        {
            onLaserShotComplete(aLaserBeam);
        }
    }

    public delegate void OnUIMovesUpdate();
    public static OnUIMovesUpdate onUIMovesUpdate;
    public static void OnUIMovesUpdateSignal()
    {
        if (onUIMovesUpdate != null)
        {
            onUIMovesUpdate();
        }
    }

    public delegate void OnGameOver();
    public static OnGameOver onGameOver;
    public static void OnGameOverSignal()
    {
        if (onGameOver != null)
        {
            onGameOver();
        }
    }
}
