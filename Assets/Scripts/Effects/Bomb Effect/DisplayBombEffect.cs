using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBombEffect : MonoBehaviour
{
    [SerializeField] private float endSequenceDelay = 0.05f;
    private GrowCenter growCenter = null;
    private GrowOuterRing[] growOuterRing = null;
    private Vector3 initialPosition = Vector3.zero;

    public void DisplayThisEffect(Vector3 aPosition)
    {
        EventManager.onEffectDisplayEnd += ResetAllComponents;
        GetInPosition(aPosition);
        DisplayEffect();
    }

    private void DisplayEffect()
    {
        Sequence bombSequence = DOTween.Sequence().OnComplete(EventManager.OnEffectDisplayEndSignal);

        bombSequence.Join(this.growCenter.Grow());

        foreach (GrowOuterRing gor in this.growOuterRing)
        {
            bombSequence.Join(gor.Grow());
        }

        bombSequence.AppendInterval(endSequenceDelay);
    }

    private void GetInPosition(Vector3 aPosition)
    {
        this.transform.position = aPosition;
    }

    private void ResetAllComponents()
    {
        this.growCenter.Reset();

        foreach (GrowOuterRing gor in this.growOuterRing)
        {
            gor.Reset();
        }
    }

    private void Awake()
    {
        this.growCenter = GetComponentInChildren<GrowCenter>();
        this.growOuterRing = GetComponentsInChildren<GrowOuterRing>();
        this.initialPosition = this.transform.position;
    }

    private void StopListening()
    {
        EventManager.onEffectDisplayEnd -= ResetAllComponents;
    }

    private void OnDisable()
    {
        StopListening();
    }

    private void OnDestroy()
    {
        StopListening();
    }
}
