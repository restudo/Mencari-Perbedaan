using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hint : MonoBehaviour
{
    [SerializeField] private Button hintButton;
    [SerializeField] private Material grayscaleMat;
    [SerializeField] private GameObject hintObject;
    [SerializeField] private Transform hintInitTransform;
    // [SerializeField] private float hintTime;
    [SerializeField] private float hintLimit;
    [SerializeField] private float animScaleIncrement;
    public Vector3 initScaleOdd { get; private set; }
    public Vector3 initScaleEven { get; private set; }

    private bool isEnable;
    private GameObject root;
    private GameObject instantiatedHint;
    private GameObject instantiatedHintSibling;
    private Image hintButtonImage;
    private SpriteRenderer instantiatedHintRend;

    private void OnEnable()
    {
        EventHandler.DestroyHint += DestroyHint;
        EventHandler.HintAnim += HintAnim;
    }

    private void OnDisable()
    {
        EventHandler.DestroyHint -= DestroyHint;
        EventHandler.HintAnim -= HintAnim;
    }

    private void Start()
    {
        hintButtonImage = hintButton.GetComponent<Image>();

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

        GameObject hintTransform = pointSpots[randomIndex];
        foreach (Transform child in hintTransform.transform)
        {
            if (child.tag == "Checkpoint")
            {
                Vector3 position = child.transform.position;
                Transform parent = hintTransform.transform;

                instantiatedHint = Instantiate(hintObject, hintInitTransform.transform.position, Quaternion.identity, parent);

                initScaleEven = new Vector3(instantiatedHint.transform.localScale.x, instantiatedHint.transform.localScale.y, instantiatedHint.transform.localScale.z);
                initScaleOdd = new Vector3(-instantiatedHint.transform.localScale.x, instantiatedHint.transform.localScale.y, instantiatedHint.transform.localScale.z);

                // get the sibling sprite sortin layer
                instantiatedHintSibling = instantiatedHint.transform.parent.GetChild(0).gameObject;
                SpriteRenderer siblingRend = instantiatedHintSibling.GetComponent<SpriteRenderer>();

                if (siblingRend != null)
                {
                    // assign the sibling sorting layer to hint sorting layer
                    instantiatedHintRend = instantiatedHint.GetComponent<SpriteRenderer>();
                    instantiatedHintRend.sortingLayerName = siblingRend.sortingLayerName;

                    root = instantiatedHint.transform.root.gameObject;
                    foreach (Transform side in root.transform)
                    {
                        if (side.CompareTag(instantiatedHintRend.sortingLayerName) && side.transform.localScale.x < 0)
                        {
                            Vector3 scalerHint = instantiatedHint.transform.localScale;
                            scalerHint.x *= -1;
                            instantiatedHint.transform.localScale = scalerHint;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Sibling Renderer Not Found");
                }

                hintButton.interactable = false;
                hintButtonImage.material = grayscaleMat;

                // Animation Sequence
                instantiatedHint.transform.DOMove(position, 0.4f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    HintAnim(instantiatedHint.transform.localScale);
                });
            }
        }
    }

    private void HintAnim(Vector3 animScale)
    {
        if (instantiatedHint != null)
        {
            if (animScale.x > 0)
            {
                animScale = new Vector3(animScale.x + animScaleIncrement, animScale.y + animScaleIncrement, animScale.z + animScaleIncrement);
            }
            else if(animScale.x < 0)
            {
                animScale = new Vector3(animScale.x - animScaleIncrement, animScale.y + animScaleIncrement, animScale.z + animScaleIncrement);
            }

            instantiatedHint.transform.DOScale(animScale, 0.5f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
        }
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
                hintButtonImage.material = null;
            }
            else
            {
                hintButton.interactable = false;
                hintButtonImage.material = grayscaleMat;
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
