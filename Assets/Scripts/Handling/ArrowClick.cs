using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowDirection {left, right};
public class ArrowClick : MonoBehaviour
{
    [SerializeField] private ArrowDirection direction = ArrowDirection.left;
    [SerializeField] private GameObject gameMasterObject= null;
    GameMaster gmInterface = null;

    private void OnMouseDown()
    {
        this.gmInterface.MoveLine(this.direction, aLineNumber: 0);
    }

    private void Awake()
    {
        this.gmInterface = this.gameMasterObject.GetComponentInParent<GameMaster>();
    }
}
