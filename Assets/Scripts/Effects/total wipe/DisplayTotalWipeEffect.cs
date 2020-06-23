using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTotalWipeEffect : MonoBehaviour
{
    [SerializeField] float laserBeamDelays = 0.2f;
    [SerializeField] SOEffectSharedData sharedData = null;
    private CenterOrbEffect centerOrbEffect = null;
    private List<LaserBeams> laserBeams = new List<LaserBeams>();

    public void totalWipe(List<Vector3> targetPositions)
    {
        SetPosition(targetPositions[0]);
        EventManager.OnLaserShotHitSignal(); //shrinks the first token of the list, which is the one used to set the position.
        targetPositions.RemoveAt(0);
        this.centerOrbEffect.Breathing(targetPositions.Count);
        ShootLasers(targetPositions);
    }

    private void ShootLasers(List<Vector3> targetPositions)
    {
        float timeMultiplier = 1;

        foreach(Vector3 target in targetPositions)
        {
            float delay = this.laserBeamDelays * timeMultiplier;
            StartCoroutine(LaserDelay(delay, target));
            timeMultiplier += 1;
        }
    }

    private IEnumerator LaserDelay(float aDelayInSeconds, Vector3 aTarget)
    {
        yield return new WaitForSeconds(aDelayInSeconds);
        laserBeams[0].LaserShot(aTarget);
        laserBeams.RemoveAt(0);
    }

    private void SetPosition(Vector3 aPosition)
    {
        this.transform.position = aPosition;
    }

    public void AddBeamToList(LaserBeams aLaserBeam)
    {
        laserBeams.Add(aLaserBeam);
    }

    private void Awake()
    {
        this.centerOrbEffect = GetComponentInChildren<CenterOrbEffect>();
        this.laserBeams.AddRange(GetComponentsInChildren<LaserBeams>());
        this.laserBeamDelays = this.sharedData.GetAnimationTime(0);
    }
}
