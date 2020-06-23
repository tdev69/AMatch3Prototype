using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovesDisplay : MonoBehaviour
{
    [SerializeField] SOInfoContainer infoContainer = null;

    private Text movesDisplayed = null;
    private int currentRemainingMoves = 0;

    private void UpdateMovesDisplayed()
    {
        if (currentRemainingMoves != this.infoContainer.GetMovesRemaining())
        {
            currentRemainingMoves = this.infoContainer.GetMovesRemaining();

            if (currentRemainingMoves < 0)
            {
                currentRemainingMoves = 0;
            }

            SetMovedDisplayedText();
        }
    }

    private void SetMovedDisplayedText()
    {
        this.movesDisplayed.text = currentRemainingMoves.ToString();
    }


    private void Awake()
    {
        this.currentRemainingMoves = this.infoContainer.GetMovesRemaining();
        this.movesDisplayed = GetComponent<Text>();
        SetMovedDisplayedText();
    }

    private void Update()
    {
        UpdateMovesDisplayed();
    }
}
