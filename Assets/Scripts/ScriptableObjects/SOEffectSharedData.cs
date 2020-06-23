using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effects Shared data", menuName = "SO/Effects/Effects Shared Data")]
public class SOEffectSharedData : ScriptableObject
{
    [SerializeField] private List<float> animationTime = new List<float>() {0.3f};
    [SerializeField] private List<Vector3> sizeTarget = new List<Vector3>() {Vector3.zero};

    public float GetAnimationTime(int index = 0)
    {
        return this.animationTime[index];
    }

    public Vector3 GetSizeTarget(int index = 0)
    {
        return this.sizeTarget[index];
    }
}
