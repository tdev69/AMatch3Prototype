using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenMovementManager : MonoBehaviour
{
    [SerializeField] private float invalidMovePauseTime = 0.15f;
    [SerializeField] private float validMovePauseTime = 0.15f;
    [SerializeField] private float delayMoveTokensToPos = 0.1f;
    
    private GridManager gridManager = null;

    private void Awake()
    {
        DOTween.Init();
        this.gridManager = GetComponent<GridManager>();
    }

    public void MoveAllTokensToPosition()
    {
        Sequence movingTokens = DOTween.Sequence().OnComplete(EventManager.OnTokenVerticalMoveEndSignal);

        for (int x = 1; x <= this.gridManager.GetNumberOfDisplayedColumns(); x++)
        {

            for (int y = 0; y < this.gridManager.GetNumberOfDisplayedRows(); y++)
            {
                movingTokens.Join(this.gridManager.MoveTokenDisplayedToCell(x, y));
            }
        }

        movingTokens.PrependInterval(this.delayMoveTokensToPos);
    }

    ///Moves the line given in arg (starts at 0) in the provided direction
    public void MoveLineHorizontal(ArrowDirection aDirection, int aLineNumber)
    {
        if (aDirection == ArrowDirection.left)
        {
            MoveLineLeft(aLineNumber);
        }

        else 
        {
            MoveLineRight(aLineNumber);
        }
    }

    private void MoveLineLeft(int aLineNumber)
    {
        Sequence movingTokens = GetSequenceHorizontalMove();
        this.gridManager.SetCellToEmpty(0, aLineNumber);

        for(int x = 0; x < this.gridManager.GetGridNumberOfColumns() - 1; x++)
        {
            Vector2 thisCellCoords = new Vector2(x, aLineNumber);
            Vector2 cellOnRightCoords = new Vector2(x + 1, aLineNumber); 
            this.gridManager.TransferCellInfo(startCellGridCoords: cellOnRightCoords, endCellGridCoords: thisCellCoords);
            movingTokens.Join(this.gridManager.MoveTokenDisplayedToCell(x, aLineNumber));
        }
    }

    private void MoveLineRight(int aLineNumber)
    {
        Sequence movingTokens = GetSequenceHorizontalMove();
        this.gridManager.SetCellToEmpty(this.gridManager.GetGridNumberOfColumns() - 1, aLineNumber);

        for(int x = this.gridManager.GetGridNumberOfColumns() - 1; x > 0; x--)
        {
            Vector2 thisCellCoords = new Vector2(x, aLineNumber);
            Vector2 cellOnLeftCoords = new Vector2(x - 1, aLineNumber); 
            this.gridManager.TransferCellInfo(startCellGridCoords: cellOnLeftCoords, endCellGridCoords: thisCellCoords);
            movingTokens.Join(this.gridManager.MoveTokenDisplayedToCell(x, aLineNumber));
        }
    }

    private Sequence GetSequenceHorizontalMove()
    {
        return DOTween.Sequence().OnComplete(EventManager.OnTokenHorizontalMoveEndSignal);
    }

    //********************User Input Moves*******************************************
    public void TokenInvalidVerticalMove(Vector2 cellCoordinates)
    {
        Vector3 clickedCellWorldCoord = this.gridManager.GetCellWorldCoordinates((int)cellCoordinates.x, (int)cellCoordinates.y);
        Vector3 bottomLineCellWorldCoord = this.gridManager.GetCellWorldCoordinates((int)cellCoordinates.x, 0);
        Vector2 bottomLineCellGridCoord = new Vector2 (cellCoordinates.x, 0);
        Sequence tokenMove = DOTween.Sequence().OnComplete(EventManager.OnTokenInvalidMoveEndSignal).SetLoops(2, LoopType.Yoyo);        
        tokenMove.Join(this.gridManager.MoveTokenToPosition(cellCoordinates, bottomLineCellWorldCoord));
        tokenMove.Join(this.gridManager.MoveTokenToPosition(bottomLineCellGridCoord, clickedCellWorldCoord));
        tokenMove.AppendInterval(invalidMovePauseTime);
    }

    public void TokenValidMove(Vector2 cellCoordinates)
    {
        Sequence tokenMove = DOTween.Sequence().OnComplete(EventManager.OnTokenValidMoveEndSignal);
        tokenMove.Join(this.gridManager.MoveTokenDisplayedToCell((int)cellCoordinates.x, (int)cellCoordinates.y));
        tokenMove.Join(this.gridManager.MoveTokenDisplayedToCell((int)cellCoordinates.x, 0));
        tokenMove.AppendInterval(validMovePauseTime);
    }


    //*****************Effects animation Moves*******************************************
    public void TokenNormalMatch(List<Vector2> aListOfCellls)
    {
        Sequence destructionSequence = DOTween.Sequence().OnComplete(EventManager.OnEffectDisplayEndSignal);
        
        foreach(Vector2 cell in aListOfCellls)
        {
            destructionSequence.Join(this.gridManager.DestroyToken(cell));
        }
        
        destructionSequence.AppendCallback(() => EventManager.OnTokenDestructionEndSignal(aListOfCellls));
    }

    public void TokenGatherSpecialMatch(List<Vector2> aListOfCells, Vector2 aCellCoordinates)
    {
        Sequence gatherTokens = DOTween.Sequence().OnComplete(EventManager.OnTokenGatherEndSignal);
        Vector3 targetPosition = this.gridManager.GetCellWorldCoordinates((int)aCellCoordinates.x, (int)aCellCoordinates.y);

        foreach(Vector2 cell in aListOfCells)
        {
            gatherTokens.Join(this.gridManager.MoveTokenToPosition(cell, targetPosition));
        }
    }

    public void TokenShrinkSpecial(List<Vector2> aListOfCells)
    {
        Sequence shrinkSequence = DOTween.Sequence().OnComplete(() => EventManager.OnTokenDestructionEndSignal(aListOfCells));

        foreach(Vector2 cell in aListOfCells)
        {
            shrinkSequence.Join(this.gridManager.DestroyToken(cell));
        }
    }

    public void TokenShrinkSpecial(Vector2 aCellCoordinates, bool sendEndSignal)
    {
        if (sendEndSignal == false)
        {
            this.gridManager.DestroyToken(aCellCoordinates);
        }

        else
        {
            this.gridManager.DestroyToken(aCellCoordinates).OnComplete(() => EventManager.OnTokenDestructionEndSignal(new List<Vector2>(){aCellCoordinates}));
        }
    }

    private Sequence GetShrinkSequenceWithEndSignal(List<Vector2> aListOfCells)
    {
        return DOTween.Sequence().OnComplete(() => EventManager.OnTokenDestructionEndSignal(aListOfCells));
    } 
}