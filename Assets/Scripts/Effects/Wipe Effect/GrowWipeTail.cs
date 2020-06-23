using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowWipeTail : MonoBehaviour
{
    [SerializeField] SOEffectSharedData effectData = null;
    [SerializeField] private Vector3 startScale = Vector3.zero; //The size to which we want to reset the object
    private Vector3 sizeTarget = Vector3.zero;
    private float animationTime = 0f;

    public Tween GrowAnimation()
    {
        return this.transform.DOScale(this.sizeTarget, this.animationTime).SetEase(Ease.Linear);
    }

    private void Awake()
    {
        this.sizeTarget = this.effectData.GetSizeTarget();
        this.animationTime = this.effectData.GetAnimationTime();
    }
    
    public void Reset()
    {
        this.transform.localScale = this.startScale;
    }
}
