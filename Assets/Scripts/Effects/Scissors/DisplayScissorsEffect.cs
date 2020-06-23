using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScissorsEffect : MonoBehaviour
{
    private ScissorsArmsBehaviour[] scissorsArmsBehaviours = null;
    private GrowCenter growCenter = null;
    private Vector3 initialRotation = Vector3.zero;

    public void DisplayEffect(Vector3 aPosition, Dictionary<SideTypes, int> aDictionary)
    {
        EventManager.onEffectDisplayEnd += Reset;
        this.transform.position = aPosition;
        SetOrientation(aDictionary);
        CreateAnimationSequence();
    }

    private void CreateAnimationSequence()
    {
        Sequence scissorsEffect = DOTween.Sequence().OnComplete(EventManager.OnEffectDisplayEndSignal);
        scissorsEffect.Join(this.growCenter.Grow());
        
        foreach(ScissorsArmsBehaviour sab in this.scissorsArmsBehaviours)
        {
            scissorsEffect.Join(sab.ArmsAnimation());
        }
    }

    private void SetOrientation(Dictionary<SideTypes, int> aDictionary)
    {
        Vector3 aRotation = Vector3.zero;

        if (aDictionary[SideTypes.up] > 0 && aDictionary[SideTypes.left] > 0)
        {
            aRotation.z += 90f;
        }

        if (aDictionary[SideTypes.down] > 0 && aDictionary[SideTypes.left] > 0)
        {
            aRotation.z += 180f;
        }

        if (aDictionary[SideTypes.down] > 0 && aDictionary[SideTypes.right] > 0)
        {
            aRotation.z += 270f;
        }
        this.transform.rotation = Quaternion.Euler(aRotation);
    }

    private void Reset()
    {
        this.growCenter.Reset();
        this.transform.rotation = Quaternion.Euler(this.initialRotation);

        foreach(ScissorsArmsBehaviour sab in this.scissorsArmsBehaviours)
        {
            sab.Reset();
        }
    }

    private void Awake()
    {
        this.initialRotation = this.transform.rotation.eulerAngles;
        this.growCenter = GetComponentInChildren<GrowCenter>();
        this.scissorsArmsBehaviours = GetComponentsInChildren<ScissorsArmsBehaviour>();
    }
}
