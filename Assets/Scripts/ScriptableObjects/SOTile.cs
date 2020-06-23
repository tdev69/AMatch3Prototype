using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Use fromCell only when you  need any generic value for TokenType which is not used elsewhere for game purposes.!-- Equivalent of null
public enum TokenTypes {empty, blue, green, orange, purple, red, yellow, fromCell};

[CreateAssetMenu(fileName = "Tile", menuName = "SO/Tile") ]
public class SOTile : ScriptableObject
{
    [SerializeField] private GameObject onDisplay = null; //The token displayed on the tile
    [SerializeField] private TokenTypes thisTokenType = TokenTypes.empty;
    [SerializeField] private GameObject tileDisplay = null;

    private TokenPool tokenPool = null;
    private Vector2 gridPosition = Vector2.zero;
    private Vector3 worldPosition = Vector3.zero;

    //**********************Token Manipulation code**********************************
    ///Moves token to provided destination
    public Tween MoveTokenToPosition(Vector3 destination, bool teleport = false)
    {
        return this.onDisplay.GetComponent<Movement>().MoveToPosition(destination, teleport);
    }

    private void ReturnTokenToPool()
    {
        this.tokenPool.PlaceTokenInPool(aToken: this.onDisplay, aTokenType: this.thisTokenType);
    }

    public Tween MoveTokenToTile()
    {
        return MoveTokenToPosition(destination: this.worldPosition);
    }

    public Tween DestroyToken()
    {
        return onDisplay.GetComponent<Movement>().Shrink();
    }

    //*********************Tile Display code***************************************
    public void DisplayTile()
    {
        this.tileDisplay.GetComponent<SpriteRenderer>().enabled = true;
    }


    //*********************Setting Functions***************************************
    public void SetWorldPosition(Vector3 aPosition)
    {
        this.worldPosition = aPosition;
    }

    public void SetGridPosition(Vector2 aPosition)
    {
        this.gridPosition = aPosition;
    }

    public void SetAllPositions(Vector3 aWorldPosition, Vector2 aGridPosition)
    {
        SetWorldPosition(aWorldPosition);
        SetGridPosition(aGridPosition);
    }

    public void SetTileDisplay(GameObject aTile)
    {
        this.tileDisplay = aTile;
        this.tileDisplay.GetComponent<ClickDetector>().SetGridPosition(this.gridPosition);
    }

    ///Tile's tokenDisplay will point to object passed as arg.
    public void SetTokenDisplay(GameObject aToken)
    {
        this.onDisplay = aToken;
    }

    ///Takes a token from the pool and teleports it to the cell
    public void SetTokenDisplay()
    {
        this.onDisplay = tokenPool.GetTokenFromPool(thisTokenType);
        MoveTokenToPosition(worldPosition, teleport: true);
    }

    public void SetToken(TokenTypes aTokenType, GameObject aToken)
    {
        SetTokenType(aTokenType);
        SetTokenDisplay(aToken);
    }

    ///This will return the token to the pool and set the cell to empty.
    ///Use SetToken() to simply mark the cell as empty without returning token to pool
    public void SetToEmpty()
    {
        ReturnTokenToPool();
        SetTokenType(TokenTypes.empty);
        //Put empty object?
    }

    public void SetTokenType(TokenTypes aTokenType)
    {
        this.thisTokenType = aTokenType;
    }

    public void SetRandomTokenType()
    {
        this.thisTokenType = (TokenTypes)Random.Range(1, 7);
    }

    public void SetTokenPool(TokenPool aPool)
    {
        this.tokenPool = aPool;
    }

    //*********************Getting Functions***************************************
    public TokenTypes GetTokenType()
    {
        return this.thisTokenType;
    }

    public GameObject GetTokenDisplay()
    {
        return this.onDisplay;
    }

    public Vector2 GetGridPosition() //used  for Debug Only
    {
        return this.gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return this.worldPosition;
    }
}
