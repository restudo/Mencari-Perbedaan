using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] private Image countdownDisplay;
    [SerializeField] private Sprite[] countdownSprite;
    [SerializeField] private Sprite goSprite;

    private Vector3 originScale;

    void Start()
    {
        originScale = countdownDisplay.transform.localScale;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        for (int i = 0; i < countdownSprite.Length; i++)
        {
            countdownDisplay.transform.localScale = Vector3.zero;
            countdownDisplay.sprite = countdownSprite[i];
            countdownDisplay.transform.DOScale(originScale, 0.2f).SetEase(Ease.OutExpo);

            yield return new WaitForSeconds(1);
        }

        countdownDisplay.transform.localScale = Vector3.zero;
        countdownDisplay.sprite = goSprite;
        countdownDisplay.transform.DOScale(originScale, 0.2f).SetEase(Ease.OutExpo);

        EventHandler.CallImagesEntryEvent();

        GameManager.Instance.isGameActive = true;
        GameManager.Instance.isThoucedActive = true;

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
    }
}
