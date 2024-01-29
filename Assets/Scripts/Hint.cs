using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hint : MonoBehaviour
{
    [SerializeField] private Button hintButton;
    [SerializeField] private GameObject hintObject;
    [SerializeField] private float hintTime;

    private bool isEnable;
    private GameObject instantiatedHint;

    private void Start()
    {
        isEnable = true;
    }

    private IEnumerator ShowHint()
    {
        hintButton.interactable = false;
        isEnable = false;

        GameObject[] pointSpots = GameObject.FindGameObjectsWithTag("Checkpoint");

        int maxAttempts = 50;
        int remainingAttempts = maxAttempts;
        int randomIndex = Random.Range(0, pointSpots.Length);
        while (remainingAttempts > 0 && pointSpots[randomIndex].GetComponent<SpriteRenderer>().enabled)
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
        instantiatedHint.transform.DOScale(originScale, 0.5f).SetEase(Ease.OutExpo);

        yield return new WaitForSeconds(hintTime);

        Destroy(instantiatedHint);

        isEnable = true;
        hintButton.interactable = true;
    }

    public void ShowHintButton()
    {
        if (isEnable && GameManager.instance.isGameActive && GameManager.instance.isThoucedActive)
        {
            StartCoroutine(ShowHint());
        }
    }
}
