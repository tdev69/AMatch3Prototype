using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComboDisplay : MonoBehaviour
{
    [SerializeField] SOInfoContainer infoContainer = null;

    private Text displayCombo = null;
    private int currentComboDisplay = 1; 

    private void UpdateComboDisplay()
    {
        if(currentComboDisplay != this.infoContainer.GetComboMultiplier())
        {
            currentComboDisplay = this.infoContainer.GetComboMultiplier();
        }

        this.displayCombo.text = "x"+ currentComboDisplay.ToString();
    }

    private void Update()
    {
        UpdateComboDisplay();
    }

    private void Awake()
    {
        this.displayCombo = GetComponent<Text>();
    }
}
