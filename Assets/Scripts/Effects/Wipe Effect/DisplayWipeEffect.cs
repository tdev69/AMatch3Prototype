using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWipeEffect : MonoBehaviour
{
    [SerializeField] private Vector3 middleCellCoordinates = Vector3.zero;
    [SerializeField] private SOEffectSharedData sharedData = null;
    private MoveArrows[] moveArrows = null;
    private GrowWipeTail grow = null;
    private Vector3 initialPosition = Vector3.zero;
    private EffectsPool effectsPool = null;
    
    ///arg anAngle should be either 0 for horizontal or 90 for vertical
    private void GetInPosition(Vector3 aPosition, float anAngle)
    {
        EventManager.onEffectDisplayEnd += ResetToWait;
        this.transform.rotation = Quaternion.Euler(0, 0, anAngle);
        this.transform.position = aPosition;
    }

    private void DisplayEffect()
    {
        Sequence wipeSequence = DOTween.Sequence().OnComplete(EventManager.OnEffectDisplayEndSignal);
        Vector3 position = this.transform.position;

        foreach(MoveArrows ma in this.moveArrows)
        {
            wipeSequence.Join(ma.ArrowMove());
        }

        wipeSequence.Join(grow.GrowAnimation());
    }

    private void ResetToWait()
    {
        StopListening();
        this.transform.position = initialPosition;

        foreach(MoveArrows ma in this.moveArrows)
        {
            ma.ResetToDefaultSettings();
        }

        this.grow.Reset();
        MarkSelfAsAvailable();
    }

    private void Recenter()
    {
        float angle = this.transform.rotation.eulerAngles.z;

        if (angle == 0 && this.transform.position.x != this.middleCellCoordinates.x)
        {
            this.transform.DOMoveX(middleCellCoordinates.x, this.sharedData.GetAnimationTime());
        }

        else if (angle == 90 && this.transform.position.y != this.middleCellCoordinates.y)
        {
            this.transform.DOMoveY(middleCellCoordinates.y, this.sharedData.GetAnimationTime());
        }
    }

    ///arg anAngle should be either 0 for horizontal or 90 for vertical
    public void DisplayThisEffect(Vector3 aPosition, float anAngle)
    {
        aPosition.z = this.initialPosition.z;
        GetInPosition(aPosition, anAngle);
        Recenter();
        DisplayEffect();
    }

    private void MarkSelfAsAvailable()
    {
        this.effectsPool.AddToAvailableWipeEffect(this);
    }

    private void StopListening()
    {
        EventManager.onEffectDisplayEnd -= ResetToWait;
    }

    private void OnDestroy()
    {
        StopListening();
    }

    private void OnDisable()
    {
        StopListening();
    }

    private void Awake()
    {
        this.moveArrows = GetComponentsInChildren<MoveArrows>();
        this.grow = GetComponentInChildren<GrowWipeTail>();
        this.initialPosition = this.transform.position;
        this.effectsPool = GetComponentInParent<EffectsPool>();
        MarkSelfAsAvailable();
    }
}
