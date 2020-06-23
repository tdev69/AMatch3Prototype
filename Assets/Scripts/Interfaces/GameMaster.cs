using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private GameObject highlight = null;
    [SerializeField] private SOState currentState = null;
    [SerializeField] private SOStatesDictionary aStatesDictionary = null;
    [SerializeField] private SOInfoContainer infoContainer = null;

    private GridManager gridManager = null;
    private MatchManager matchManager = null;
    private TokenMovementManager tokenMovement = null;
    private Highlighter highlighter = null;
    private EffectsPool effectsPool = null;
    private List<GameObject> wipeEffectInUse = new List<GameObject>();
    bool gameOver = false;

    private void Awake()
    {
        this.gridManager = GetComponent<GridManager>();
        this.matchManager = GetComponent<MatchManager>();
        this.tokenMovement = GetComponent<TokenMovementManager>();
        this.highlighter = this.highlight.GetComponent<Highlighter>();
        this.effectsPool = GetComponent<EffectsPool>();
        this.currentState.SetMainComponentsInAllStates(this, this.gridManager, this.matchManager, this.tokenMovement, this.highlighter, this.effectsPool);
        DOTween.Clear();
    }

    ///Set the state passed as arg as the current state and returns it
    public SOState SetCurrentState(SOState aState)
    {
        this.currentState = aState;
        return this.currentState;
    }

    ///Used as debug to enable switching tokens
    public void SetState(SOState aState)
    {
        this.currentState = aState;
    }

    public void MoveLine (ArrowDirection aDirection, int aLineNumber)
    {
        this.highlighter.ResetHighlight();
        this.currentState.ArrowClicked(aDirection, aLineNumber);
    }

    public void CellClicked(Vector2 cellCoordinates)
    {
        this.currentState.CellClicked(cellCoordinates);
    }

    private void SetMovesMinusOne()
    {
        int currentMoves = this.infoContainer.GetMovesRemaining();
        this.infoContainer.SetMovesRemaining(currentMoves - 1);
    }

    private void AddToDestroyedTokens(List<Vector2> aListOfVectors)
    {
        this.infoContainer.AddToCurrentTokenDestroyed(aListOfVectors.Count);
    }
    private void IncreaseCombo()
    {
        int comboMulti = this.infoContainer.GetComboMultiplier();
        this.infoContainer.SetComboMultiplier(comboMulti + 1);
    }

    private void DecreaseCombo()
    {
        int comboMulti = this.infoContainer.GetComboMultiplier();
        this.infoContainer.SetComboMultiplier(comboMulti / 2);
    }

    private void IncreaseScore(List<Vector2> aListOfVectors)
    {
        int basePoints = aListOfVectors.Count * this.infoContainer.GetPointsPerToken();
        int pointsToAdd = basePoints * this.infoContainer.GetComboMultiplier();
        this.infoContainer.AddToScore(pointsToAdd);
    }

    private void StopListening()
    {
        EventManager.onTokenHorizontalMoveEnd -= SetMovesMinusOne;
        EventManager.onTokenHorizontalMoveEnd -= DecreaseCombo;
        EventManager.onTokenInvalidMoveEnd -= SetMovesMinusOne;
        EventManager.onTokenInvalidMoveEnd -= DecreaseCombo;
        EventManager.onTokenValidMoveEnd -= SetMovesMinusOne;
        EventManager.onTokenValidMoveEnd -= IncreaseCombo;
        EventManager.onTokenDestructionEnd -= AddToDestroyedTokens;
        EventManager.onTokenDestructionEnd -= IncreaseScore;
    }

    private void OnDisable()
    {
        StopListening();
    }

    private void OnDestroy()
    {
        StopListening();
    }

    private void Start()
    {
        EventManager.onTokenHorizontalMoveEnd += SetMovesMinusOne;
        EventManager.onTokenHorizontalMoveEnd += DecreaseCombo;
        EventManager.onTokenInvalidMoveEnd += SetMovesMinusOne;
        EventManager.onTokenInvalidMoveEnd += DecreaseCombo;
        EventManager.onTokenValidMoveEnd += SetMovesMinusOne;
        EventManager.onTokenValidMoveEnd += IncreaseCombo;
        EventManager.onTokenDestructionEnd += AddToDestroyedTokens;
        EventManager.onTokenDestructionEnd += IncreaseScore;


        SetCurrentState(this.aStatesDictionary.GetState(StateNames.refill)); //ensures the first state is always refill, necessary to bring all tokens down.
        this.currentState.StateAction();
    }

    public void Update()
    {
        if (this.gameOver == false)
        {
            if (this.currentState == this.aStatesDictionary.GetState(StateNames.wait) && this.infoContainer.GetMovesRemaining() == 0)
            {
                this.gameOver = true;
                EventManager.OnGameOverSignal();
            }
        }
    }
}
