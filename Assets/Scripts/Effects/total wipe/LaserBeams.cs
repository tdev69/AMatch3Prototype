using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeams : MonoBehaviour
{
    [SerializeField] private SOEffectSharedData effectSharedData = null;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private Vector3 testPosition = Vector3.zero;

    private DisplayTotalWipeEffect displayTotalWipeEffect = null;
    private Vector3 originalPosition = Vector3.zero;

    public Sequence LaserShot(Vector3 targetPosition)
    {
        originalPosition = this.transform.position;
        targetPosition.z = this.transform.position.z;
        Vector3 direction = targetPosition - originalPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        float originAngle = angle - 180;
        LookAtDirection(angle);

        Sequence shot = DOTween.Sequence().OnComplete(MakeAvailable);
        shot.Join(this.transform.DOScale(this.effectSharedData.GetSizeTarget(2), this.effectSharedData.GetAnimationTime(0)));
        shot.Join(this.transform.DOMove(targetPosition, speed).SetEase(Ease.Linear));
        shot.AppendCallback(() => LookAtDirection(originAngle));
        shot.AppendCallback(EventManager.OnLaserShotHitSignal);
        shot.Append(this.transform.DOScale(Vector3.zero, this.effectSharedData.GetAnimationTime(0)));
        return shot;
    }

    private void LookAtDirection(float anAngle)
    {
        this.transform.rotation = Quaternion.Euler(0f, 0f, anAngle);
    }

    private void Awake()
    { 
        this.displayTotalWipeEffect = GetComponentInParent<DisplayTotalWipeEffect>();
        this.originalPosition = this.transform.position;
    }

    private void MakeAvailable()
    {
        this.displayTotalWipeEffect.AddBeamToList(this);
        this.transform.position = this.originalPosition;
    }

    void Start()
    {

       // StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(4);
        LaserShot(this.testPosition);
    }
}
