using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    private Grow[] growSizes = null;

    public void GrowHighlights(Vector3 aPosition)
    {
        this.transform.position = aPosition;

        foreach(Grow g in this.growSizes)
        {
            g.GrowAnimation();
        }
    }

    public void ResetHighlight()
    {
        foreach (Grow g in this.growSizes)
        {
            g.Reset();
        }
    }

    private void Awake()
    {
        this.growSizes = GetComponentsInChildren<Grow>();
    }
}
