using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] float xTarget = 0f;
    [SerializeField] float animationTime = 0.5f;
    [SerializeField] float startDelay = 0.02f;
    
    private void SlideInPosition()
    {
        this.transform.DOMoveX(xTarget, animationTime).SetEase(Ease.Linear);
    }

    private void Start()
    {
        StartCoroutine(SetDelay());
    }

    IEnumerator SetDelay()
    {
        yield return new WaitForSeconds(startDelay);
        SlideInPosition();
    }
}
