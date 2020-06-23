using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Matching", menuName = "SO/Matching")]
public class SOMatching : SOState
{   
    (MatchTypes, Dictionary<SideTypes, int>) completeInfo;
    MatchTypes typeOfMatch = MatchTypes.empty;
    List<Vector2> cellsToMove = new List<Vector2>();
    List<Vector2> cellsToDestroy = new List<Vector2>();
    Vector2 cellCoordinates = Vector2.zero;
    Dictionary<SideTypes, int> cleanDictionary = new Dictionary<SideTypes, int>();

    public override void StateAction(Vector2 aCellCoordinates, bool fromRefill = false)
    {
        EventManager.onTokenDestructionEnd += UpdateCells;
        EventManager.onTokenGatherEnd += DisplaySpecialEffect;
        EventManager.onEffectDisplayEnd += NextStateTransition;
        EventManager.onLaserShotHit += DestroyNextCell;
        this.cellCoordinates = aCellCoordinates;
        this.completeInfo = this.matchManager.GetCompleteMatchInfo(this.cellCoordinates);
        this.cleanDictionary = completeInfo.Item2;
        this.typeOfMatch = completeInfo.Item1;
        GetCellHandlingInformation();

        if (typeOfMatch == MatchTypes.normal)
        {
            this.tokenMovement.TokenNormalMatch(this.cellsToDestroy);
        }

        else if(fromRefill == false)
        {
            this.tokenMovement.TokenGatherSpecialMatch(this.cellsToMove, this.cellCoordinates);
        }
    }


    //*******************************Cell Coords Gathering Functions***********************************
    private void GetCellHandlingInformation()
    {
        this.cellsToMove = new List<Vector2>();
        this.cellsToDestroy = new List<Vector2>();

        switch(this.typeOfMatch)
        {
            case (MatchTypes.bomb):
                GetCellsToMove();
                GetDestroyCellsBomb();
                break;

            case (MatchTypes.columnWipe):
                GetCellsToMove();
                this.cellsToDestroy = GetDestroyCellsColumnWipe(this.cellCoordinates);
                break;

            case (MatchTypes.lineWipe):
                GetCellsToMove();
                this.cellsToDestroy = GetDestroyCellsLineWipe(this.cellCoordinates);
                break;

            case (MatchTypes.normal):
                GetDestroyCellsNormal();
                break;
            
            case (MatchTypes.scissors):
                GetCellsToMove();
                GetDestroyCellsScissors();
                break;

            case (MatchTypes.tripleColumnWipe):
                GetCellsToMove();
                GetDestroyCellsTripleColumnWipe();
                break;
            
            case (MatchTypes.tripleLineWipe):
                GetCellsToMove();
                GetDestroyCellsTripleLineWipe();
                break;
            
            case (MatchTypes.totalWipe):
                GetCellsToMove();
                GetDestroyCellsTotalWipe();
                break;
            
            default:
                break;
                
        }
    }

    private void GetCellsToMove()
    {
        this.cellsToMove = new List<Vector2>();
        for (int x = 1; x <= this.cleanDictionary[SideTypes.down]; x++)
        {
            this.cellsToMove.Add(new Vector2(this.cellCoordinates.x, this.cellCoordinates.y - x));
        }

        for (int x = 1; x <= this.cleanDictionary[SideTypes.left]; x++)
        {
            this.cellsToMove.Add(new Vector2(this.cellCoordinates.x - x, this.cellCoordinates.y));
        }

        for (int x = 1; x <= this.cleanDictionary[SideTypes.right]; x++)
        {
            this.cellsToMove.Add(new Vector2(this.cellCoordinates.x + x, this.cellCoordinates.y));
        }

        for (int x = 1; x <= this.cleanDictionary[SideTypes.up]; x++)
        {
            this.cellsToMove.Add(new Vector2(this.cellCoordinates.x, this.cellCoordinates.y + x));
        }
    }

