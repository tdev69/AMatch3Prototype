using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SideTypes {down, left, right, up};
public enum MatchTypes {bomb, columnWipe, empty, lineWipe, normal, scissors, totalWipe, tripleColumnWipe, tripleLineWipe}

public class MatchManager : MonoBehaviour
{
    [SerializeField] int minValidMatchNum = 3;
    private GridManager gridManager = null;


    //*********************************Matching Functions************************
    private Dictionary<SideTypes, int> GetMatches(int x, int y, TokenTypes typeToCheck = TokenTypes.fromCell) //, bool lineOnly = false)
    {
        bool lineOnly = false;
        Dictionary<SideTypes, int> matches = new Dictionary<SideTypes, int>();

        if (y == 0)
        {
            lineOnly = true;    
        }

        matches.Add(SideTypes.left, GetLeftMatches(x, y, typeToCheck));
        matches.Add(SideTypes.right, GetRightMatches(x, y, typeToCheck));

        if (lineOnly == false)
        {
            matches.Add(SideTypes.down, GetDownMatches(x, y, typeToCheck));
            matches.Add(SideTypes.up, GetUpMatches(x, y, typeToCheck));
        }

        else
        {
            matches.Add(SideTypes.down, 0);
            matches.Add(SideTypes.up, 0);
        }

        return matches;
    }

