using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    private Vector2 gridPosition = Vector2.zero;
    private GameMaster gameMaster = null;


    public void SetGridPosition(Vector2 aGridPosition)
    {
        this.gridPosition = aGridPosition;
    }

    private void ClickDetected()
    {
        this.gameMaster.CellClicked(this.gridPosition);
    }

    private void OnMouseDown()
    {
        ClickDetected();
    }

    private void Awake()
    {
        this.gameMaster = GetComponentInParent<GameMaster> ();
    }
}
