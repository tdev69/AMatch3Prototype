using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowOuterRing : MonoBehaviour
{
    [SerializeField] SOEffectSharedData effectSharedData = null;
    public Tween Grow()
    {
        return this.transform.DOScale(this.effectSharedData.GetSizeTarget(), this.effectSharedData.GetAnimationTime()).SetEase(Ease.OutExpo);
    }

    public void Reset()
    {
        this.transform.localScale = Vector3.zero;
    }
}
