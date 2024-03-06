using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hint : MonoBehaviour
{
    [SerializeField] private Button hintButton;
    [SerializeField] private GameObject hintObject;
    // [SerializeField] private float hintTime;
    [SerializeField] private float hintLimit;

    private bool isEnable;
    private GameObject instantiatedHint;

    private void OnEnable()
    {
        EventHandler.DestroyHint += DestroyHint;
    }

    private void OnDisable()
    {
        EventHandler.DestroyHint -= DestroyHint;
    }

    private void Start()
    {
        isEnable = true;
    }

    private void ShowHint()
    {
        if (!GameManager.Instance.isThoucedActive)
        {
            return;
        }

        hintButton.interactable = false;
        isEnable = false;

        GameObject[] pointSpots = GameObject.FindGameObjectsWithTag("DifferentPoint");

        int maxAttempts = 50;
        int remainingAttempts = maxAttempts;
        int randomIndex = Random.Range(0, pointSpots.Length);
        while ((remainingAttempts > 0 && !pointSpots[randomIndex].GetComponent<Collider2D>().enabled) ||
               (remainingAttempts > 0 && pointSpots[randomIndex].GetComponent<Collider2D>().enabled &&
                    pointSpots[randomIndex].GetComponent<Point>().isClicked))
        {
            randomIndex = Random.Range(0, pointSpots.Length);
            remainingAttempts--;
        }

        Vector3 position = pointSpots[randomIndex].transform.position;
        Transform parent = pointSpots[randomIndex].transform;
        instantiatedHint = Instantiate(hintObject, position, Quaternion.identity, parent);

        // Play Animation fade or something
        Vector3 originScale = instantiatedHint.transform.localScale;
        float newScalex = instantiatedHint.transform.localScale.x + 2f;
        float newScaley = instantiatedHint.transform.localScale.y + 2f;
        float newScalez = instantiatedHint.transform.localScale.z + 2f;
        instantiatedHint.transform.localScale = new Vector3(newScalex, newScaley, newScalez);
        instantiatedHint.transform.DOScale(originScale, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            instantiatedHint.transform.DOScale(originScale + new Vector3(0.2f, 0.2f, 0.2f), 0.5f).SetEase(Ease.InExpo).SetLoops(-1, LoopType.Yoyo);
        });

        hintButton.interactable = false;
    }

    private void DestroyHint()
    {
        if (instantiatedHint != null)
        {
            DOTween.Kill(instantiatedHint.transform);

            Destroy(instantiatedHint);
            
            isEnable = true;

            if (hintLimit > 0)
            {
                hintButton.interactable = true;
            }
            else
            {
                hintButton.interactable = false;
            }
        }
    }

    public void ShowHintButton()
    {
        if (isEnable && GameManager.Instance.isGameActive && GameManager.Instance.isThoucedActive && hintLimit > 0)
        {
            ShowHint();
            hintLimit--;
        }
    }
}
