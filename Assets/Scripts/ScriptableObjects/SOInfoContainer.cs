using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Info Container", menuName = "SO/Info Container")]
public class SOInfoContainer : ScriptableObject
{
    [SerializeField] private int movesAtStart = 25;
    [SerializeField] private int tokenDestoyedForExtraMove = 7;
    [SerializeField] private int pointsPerToken = 20;
    [SerializeField] private int movesRemaining = 25;
    private int currentComboMultiplier = 1; 
    private int currentTokenDestroyed = 0;
    private int currentScore = 0;
    private int highScore = 0;
    private string highscoreKey = "highscore";


    public int GetMovesRemaining()
    {
        return this.movesRemaining;
    }

    public void SetMovesRemaining(int aNumber)
    {
        this.movesRemaining = aNumber;
        EventManager.OnUIMovesUpdateSignal();
    }

    public int GetTokenDestroyForExtraMove()
    {
        return this.tokenDestoyedForExtraMove;
    }

    ///When reaches max determined by tokenDestroyedForExtraMove, is resetted
    public int GetCurrentTokenDestroyed()
    {
        return this.currentTokenDestroyed;
    }

    public int GetPointsPerToken()
    {
        return this.pointsPerToken;
    }

    public void AddToCurrentTokenDestroyed(int aNumber)
    {
        this.currentTokenDestroyed += aNumber;
        UpdateRemainingMoves();
    }

    public int GetHighScore()
    {
        return this.highScore;
    }

    public int GetScore()
    {
        return this.currentScore;
    }

    public void AddToScore(int aNumberToAdd)
    {
        this.currentScore += aNumberToAdd;

        if (this.currentScore > this.highScore)
        {
            this.highScore = this.currentScore;
        }
    }

    ///Do not pass a number below 1, as is used to calculate score
    public void SetComboMultiplier(int aNumber)
    {
        this.currentComboMultiplier = aNumber;
    }

    public int GetComboMultiplier()
    {
        return this.currentComboMultiplier;
    }

    private void UpdateRemainingMoves()
    {
        while (this.currentTokenDestroyed / this.tokenDestoyedForExtraMove >= 1)
        {
            this.movesRemaining += 1;
            this.currentTokenDestroyed -= this.tokenDestoyedForExtraMove;
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(highscoreKey, this.highScore);
        PlayerPrefs.Save();
    }

    public void ResetToStartCondition()
    {
        this.movesRemaining = this.movesAtStart;
        this.currentTokenDestroyed = 0;
        this.currentScore = 0;
        this.currentComboMultiplier = 1;

        if (PlayerPrefs.HasKey(highscoreKey) == true)
        {
            this.highScore = PlayerPrefs.GetInt(highscoreKey);
        }

        else
        {
            this.highScore = 0;
        }
    }

    private void StopListening()
    {
        EventManager.onGameOver -= SaveHighScore;
    }

    private void OnEnable()
    {
        ResetToStartCondition();
        EventManager.onGameOver += SaveHighScore; 
    }

    private void OnDisable()
    {
        StopListening();
        ResetToStartCondition();
    }

    private void OnDestroy()
    {
        StopListening();
    }
}
