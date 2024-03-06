using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] private Image countdownDisplay;
    [SerializeField] private Image goDisplay;
    [SerializeField] private Sprite[] countdownSprite;
    [SerializeField] private Sprite goSprite;

    private Vector3 originScale;

    void Start()
    {
        countdownDisplay.gameObject.SetActive(false);
        goDisplay.gameObject.SetActive(false);

        originScale = countdownDisplay.transform.parent.localScale;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        countdownDisplay.gameObject.SetActive(true);

        for (int i = 0; i < countdownSprite.Length; i++)
        {
            countdownDisplay.transform.parent.localScale = Vector3.zero;
            countdownDisplay.sprite = countdownSprite[i];
            countdownDisplay.transform.parent.DOScale(originScale, 0.2f).SetEase(Ease.OutExpo);

            yield return new WaitForSeconds(1);
        }

        countdownDisplay.gameObject.SetActive(false);
        goDisplay.gameObject.SetActive(true);

        goDisplay.transform.parent.localScale = Vector3.zero;
        goDisplay.sprite = goSprite;
        goDisplay.transform.parent.DOScale(originScale, 0.2f).SetEase(Ease.OutExpo);

        EventHandler.CallImagesEntryEvent();

        GameManager.Instance.isGameActive = true;
        GameManager.Instance.isThoucedActive = true;

        yield return new WaitForSeconds(1f);

        countdownDisplay.transform.parent.gameObject.SetActive(false);
    }
}
