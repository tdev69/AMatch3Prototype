using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPool : MonoBehaviour
{
    [SerializeField] private List<DisplayWipeEffect> wipeEffectsAvailable = new List<DisplayWipeEffect>();
    private DisplayBombEffect displayBombEffect = null;
    private DisplayScissorsEffect displayScissorsEffect = null;

    private DisplayTotalWipeEffect displayTotalWipeEffect = null;

    public void AddToAvailableWipeEffect(DisplayWipeEffect aWipeEffect)
    {
        this.wipeEffectsAvailable.Add(aWipeEffect);
    }

    ///arg anAngle should be either 0 for horizontal or 90 for vertical
    public void DisplayWipeEffect(Vector3 aPosition, float anAngle)
    {
        this.wipeEffectsAvailable[0].GetComponent<DisplayWipeEffect>().DisplayThisEffect(aPosition, anAngle);
        this.wipeEffectsAvailable.RemoveAt(0);
    }

    public void DisplayTripleLineWipeEffect(Vector3 aPosition, SideTypes aDirection)
    {
        for (float y = 0; y <= 2; y++)
        {
            if (aDirection == SideTypes.up)
            {
                DisplayWipeEffect(new Vector3(aPosition.x, aPosition.y + y, aPosition.z), 0f);
            }

            else
            {
                DisplayWipeEffect(new Vector3(aPosition.x, aPosition.y - y, aPosition.z), 0f);
            }
        }
    }

    public void DisplayTripleColumnWipeEffect(Vector3 aPosition, SideTypes aDirection)
    {
        for (float x = 0; x <= 2; x++)
        {
            if (aDirection == SideTypes.left)
            {
                DisplayWipeEffect(new Vector3(aPosition.x - x, aPosition.y, aPosition.z), 90f);
            }

            else
            {
                DisplayWipeEffect(new Vector3(aPosition.x + x, aPosition.y, aPosition.z), 90f);
            }
        }
    }

    ///first coordinates in the list will be used to display the heart of totalWipe
    public void DisplayTotalWipeEffect(List<Vector3> targetPositions)
    {
        this.displayTotalWipeEffect.totalWipe(targetPositions);
    }

    public void DisplayBombEffect(Vector3 aPosition)
    {
        this.displayBombEffect.DisplayThisEffect(aPosition);
    }

    public void DisplayScissorsEffect(Vector3 aPosition, Dictionary<SideTypes, int> aDictionary)
    {
        this.displayScissorsEffect.DisplayEffect(aPosition, aDictionary);
    }

    public void Awake()
    {
        this.displayBombEffect = GetComponentInChildren<DisplayBombEffect>();
        this.displayScissorsEffect = GetComponentInChildren<DisplayScissorsEffect>();
        this.displayTotalWipeEffect = GetComponentInChildren<DisplayTotalWipeEffect>();
    }
}
