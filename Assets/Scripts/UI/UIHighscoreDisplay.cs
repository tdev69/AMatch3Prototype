using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighscoreDisplay : MonoBehaviour
{
    [SerializeField] private SOInfoContainer infoContainer = null;

    private Text highScoreDisplay = null;
    private int currentHighscore = 0;

    private void UpdateHighScoreDisplay()
    {
        if (this.currentHighscore != this.infoContainer.GetHighScore())
        {
            this.currentHighscore = this.infoContainer.GetHighScore();
        }

        this.highScoreDisplay.text = this.currentHighscore.ToString();
    }

    void Update()
    {
        UpdateHighScoreDisplay();
    }

    private void Awake()
    {
        this.highScoreDisplay = GetComponent<Text>();
    }
}
