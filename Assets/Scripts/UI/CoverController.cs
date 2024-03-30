using UnityEngine;
using Spine.Unity;
using System.Collections;

public class CoverController : MonoBehaviour
{
    private SkeletonAnimation skeletonAnim;
    private Spine.AnimationState animationState;

    private float duration;

    private void Awake()
    {
        skeletonAnim = GetComponent<SkeletonAnimation>();
    }

    private void OnEnable()
    {
        animationState = skeletonAnim.AnimationState;

        var startAnimate = skeletonAnim.Skeleton.Data.FindAnimation("start");
        duration = startAnimate.Duration;

        StartCoroutine(PlayAnim());
    }

    private IEnumerator PlayAnim()
    {
        animationState.SetAnimation(0, "start", false);

        yield return new WaitForSeconds(duration);

        animationState.SetAnimation(0, "loop", true);
    }
}