    public bool CheckMatchesAvailability()
    {
        List<TokenTypes> bottomLineColours = CollectBottomLineColours();
        
        foreach (TokenTypes colour in bottomLineColours)
        {
            for (int y = 1; y < this.gridManager.GetNumberOfDisplayedRows(); y++)
            {
                for (int x = 0; x <= this.gridManager.GetNumberOfDisplayedColumns(); x++)
                {
                    if (MatchDetector(x, y, colour) == true)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private List<TokenTypes> CollectBottomLineColours()
    {
        List<TokenTypes> bottomLineColours = new List<TokenTypes>();

        for (int x = 1; x <= this.gridManager.GetNumberOfDisplayedColumns(); x++)
        {
            if (bottomLineColours.Contains(this.gridManager.GetTokenTypeOfCell(x , 0)) == false)
            {
                bottomLineColours.Add(this.gridManager.GetTokenTypeOfCell(x, 0));
            }
        }

        return bottomLineColours;
    }

    ///Returns true if a match is detected, false otherwise. Arg typeToCheck allows to check with token from another cell
    public bool MatchDetector(int x, int y, TokenTypes typeToCheck = TokenTypes.fromCell) //, bool lineOnly = false)
    {
        bool lineOnly = false;
        Dictionary<SideTypes, int> thisCellMatches = GetMatches(x, y, typeToCheck);

        if (y == 0)
        {
            lineOnly = true;
        }

        if (lineOnly == false)
        {
            if (thisCellMatches[SideTypes.left] + thisCellMatches[SideTypes.right] >= this.minValidMatchNum - 1
                || thisCellMatches[SideTypes.up] + thisCellMatches[SideTypes.down] >= this.minValidMatchNum - 1)
            {
                return true;
            }
        }

        else if (lineOnly == true)
        {
            if (thisCellMatches[SideTypes.left] + thisCellMatches[SideTypes.right] >= this.minValidMatchNum - 1)
            {
                return true;
            }
        }

        return false;
    }

    private int GetLeftMatches(int x, int y, TokenTypes typeToCheck)
    {
        int numberOfMatches = 0;
        TokenTypes lookForColour = typeToCheck;

        if (lookForColour == TokenTypes.fromCell)
        {
            lookForColour = this.gridManager.GetTokenTypeOfCell(x, y);
        }

        for (int xPos = x - 1; xPos >= 1; xPos--)
        {
            if (this.gridManager.GetTokenTypeOfCell(xPos, y) == lookForColour)
            {
                numberOfMatches += 1;
            }

            else
            {
                break;
            }
        }

        return numberOfMatches;
    }

    private int GetRightMatches(int x, int y, TokenTypes typeToCheck)
    {
        int numberOfMatches = 0;
        TokenTypes lookForColour = typeToCheck;

        if (lookForColour == TokenTypes.fromCell)
        {
            lookForColour = this.gridManager.GetTokenTypeOfCell(x, y);
        }

        for (int xPos = x + 1; xPos <= this.gridManager.GetNumberOfDisplayedColumns(); xPos++)
        {
            if (this.gridManager.GetTokenTypeOfCell(xPos, y) == lookForColour)
            {
                numberOfMatches += 1;
            }

            else
            {
                break;
            }
        }

        return numberOfMatches;
    }

    private int GetDownMatches(int x, int y, TokenTypes typeToCheck)
    {
        int numberOfMatches = 0;
        TokenTypes lookForColour = typeToCheck;

        if (lookForColour == TokenTypes.fromCell)
        {
            lookForColour = this.gridManager.GetTokenTypeOfCell(x, y);
        }

        for (int yPos = y - 1; yPos > 0; yPos--)
        {
            if (this.gridManager.GetTokenTypeOfCell(x, yPos) == lookForColour)
            {
                numberOfMatches += 1;
            }

            else
            {
                break;
            }
        }

        return numberOfMatches;
    }
    
    private int GetUpMatches(int x, int y, TokenTypes typeToCheck)
    {
        int numberOfMatches = 0;
        TokenTypes lookForColour = typeToCheck;

        if (lookForColour == TokenTypes.fromCell)
        {
            lookForColour = this.gridManager.GetTokenTypeOfCell(x, y);
        }

        for (int yPos = y + 1; yPos < this.gridManager.GetNumberOfDisplayedRows(); yPos++)
        {
            if (this.gridManager.GetTokenTypeOfCell(x, yPos) == lookForColour)
            {
                numberOfMatches += 1;
            }

            else
            {
                break;
            }
        }

        return numberOfMatches;
    }

    //*********************************Types of Match***********************************
    private bool IsNormalMatch(Dictionary<SideTypes, int> allMatches)
    {
        int maxMatch = 2;
        int leftMatches = allMatches[SideTypes.left];
        int rightMatches = allMatches[SideTypes.right];
        int upMatches = allMatches[SideTypes.up];
        int downMatches = allMatches[SideTypes.down];

        if ((leftMatches + rightMatches) == maxMatch || (downMatches + upMatches) == maxMatch)
        {
            return true;
        }

        return false;
    }

    private bool IsBombMatch(Dictionary<SideTypes, int> allMatches)
    {
        int minMatch = 2;
        int leftMatches = allMatches[SideTypes.left];
        int rightMatches = allMatches[SideTypes.right];
        int upMatches = allMatches[SideTypes.up];
        int downMatches = allMatches[SideTypes.down];

        if (leftMatches + rightMatches >= minMatch && upMatches + downMatches >= minMatch 
            && upMatches != 0 && downMatches != 0 && leftMatches != 0 && rightMatches != 0)
        {
            return true;
        }

        return false;
    }

    private bool IsLineWipe(Dictionary<SideTypes, int> allMatches)
    {
        int minMatch = 3;
        int leftMatches = allMatches[SideTypes.left];
        int rightMatches = allMatches[SideTypes.right];

        if (leftMatches + rightMatches >= minMatch)
        {
            return true;;
        }

        return false;
    }

    private bool IsColumnWipe(Dictionary<SideTypes, int>allMatches)
    {
        int minMatch = 3;
        int upMatches = allMatches[SideTypes.up];
        int downMatches = allMatches[SideTypes.down];

        if (upMatches + downMatches >= minMatch)
        {
            return true;;
        }

        return false;    
    }

    private bool IsScissors(Dictionary<SideTypes, int> allMatches)
    {
        int minMatch = 2;
        int leftMatches = allMatches[SideTypes.left];
        int rightMatches = allMatches[SideTypes.right];
        int upMatches = allMatches[SideTypes.up];
        int downMatches = allMatches[SideTypes.down];

        if (upMatches >= minMatch && (leftMatches >= minMatch || rightMatches >= minMatch))
        {
            return true;
        }

        else if (downMatches >= minMatch && (leftMatches >= minMatch || rightMatches >= minMatch))
        {
            return true;
        }

        return false;
    }

    private bool IsTripleLineWipe(Dictionary<SideTypes, int> allMatches)
    {
        int vertMinMatch = 2;
        int horMinMatch = 4;
        int leftMatches = allMatches[SideTypes.left];
        int rightMatches = allMatches[SideTypes.right];
        int upMatches = allMatches[SideTypes.up];
        int downMatches = allMatches[SideTypes.down];

        if ((leftMatches + rightMatches) >= horMinMatch && (upMatches >= vertMinMatch || downMatches >= vertMinMatch))
        {
            return true;
        }

        return false;
    }

    private bool IsTripleColumnWipe(Dictionary<SideTypes, int> allMatches)
    {
        int vertMinMatch = 4;
        int horMinMatch = 2;
        int leftMatches = allMatches[SideTypes.left];
        int rightMatches = allMatches[SideTypes.right];
        int upMatches = allMatches[SideTypes.up];
        int downMatches = allMatches[SideTypes.down];

        if ((upMatches + downMatches) >= vertMinMatch && (leftMatches >= horMinMatch || rightMatches >= horMinMatch))
        {
            return true;
        }

        return false;
    }

    private bool IsTotalWipe(Dictionary<SideTypes, int> allMatches)
    {
        int minMatch = 2;
        int leftMatches = allMatches[SideTypes.left];
        int rightMatches = allMatches[SideTypes.right];
        int upMatches = allMatches[SideTypes.up];
        int downMatches = allMatches[SideTypes.down];

        if (leftMatches >= minMatch && rightMatches >= minMatch && downMatches >= minMatch && upMatches >= minMatch)
        {
            return true;
        }

        return false;
    }

    public (MatchTypes, Dictionary<SideTypes, int>) GetCompleteMatchInfo(Vector2 cellCoordinates)
    {
        Dictionary<SideTypes, int> theMatches = GetMatches((int)cellCoordinates.x, (int)cellCoordinates.y);

        switch (true)
        {
            case var test when (test = IsTotalWipe(theMatches)):
                //print("Total Wipe");
                GetCleanDictionary(cellCoordinates, MatchTypes.totalWipe, theMatches);
                return (MatchTypes.totalWipe, theMatches);
            
            case var test when (test = IsTripleColumnWipe(theMatches)):
               // print ("triple column wipe");
                GetCleanDictionary(cellCoordinates, MatchTypes.tripleColumnWipe, theMatches);
                return (MatchTypes.tripleColumnWipe, theMatches);
            
            case var test when (test = IsTripleLineWipe(theMatches)):
                //print("triple line wipe");
                GetCleanDictionary(cellCoordinates, MatchTypes.tripleLineWipe, theMatches);
                return (MatchTypes.tripleLineWipe, theMatches);
            
            case var test when (test = IsScissors(theMatches)):
                //print("scissors");
                GetCleanDictionary(cellCoordinates, MatchTypes.scissors, theMatches);
                return (MatchTypes.scissors, theMatches);
                        
            case var test when (test = IsBombMatch(theMatches)):
                //print("bomb match");
                GetCleanDictionary(cellCoordinates, MatchTypes.bomb, theMatches);
                return (MatchTypes.bomb, theMatches);
            
            case var test when (test = IsColumnWipe(theMatches)):
                //print("column wipe");
                GetCleanDictionary(cellCoordinates, MatchTypes.columnWipe, theMatches);
                return (MatchTypes.columnWipe, theMatches);

            case var test when (test = IsLineWipe(theMatches)):
                //print("line wipe");
                GetCleanDictionary(cellCoordinates, MatchTypes.lineWipe, theMatches);
                return (MatchTypes.lineWipe, theMatches);

            case var test when (test = IsNormalMatch(theMatches)):
                //print ("normal match");
                GetCleanDictionary(cellCoordinates, MatchTypes.normal, theMatches);
                return (MatchTypes.normal, theMatches);

            default:
                return (MatchTypes.empty, theMatches);
        }
    }

    //************************Clean Dictionary of matches*******************************
    private Dictionary<SideTypes, int> GetCleanDictionary(Vector2 cellCoordinates, MatchTypes aTypeOfMatch, Dictionary<SideTypes, int> aDictionary)
    {
        switch(aTypeOfMatch)
        {
            case MatchTypes.bomb:
                CleanBombMatch(aDictionary);
                break;

            case MatchTypes.columnWipe:
                CleanColumnWipe(aDictionary);
                break;

            case MatchTypes.lineWipe:
                CleanLineWipe(aDictionary);
                break;

            case MatchTypes.normal:
                CleanNormalMatch(aDictionary);
                break;

            case MatchTypes.scissors:
                CleanScissors(aDictionary);
                break;

            case MatchTypes.totalWipe:
                CleanTotalWipe(aDictionary);
                break;

            case MatchTypes.tripleColumnWipe:
                CleanTripleColumnWipe(aDictionary);
                break;

            case MatchTypes.tripleLineWipe:
                CleanTripleLineWipe(aDictionary);
                break;
        }

        return aDictionary;
    }

    private void CleanBombMatch(Dictionary<SideTypes, int> aDictionary)
    {
        int maxMatch = 1;

        foreach(SideTypes side in SideTypes.GetValues(typeof(SideTypes)))
        {
            if (aDictionary[side] > maxMatch)
            {
                aDictionary[side] = maxMatch;
            }
        }
    }

    private void CleanColumnWipe(Dictionary<SideTypes, int> aDictionary)
    {
        int minMatch = 3;

        if (aDictionary[SideTypes.left] + aDictionary[SideTypes.right] >= 1)
        {
            aDictionary[SideTypes.left] = 0;
            aDictionary[SideTypes.right] = 0;
        }

        if (aDictionary[SideTypes.down] + aDictionary[SideTypes.up] > minMatch)
        {
            aDictionary[SideTypes.up] -= 1;
        }
    }

    private void CleanLineWipe(Dictionary<SideTypes, int> aDictionary)
    {
        int minMatch = 3;

        if (aDictionary[SideTypes.up] + aDictionary[SideTypes.down] >= 1)
        {
            aDictionary[SideTypes.up] = 0;
            aDictionary[SideTypes.down] = 0;
        }

        if (aDictionary[SideTypes.left] + aDictionary[SideTypes.right] > minMatch)
        {
            aDictionary[SideTypes.right] -= 1;
        }

    }

    private void CleanNormalMatch(Dictionary<SideTypes, int> aDictionary)
    {
        int minMatch = 2;

        if (aDictionary[SideTypes.down] + aDictionary[SideTypes.up] < minMatch)
        {
            aDictionary[SideTypes.down] = 0;
            aDictionary[SideTypes.up] = 0;
        }

        if (aDictionary[SideTypes.left] + aDictionary[SideTypes.right] < minMatch)
        {
            aDictionary[SideTypes.left] = 0;
            aDictionary[SideTypes.right] = 0;
        }
    }

    private void CleanScissors(Dictionary<SideTypes, int> aDictionary)
    {
        int minMatch = 2;

        foreach(SideTypes side in SideTypes.GetValues(typeof(SideTypes)))
        {
            if (aDictionary[side] > minMatch)
            {
                aDictionary[side] = minMatch;
            }

            else if(aDictionary[side] < minMatch)
            {
                aDictionary[side] = 0;
            }
        }
    }

    private void CleanTotalWipe(Dictionary<SideTypes, int> aDictionary)
    {
        int minMatch = 2;

        foreach(SideTypes side in SideTypes.GetValues(typeof(SideTypes)))
        {
            if (aDictionary[side] > minMatch)
            {
                aDictionary[side] = minMatch;
            }
        } 
    }

    private void CleanTripleColumnWipe(Dictionary<SideTypes, int> aDictionary)
    {
        int minMatch = 2;

        if (aDictionary[SideTypes.left] < minMatch)
        {
            aDictionary[SideTypes.left] = 0;
        }

        else if (aDictionary[SideTypes.right] < minMatch)
        {
            aDictionary[SideTypes.right] = 0;
        }
    }

    private void CleanTripleLineWipe(Dictionary<SideTypes, int> aDictionary)
    {
        int minMatch = 2;

        if (aDictionary[SideTypes.up] < minMatch)
        {
            aDictionary[SideTypes.up] = 0;
        }

        else if (aDictionary[SideTypes.down] < minMatch)
        {
            aDictionary[SideTypes.down] = 0;
        }
    }


    //*********************************Awake and Start Functions************************
    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }
}