    private void GetDestroyCellsNormal()
    {
        this.cellsToDestroy = new List<Vector2> ();
        this.cellsToDestroy.Add(this.cellCoordinates);

        for (int x = 1; x <= this.cleanDictionary[SideTypes.down]; x++)
        {
            this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x, this.cellCoordinates.y - x));
        }

        for (int x = 1; x <= this.cleanDictionary[SideTypes.left]; x++)
        {
            this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x - x, this.cellCoordinates.y));
        }

        for (int x = 1; x <= this.cleanDictionary[SideTypes.right]; x++)
        {
            this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x + x, this.cellCoordinates.y));
        }

        for (int x = 1; x <= this.cleanDictionary[SideTypes.up]; x++)
        {
            this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x, this.cellCoordinates.y + x));
        }
    }

    private void GetDestroyCellsTotalWipe()
    {
        this.cellsToDestroy.Add(this.cellCoordinates);
        this.cellsToDestroy.AddRange(this.cellsToMove);

        for (float y = 1; y < this.gridManager.GetNumberOfDisplayedRows(); y++)
        {
            for (float x = 1; x <= this.gridManager.GetNumberOfDisplayedColumns(); x++)
            {
                Vector2 cell = new Vector2(x, y);
                
                if (this.cellsToMove.Contains(cell) != true && cell != this.cellCoordinates)
                {    
                    this.cellsToDestroy.Add(cell);
                }
            }
        }
    }

    private List<Vector2> GetDestroyCellsLineWipe(Vector2 refCellCoordinates)
    {
        List<Vector2> theList = new List<Vector2>(); 
        for (int x = 1; x <= this.gridManager.GetNumberOfDisplayedColumns(); x++)
        {
            theList.Add(new Vector2(x, refCellCoordinates.y));
        }

        return theList;
    }

    private List<Vector2> GetDestroyCellsColumnWipe(Vector2 refCellCoordinates)
    {
        List<Vector2> theList = new List<Vector2>();
        for (int y = 1; y <= this.gridManager.GetNumberOfDisplayedColumns(); y++)
        {
            theList.Add(new Vector2(refCellCoordinates.x, y));
        }

        return theList;
    }

    private void GetDestroyCellsTripleLineWipe()
    {
        if (this.cleanDictionary[SideTypes.up] > 0)
        {
            for (float y = 0f; y <= 2; y++ )
            {
                Vector2 coordinates = new Vector2(this.cellCoordinates.x, this.cellCoordinates.y + y);
                this.cellsToDestroy.AddRange(GetDestroyCellsLineWipe(coordinates));
            }
        }

        else
        {
            for (float y = 0f; y <= 2; y++ )
            {
                Vector2 coordinates = new Vector2(this.cellCoordinates.x, this.cellCoordinates.y - y);
                this.cellsToDestroy.AddRange(GetDestroyCellsLineWipe(coordinates));
            }
        } 
    }

    private void GetDestroyCellsTripleColumnWipe()
    {
        if (this.cleanDictionary[SideTypes.left] > 0)
        {
            for (float x = 0f; x <= 2; x++)
            {
                Vector2 coordinates = new Vector2(this.cellCoordinates.x - x, this.cellCoordinates.y);
                this.cellsToDestroy.AddRange(GetDestroyCellsColumnWipe(coordinates));
            }
        }

        else
        {
            for (float x = 0f; x <= 2; x++)
            {
                Vector2 coordinates = new Vector2(this.cellCoordinates.x + x, this.cellCoordinates.y);
                this.cellsToDestroy.AddRange(GetDestroyCellsColumnWipe(coordinates));
            }
        }
    }

    private void GetDestroyCellsBomb()
    {
        this.cellsToDestroy = new List<Vector2>();
        this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x - 1, this.cellCoordinates.y + 1));
        this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x + 1, this.cellCoordinates.y + 1));
        this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x - 1, this.cellCoordinates.y - 1));
        this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x + 1, this.cellCoordinates.y - 1));
        this.cellsToDestroy.Add(this.cellCoordinates);
        this.cellsToDestroy.AddRange(this.cellsToMove);
    }

    private void GetDestroyCellsScissors()
    {
        this.cellsToDestroy = new List<Vector2>();
        this.cellsToDestroy.Add(this.cellCoordinates);
        this.cellsToDestroy.AddRange(this.cellsToMove);

        if (this.cleanDictionary[SideTypes.up] > 0 && this.cleanDictionary[SideTypes.right] > 0)
        {
            for (int x = 1; x <= 2; x++)
            {
                for (int y = 1; y <= 2; y++)
                {
                    this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x + x, this.cellCoordinates.y + y));
                }
            }
        }

        else if (this.cleanDictionary[SideTypes.up] > 0 && this.cleanDictionary[SideTypes.left] > 0)
        {
            for (int x = 1; x <= 2; x++)
            {
                for (int y = 1; y <= 2; y++)
                {
                    this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x - x, this.cellCoordinates.y + y));
                }
            }
        }

        else if (this.cleanDictionary[SideTypes.down] > 0 && this.cleanDictionary[SideTypes.right] > 0)
        {
            for (int x = 1; x <= 2; x++)
            {
                for (int y = 1; y <= 2; y++)
                {
                    this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x + x, this.cellCoordinates.y - y));
                }
            }    
        }

        else if (this.cleanDictionary[SideTypes.down] > 0 && this.cleanDictionary[SideTypes.left] > 0)
        {
            for (int x = 1; x <= 2; x++)
            {
                for (int y = 1; y <= 2; y++)
                {
                    this.cellsToDestroy.Add(new Vector2(this.cellCoordinates.x - x, this.cellCoordinates.y - y));
                }
            }
        }
    }

    private (bool down, bool left, bool right, bool up) GetDirections()
    {
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;

        if (this.cleanDictionary[SideTypes.left] > 0)
        {
            left = true;
        }

        if (this.cleanDictionary[SideTypes.right] > 0)
        {
            right = true;
        }

        if (this.cleanDictionary[SideTypes.up] > 0)
        {
            up = true;
        }

        if (this.cleanDictionary[SideTypes.down] > 0)
        {
            down = true;
        }

        return (down, left, right, up);
    }


    //***********************************Display Effect Order Functions*****************************************
    private void DisplaySpecialEffect()
    {
        Vector3 position = this.gridManager.GetCellWorldCoordinates((int)this.cellCoordinates.x, (int)cellCoordinates.y);
        switch (this.typeOfMatch)
        {
            case MatchTypes.bomb:
                this.effectsPool.DisplayBombEffect(position);
                this.tokenMovement.TokenShrinkSpecial(this.cellsToDestroy);
                break;

            case MatchTypes.columnWipe:
                this.effectsPool.DisplayWipeEffect(position, 90f);
                this.tokenMovement.TokenShrinkSpecial(this.cellsToDestroy);
                break;

            case MatchTypes.lineWipe:
                this.effectsPool.DisplayWipeEffect(position, 0f);
                this.tokenMovement.TokenShrinkSpecial(this.cellsToDestroy);
                break;

            case MatchTypes.scissors:
                Dictionary<SideTypes, int> cleanDictionary = this.completeInfo.Item2;
                this.effectsPool.DisplayScissorsEffect(position, cleanDictionary);
                this.tokenMovement.TokenShrinkSpecial(this.cellsToDestroy);
                break;
            
            case MatchTypes.tripleLineWipe:
                (bool down, bool left, bool right, bool up) vertDirection = GetDirections();
                
                if (vertDirection.down == true)
                {
                    this.effectsPool.DisplayTripleLineWipeEffect(position, SideTypes.down);
                }

                else
                {
                    this.effectsPool.DisplayTripleLineWipeEffect(position, SideTypes.up);
                }

                this.tokenMovement.TokenShrinkSpecial(this.cellsToDestroy);
                break;
            
            case MatchTypes.tripleColumnWipe:
                (bool down, bool left, bool right, bool up) horDirection = GetDirections();

                if (horDirection.left == true)
                {
                    this.effectsPool.DisplayTripleColumnWipeEffect(position, SideTypes.left);
                }

                else 
                {
                    this.effectsPool.DisplayTripleColumnWipeEffect(position, SideTypes.right);
                }

                this.tokenMovement.TokenShrinkSpecial(this.cellsToDestroy);
                break;
            
            case MatchTypes.totalWipe:
                List<Vector3> cellsWorldCoordinates = GetCellsWorldCoordinates();
                cellsWorldCoordinates.RemoveRange(1, 8);
                List<Vector2> firstWave = this.cellsToDestroy.GetRange(0, 8);
                this.cellsToDestroy.RemoveRange(0, 8);
                List<Vector3> randomizedWorldCoordinates = RandomizeCellCoordinates(cellsWorldCoordinates);
                this.effectsPool.DisplayTotalWipeEffect(randomizedWorldCoordinates);
                DestroyFirstWave(firstWave); 
                break;
        }
    }

    ///Randomizes the world cell coordinates. Keeps the list of associated cells in the same order.
    private List<Vector3> RandomizeCellCoordinates(List<Vector3> aListOfWorldCoords)
    {
        for (int x = 1; x < aListOfWorldCoords.Count; x++)
        {
            int randomIndex = UnityEngine.Random.Range(1, aListOfWorldCoords.Count);
            Vector3 currentV3 = aListOfWorldCoords[x];
            Vector2 currentV2 = this.cellsToDestroy[x];
            aListOfWorldCoords[x] = aListOfWorldCoords[randomIndex];
            aListOfWorldCoords[randomIndex] = currentV3;
            this.cellsToDestroy[x] = this.cellsToDestroy[randomIndex];
            this.cellsToDestroy[randomIndex] = currentV2;
        }

        return aListOfWorldCoords;
    }

    private void DestroyFirstWave(List<Vector2> aListOfCells)
    {
        foreach (Vector2 cell in aListOfCells)
        {
            this.tokenMovement.TokenShrinkSpecial(cell, false);
        }
    }

    private List<Vector3> GetCellsWorldCoordinates()
    {
        List<Vector3> cellsWorldCoordinates = new List<Vector3>();

        foreach (Vector2 cell in this.cellsToDestroy)
        {
            cellsWorldCoordinates.Add(this.gridManager.GetCellWorldCoordinates((int)cell.x, (int)cell.y));
        }

        return cellsWorldCoordinates;
    }

    private void DestroyNextCell()
    {
        if (this.cellsToDestroy.Count > 1)
        {
            tokenMovement.TokenShrinkSpecial(this.cellsToDestroy[0], false);
            this.cellsToDestroy.RemoveAt(0);
        }

        else
        {
            tokenMovement.TokenShrinkSpecial(this.cellsToDestroy[0], true);
            this.cellsToDestroy.RemoveAt(0);
        }

    }

    private void UpdateCells(List<Vector2> cellsToUpdate)
    {
        if (cellsToUpdate.Count > 1)
        {
            foreach (Vector2 cell in cellsToUpdate)
            {
                this.gridManager.SetCellToEmpty((int)cell.x, (int)cell.y);
            }
        }

        else
        {
            for (int y = 1; y < this.gridManager.GetNumberOfDisplayedRows(); y++)
            {
                for (int x = 1; x <= this.gridManager.GetNumberOfDisplayedColumns(); x++)
                {
                    this.gridManager.SetCellToEmpty(x, y);
                }
            }
        }
    }

    private void NextStateTransition()
    {

        SOState nextState = SetNextState(this.statesDictionary.GetState(StateNames.refill));
        StopListening();
        cellsToMove.Clear();
        cellsToDestroy.Clear();
        cleanDictionary.Clear();
        nextState.StateAction();
    }

    private void StopListening()
    {
        EventManager.onTokenDestructionEnd -= UpdateCells; 
        EventManager.onTokenGatherEnd -= DisplaySpecialEffect;
        EventManager.onEffectDisplayEnd -= NextStateTransition;
        EventManager.onLaserShotHit -= DestroyNextCell;
    }

    private void OnDisable()
    {
        StopListening();
    }
}
