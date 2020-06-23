using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIScoreDisplay : MonoBehaviour
{
    [SerializeField] SOInfoContainer infoContainer = null;

    private int currentScore = 0;
    private Text scoreDisplay = null;


    private void UpdateScoreDisplay()
    {
        if (currentScore != this.infoContainer.GetScore())
        {
            currentScore = this.infoContainer.GetScore();
        }

        this.scoreDisplay.text = this.currentScore.ToString();
    }

    private void Awake()
    {
        this.currentScore = 0;
        this.scoreDisplay = GetComponent<Text>();
    }

    void Update()
    {
        UpdateScoreDisplay();
    }
}
