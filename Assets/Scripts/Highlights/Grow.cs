using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    [SerializeField] private Vector3 growTarget = Vector3.zero;
    [SerializeField] private Vector3 startScale = Vector3.zero; //The size to which we want to reset the object
    [SerializeField] private float growTime = 0.2f;

    public Tween GrowAnimation()
    {
        return this.transform.DOScale(this.growTarget, this.growTime).SetEase(Ease.Linear);
    }

    public void Reset()
    {
        this.transform.localScale = this.startScale;
    }
}
