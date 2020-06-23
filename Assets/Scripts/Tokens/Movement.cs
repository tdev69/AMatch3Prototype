using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementTime = 2f; 
    [SerializeField] float shrinkTime = 0.2f;


    ///Teleport True means the token will change position instantly
    public Tween MoveToPosition(Vector3 aPosition, bool teleport = false)
    {
        if (this.transform.localScale != new Vector3(1,1,1))
        {
            print("sacale = " + this.transform.localScale);
        }
        SetScaleToNormal();
        Tween moveTween = null;

        if (teleport == false)
        {
            moveTween =this.transform.DOMove(aPosition, movementTime, false);
        }

        else
        {
            this.transform.position = aPosition;
        }

        return moveTween;
    }

    public Tween Shrink()
    {
        return this.transform.DOScale(Vector3.zero, shrinkTime);
    }

    public void ResetToken(Vector3 aPosition)
    {
        this.transform.position = aPosition;
        SetScaleToNormal();
        //this.transform.localScale = new Vector3 (1, 1, 1);
    }

    private void SetScaleToNormal()
    {
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void Start()
    {
        if (this.transform.localScale == Vector3.zero)
        {
            print("scale bug avoided");
            SetScaleToNormal();
        }
    }
}
