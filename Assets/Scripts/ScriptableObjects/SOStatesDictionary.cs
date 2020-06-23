using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "States Dictionary", menuName = "SO/States Dictionary")]
public class SOStatesDictionary : ScriptableObject
{
    [SerializeField] private List<States> theStates = new List<States>();
    private Dictionary<StateNames, SOState> stateDictionary = new Dictionary<StateNames, SOState>();

    private void OnEnable()
    {
        if (this.theStates.Count > 0)
        {
            foreach (States state in theStates)
            {
                this.stateDictionary.Add(state.GetStateNames(), state.GetState());
            }
        }
    }

    public SOState GetState(StateNames aStateName)
    {
        if (this.stateDictionary.ContainsKey(aStateName))
        {
            return this.stateDictionary[aStateName];
        }

        else
        {
            Debug.LogWarning("The key you are looking for (" + aStateName + ") does not exists");
            return null;
        }
    }
}