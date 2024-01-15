using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public void ShowHintButton()
    {
        if (isEnable)
        {
            StartCoroutine(ShowHint());
        }
    }

    private IEnumerator ShowHint()
    {
        hintButton.interactable = false;
        isEnable = false;
        // Play Animation fade or something

        GameObject[] pointSpots = GameObject.FindGameObjectsWithTag("PointSpot");

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

        yield return new WaitForSeconds(hintTime);

        Destroy(instantiatedHint);

        isEnable = true;
        hintButton.interactable = true;
    }
}
