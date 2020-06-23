using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrows : MonoBehaviour
{
    [SerializeField] private SOEffectSharedData effectSharedData = null;
    [SerializeField] private Vector3 destination = Vector3.zero;
    private Vector3 initialLocalPos = Vector3.zero;
    private float animationTime = 0f;

    public Tween ArrowMove()
    {
        return this.transform.DOLocalMove(destination, this.animationTime).SetEase(Ease.Linear);
    }

    public void ResetToDefaultSettings()
    {
        this.transform.localPosition = this.initialLocalPos;  
    }

    private void Awake()
    {
        this.initialLocalPos = this.transform.localPosition;
        this.animationTime = this.effectSharedData.GetAnimationTime();
    }
}
