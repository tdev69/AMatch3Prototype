using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOrbEffect : MonoBehaviour
{
    [SerializeField] private SOEffectSharedData effectSharedData = null;

    private Vector3 originalPosition = Vector3.zero;

    ///aNumber should be the number of cells the laser will target
    public Sequence Breathing(int aNumber)
    {
        Sequence breath = DOTween.Sequence();
        breath.Join(this.transform.DOScale(this.effectSharedData.GetSizeTarget(0), this.effectSharedData.GetAnimationTime(0)));
        breath.Append(this.transform.DOScale(this.effectSharedData.GetSizeTarget(1), this.effectSharedData.GetAnimationTime(1)).SetLoops(aNumber));
        
        return breath;
    }

    public void ResetOrb()
    {
        this.transform.position = this.originalPosition;
        this.transform.localScale = Vector3.zero;
    }

    private void Awake()
    {
        this.originalPosition = this.transform.position; 
    }

    private void Start()
    {
        //StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(4);
        Breathing(40);
    }
}
