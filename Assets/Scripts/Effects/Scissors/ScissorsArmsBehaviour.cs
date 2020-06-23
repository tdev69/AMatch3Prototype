using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsArmsBehaviour : MonoBehaviour
{
    [SerializeField] SOEffectSharedData effectSharedData = null;
    [SerializeField] Vector3 rotationEndValue = Vector3.zero;

    private Quaternion originalRotation = new Quaternion (0f, 0f, 0f, 0f);

    public Sequence ArmsAnimation()
    {
        Sequence armsSequence = DOTween.Sequence().SetEase(Ease.Linear);
        armsSequence.Join(this.transform.DOScale(this.effectSharedData.GetSizeTarget(1), this.effectSharedData.GetAnimationTime(0)));
        armsSequence.Append(this.transform.DOLocalRotate(rotationEndValue, this.effectSharedData.GetAnimationTime(1)));
        armsSequence.Join(this.transform.DOScale(this.effectSharedData.GetSizeTarget(0), this.effectSharedData.GetAnimationTime(1)));

        return armsSequence;
    }

    public void Reset()
    {
        this.transform.localScale = new Vector3(1f,0f,1f);
        this.transform.rotation = originalRotation;
    }

    private void Awake()
    {
        this.originalRotation = this.transform.rotation;
    }
}
