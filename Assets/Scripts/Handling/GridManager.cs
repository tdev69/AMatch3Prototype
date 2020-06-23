using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int displayedRows = 0;
    [SerializeField] private int displayedColumns = 0;
    [SerializeField] private SOTile tileTemplate = null; //Tiles are just the information container. Tokens are the objects displayed for player to interact with. They are contained in Tiles.
    private SOTile[,] grid = new SOTile[9,16];
    private List<GameObject> tileDisplays = new List<GameObject>();
    private MatchManager matchManager = null;
    private TokenMovementManager tokenMovement = null;
    private TokenPool theTokenPool = null; //The script handling management of tokens incl. spawning. Has to be the same for all Tiles.
    private bool removeMatches = true; //Used only at beginning to remove matches at start.

    
    //********************Grid Operations*****************************
    private void FillGridWithTiles()
    {   
        for (int y = 0; y < this.grid.GetLength(1); y++)
        {

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Vector3 position = SpawnPosition(x, y);
                Vector2 gridPosition =  new Vector2(x, y);

                this.grid[x, y] = Instantiate(tileTemplate);
                this.grid[x, y].SetAllPositions(position, gridPosition);
                this.grid[x,y].SetTokenPool(theTokenPool);

                if (x > 0 && x <= displayedColumns && y < displayedRows)
                {
                    this.grid[x, y].SetTileDisplay(tileDisplays[0]);
                    tileDisplays.RemoveAt(0);
                }
            }
        }
    }

    private Vector3 SpawnPosition(int x, int y)
    {
        Vector3 spawnPos = new Vector3 (x, 0, 0);

        if (y >= 1) 
        {
            spawnPos.y = y + 1;
        }

        return spawnPos;
    }

    ///If no arg is passed, an empty token is placed on the tile.
    ///Empty token should be only used for debug only.
    private void SetSpecificTileToken(Vector2 gridCoordinates, TokenTypes aTokenType = TokenTypes.empty)
    {
        this.grid[(int)gridCoordinates.x, (int)gridCoordinates.y].SetTokenType(aTokenType); 
    }


    //********************Refill Grid Operations*****************************
    public void RefillGridWithTokens()
    {
        List<Vector2> emptyTiles = FindEmptyTiles();
        GenerateNewToken(aListOfEmptyCells: emptyTiles, typeOrDisplay: 0);

        if (this.removeMatches == true)
        {
            bool scanNeeded = RemoveMatchesAtStart();

            while (scanNeeded == true)
            {
                scanNeeded = RemoveMatchesAtStart();
            }

            this.removeMatches = false;
        }

        GenerateNewToken(aListOfEmptyCells: emptyTiles, typeOrDisplay: 1);
        RefillEmptyTiles(emptyTiles);
        SetSideTokens();
        //this.tokenMovement.MoveAllTokensToPosition(); //USED HERE AS DEBUG. MOVE IT OUT WHEN STATES ARE IMPLEMENTED
    }

    private void RefillEmptyTiles(List<Vector2> aList)
    {
        for (int index = 0; index < aList.Count; index++)
        {
            int xPos = (int)aList[index].x;
            int yPos = (int)aList[index].y;
            bool tokenFound = false;
            int yTarget = yPos + 1;

            while (tokenFound == false && yTarget < this.grid.GetLength(1))
            {
                if (this.grid[xPos, yTarget].GetTokenType() != TokenTypes.empty)
                {
                    Vector2 thisCell = new Vector2(xPos, yPos);
                    Vector2 targetCell = new Vector2(xPos, yTarget);
                    TransferCellInfo(startCellGridCoords: targetCell, endCellGridCoords: thisCell);
                    SetCellToEmpty(xPos, yTarget, returnTokenToPool: false);
                    tokenFound = true;

                    if (yTarget < this.displayedRows)
                    {
                        aList.Add(new Vector2(xPos, yTarget));
                    }
                }

                else
                {
                    yTarget += 1;
                }
            }               
        }
    }

    private List<Vector2> FindEmptyTiles()
    {
        List<Vector2> emptyTilesGridCoordinates = new List<Vector2>();

        for (int x = 1; x <= this.displayedColumns; x++)
        {
            
            for (int y = 0; y < this.displayedRows; y++)
            {
                if (this.grid[x, y].GetTokenType() == TokenTypes.empty)
                {
                    emptyTilesGridCoordinates.Add(new Vector2 (x, y));
                }
            }
        }

        return emptyTilesGridCoordinates;
    }

    ///If set int to 0 for token type and 1 for token display
    private void GenerateNewToken(List<Vector2> aListOfEmptyCells, int typeOrDisplay)
    {
        Dictionary<int, int> spawnsPerColumn = CountEmptyCellsPerColumns(aListOfEmptyCells);

        foreach (KeyValuePair<int, int> kv in spawnsPerColumn)
        {
            for (int y = this.displayedRows; y < kv.Value + this.displayedRows; y++)
            {
                if (typeOrDisplay == 0)
                {
                    this.grid[kv.Key, y].SetRandomTokenType();
                }

                else if (typeOrDisplay == 1)
                {
                    this.grid[kv.Key, y].SetTokenDisplay();
                }

                else
                {
                    Debug.LogWarning("Pass int = 0 for type or int = 1 for display");
                }
            }
        }
    }

    ///Returns a dictionary where key is the column index and value is number of empty tiles
    private Dictionary<int, int> CountEmptyCellsPerColumns(List<Vector2> aListOfEmptyCells)
    {
        Dictionary<int, int> emptyCellsPerColumns = new Dictionary<int, int>();

        foreach(Vector2 v in aListOfEmptyCells)
        {
            int x = (int)v.x;

            if (emptyCellsPerColumns.ContainsKey(x) == true)
            {
                emptyCellsPerColumns[x] += 1;
            }

            else
            {
                emptyCellsPerColumns.Add(x, 1);
            }
        }

        return emptyCellsPerColumns;
    }

    //Used for smoooth carousel look when moving the bottom line.
    private void SetSideTokens()
    {
        int hiddenCellRight = this.grid.GetLength(0) - 1;
        this.grid[0, 0].SetTokenType(this.grid[displayedColumns, 0].GetTokenType());
        this.grid[0, 0].SetTokenDisplay();
        this.grid[hiddenCellRight, 0].SetTokenType(this.grid[1, 0].GetTokenType());
        this.grid[hiddenCellRight, 0].SetTokenDisplay();
    }

    //********************Used only for the first spawn*****************************
    
    ///Returns True if a match was removed, false otherwise
    private bool RemoveMatchesAtStart()
    {
        bool matchRemoved = false;

        for (int y = this.displayedRows; y < this.grid.GetLength(1); y++)
        {

            for (int x = 1; x <= this.displayedColumns; x++)
            {
                if (matchManager.MatchDetector(x, y) == true)
                {
                    TokenTypes thisCellTokenType = this.grid[x, y].GetTokenType();
                    TokenTypes prospectTokenType = thisCellTokenType;

                    while (thisCellTokenType == prospectTokenType)
                    {
                        this.grid[x, y].SetRandomTokenType();
                        prospectTokenType = this.grid[x, y].GetTokenType();
                    }

                    matchRemoved = true; 
                }
            }
        }

        return matchRemoved;
    }


    //********************Display Tiles*****************************
    public void DisplayAllTiles()
    {
        for (int y = 0; y < this.displayedRows; y++)
        {

            for (int x = 0; x < this.displayedColumns; x++)
            {
                this.grid[x, y].DisplayTile();
            }
        }
    }

    public void DisplayOneTile(int x, int y)
    {
        if (x < this.displayedColumns && y < this.displayedRows)
        {
            this.grid[x, y].DisplayTile();
        }

        else
        {
            Debug.LogWarning("Only tiles already in scene can be displayed", this);
        }
    }

    //********************Interface: Getter Functions*****************************
    public TokenTypes GetTokenTypeOfCell(int gridPosX, int gridPosY)
    {
        return this.grid[gridPosX, gridPosY].GetTokenType();
    }

    public GameObject GetTokenDisplay(int gridPosX, int gridPosY)
    {
        return this.grid[gridPosX, gridPosY].GetTokenDisplay();
    }

    public SOTile GetTile(int gridPosX, int gridPosY)
    {
        return this.grid[gridPosX, gridPosY];
    }

    public int GetNumberOfDisplayedRows()
    {
        return this.displayedRows;
    }

    public int GetNumberOfDisplayedColumns()
    {
        return this.displayedColumns;
    }

    public int GetGridNumberOfColumns()
    {
        return this.grid.GetLength(0);
    }

    public Vector3 GetCellWorldCoordinates(int gridPosX, int gridPosY)
    {
        return this.grid[gridPosX, gridPosY].GetWorldPosition();
    }

    //********************Interface: Setter Functions*****************************
    public void SetTokenTypeOfCell(int gridPosX, int gridPosY, TokenTypes aTokenType)
    {
        this.grid[gridPosX, gridPosY].SetTokenType(aTokenType);
    }

    ///Takes the token type ffrom the cell, find the equivalent game objects and teleports it to cell
    ///DO NOT USE ON VISIBLE CELLS
    public void SetTokenDisplay(int gridPosX, int gridPosY)
    {
        this.grid[gridPosX, gridPosY].SetTokenDisplay();
    }

    public void SetTokenDisplay(int gridPosX, int gridPosY, GameObject aToken)
    {
        this.grid[gridPosX, gridPosY].SetTokenDisplay(aToken); 
    }

    public void SetTokenOfCell(int gridPosX, int gridPosY, TokenTypes aTokenType, GameObject aToken)
    {
        this.grid[gridPosX, gridPosY].SetToken(aTokenType, aToken);
        this.grid[gridPosX, gridPosY].MoveTokenToPosition(this.grid[gridPosX, gridPosY].GetWorldPosition(), teleport: true);
    }

    public void SetCellToEmpty(int gridPosX, int gridPosY, bool returnTokenToPool = true)
    {
        if (returnTokenToPool == true)
        {
            this.grid[gridPosX, gridPosY].SetToEmpty();
        }

        else
        {
            this.grid[gridPosX, gridPosY].SetToken(aTokenType: TokenTypes.empty, aToken: null);
        }
    }

    //********************Interface: Manipulate & Message Cells Functions*****************************

    ///Cell at coordinates provided will move its displayed token to its world position
    public Tween MoveTokenDisplayedToCell(int gridPosX, int gridPosY)
    {
        Tween aTween = this.grid[gridPosX, gridPosY].MoveTokenToTile();
        return aTween;
    }

    ///Moves the token displayed at coordinates to world coordinates. Concerns only graphic display.
    public Tween MoveTokenToPosition(Vector2 cellCoordinates, Vector3 destination)
    {
        return this.grid[(int)cellCoordinates.x, (int)cellCoordinates.y].MoveTokenToPosition(destination);
    }

    ///Transfer tokenType and tokenDisplay from start cell to end cell
    public void TransferCellInfo(Vector2 startCellGridCoords, Vector2 endCellGridCoords)
    {
        TokenTypes theTokenType = this.grid[(int)startCellGridCoords.x, (int)startCellGridCoords.y].GetTokenType();
        GameObject theTokenDisplayed = this.grid[(int)startCellGridCoords.x, (int)startCellGridCoords.y].GetTokenDisplay();
        this.grid[(int)endCellGridCoords.x, (int)endCellGridCoords.y].SetToken(theTokenType, theTokenDisplayed);
    }

    ///Swaps information between cell on grid and cell on bottom line
    public void SwapCellsInfoGridLine(Vector2 gridCoordinates)
    {
        TokenTypes gridCellTokenType = this.grid[(int)gridCoordinates.x, (int)gridCoordinates.y].GetTokenType();
        TokenTypes lineCellTokenType = this.grid[(int)gridCoordinates.x, 0].GetTokenType();
        GameObject gridCellTokenDisplay = this.grid[(int)gridCoordinates.x, (int)gridCoordinates.y].GetTokenDisplay();
        GameObject lineCellTokenDisplay = this.grid[(int)gridCoordinates.x, 0].GetTokenDisplay();

        this.grid[(int)gridCoordinates.x, (int)gridCoordinates.y].SetTokenType(lineCellTokenType);
        this.grid[(int)gridCoordinates.x, (int)gridCoordinates.y].SetTokenDisplay(lineCellTokenDisplay);
        this.grid[(int)gridCoordinates.x, 0].SetTokenType(gridCellTokenType);
        this.grid[(int)gridCoordinates.x, 0].SetTokenDisplay(gridCellTokenDisplay);
    }

    ///Destroys only the graphic token. Does not empty cell info!
    public Tween DestroyToken(Vector2 cellCoordinates)
    {
        return this.grid[(int)cellCoordinates.x, (int)cellCoordinates.y].DestroyToken();
    }


    //********************Awake and Start Functions*****************************
    private void CollectTilesInScene()
    {
        Transform tileHolder = this.gameObject.transform.GetChild(0);
        int numOfChildren = tileHolder.gameObject.transform.childCount;

        for (int x = 0; x < numOfChildren; x++)
        {
            tileDisplays.Add(tileHolder.gameObject.transform.GetChild(x).gameObject);
        }    
    }
    
    private void Awake()
    {
        CollectTilesInScene();
        this.theTokenPool = GetComponent<TokenPool>();
        this.matchManager = GetComponent<MatchManager>();
        this.tokenMovement = GetComponent<TokenMovementManager>();
        FillGridWithTiles();
    }
}
