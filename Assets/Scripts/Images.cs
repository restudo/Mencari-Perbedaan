using DG.Tweening;
using UnityEngine;

public class Images : MonoBehaviour
{
    [SerializeField] private Pooler pool;
    [SerializeField] private Transform imageTransform;
    [SerializeField] private AudioClip negativeSfx;
    // [SerializeField] private Images pair;
    private Vector3 worldPosition;

    private void OnEnable()
    {
        EventHandler.DifferenceClickedFeedback += DiffecenceClickedFeedback;
    }

    private void OnDisable()
    {
        EventHandler.DifferenceClickedFeedback -= DiffecenceClickedFeedback;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.isGameActive && GameManager.Instance.isThoucedActive)
        {
            EventHandler.CallDestroyHintEvent();
            EventHandler.CallDecreaseHealthEvent();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            GameObject g = pool.GetObject();
            g.transform.parent = transform;
            g.transform.position = worldPosition;

            imageTransform.DOShakeRotation(0.1f, new Vector3(0, 0, 10), 10, 90, true, ShakeRandomnessMode.Full);
        }
    }

    private void DiffecenceClickedFeedback(string side)
    {
        Vector3 targetScale = new Vector3(imageTransform.transform.localScale.x - 0.05f, imageTransform.transform.localScale.y - 0.05f, imageTransform.transform.localScale.z - 0.05f);

        if (imageTransform.CompareTag(side))
        {
            imageTransform.transform.DOScale(targetScale, 0.045f).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo);
        }
    }
}
