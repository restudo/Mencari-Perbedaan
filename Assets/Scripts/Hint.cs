using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Hint : MonoBehaviour
{
    [SerializeField] private Button hintButton;
    [SerializeField] private Material grayscaleMat;
    [SerializeField] private GameObject hintObject;
    [SerializeField] private Transform hintInitTransform;
    // [SerializeField] private float hintTime;
    [SerializeField] private float hintLimit;
    [SerializeField] private float breathAnimDuration = 0.5f;
    [SerializeField] private AudioClip hintMoveSfx;

    // [SerializeField] private float animScaleIncrement = 0.03f;
    // public Vector3 initScaleOdd { get; private set; }
    // public Vector3 initScaleEven { get; private set; }

    private bool isEnable;
    private GameObject root;
    private GameObject instantiatedHint;
    private GameObject instantiatedHintSibling;
    private Image hintButtonImage;
    private SpriteRenderer instantiatedHintRend;
    private SpriteRenderer instantiatedHintCircleRend;
    private LevelManager levelManager;
    private AudioClip buttonSfx;

    private void OnEnable()
    {
        EventHandler.DestroyHint += DestroyHint;
        EventHandler.HintAnim += HintAnim;
        EventHandler.Transparent += Transparent;
    }

    private void OnDisable()
    {
        EventHandler.DestroyHint -= DestroyHint;
        EventHandler.HintAnim -= HintAnim;
        EventHandler.Transparent -= Transparent;
    }

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        buttonSfx = levelManager.ButtonSfx;

        hintButtonImage = hintButton.GetComponent<Image>();

        isEnable = true;
    }

    private void ShowHint()
    {
        if (!GameManager.Instance.isTouchActive)
        {
            return;
        }

        AudioManager.Instance.PlaySFX(buttonSfx);

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
        Transform parentCheckpoint = hintTransform.transform.GetChild(hintTransform.transform.childCount - 1);

        foreach (Transform child in parentCheckpoint)
        {
            if (child.tag == "Checkpoint")
            {
                Vector3 position = child.transform.position;
                Transform parent = hintTransform.transform;

                instantiatedHint = Instantiate(hintObject, hintInitTransform.transform.position, Quaternion.identity, parent);

                // initScaleEven = new Vector3(instantiatedHint.transform.localScale.x, instantiatedHint.transform.localScale.y, instantiatedHint.transform.localScale.z);
                // initScaleOdd = new Vector3(-instantiatedHint.transform.localScale.x, instantiatedHint.transform.localScale.y, instantiatedHint.transform.localScale.z);

                // get the sibling sprite sortin layer
                instantiatedHintSibling = instantiatedHint.transform.parent.GetChild(0).gameObject;
                SpriteRenderer siblingRend = instantiatedHintSibling.GetComponent<SpriteRenderer>();

                if (siblingRend != null)
                {
                    // assign the sibling sorting layer to hint sorting layer
                    instantiatedHintRend = instantiatedHint.GetComponent<SpriteRenderer>();
                    instantiatedHintCircleRend = instantiatedHint.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    instantiatedHintRend.sortingLayerName = siblingRend.sortingLayerName;
                    instantiatedHintCircleRend.sortingLayerName = siblingRend.sortingLayerName;

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

                    if (instantiatedHint.transform.parent.localScale.x < 0)
                    {
                        Vector3 scalerHint = instantiatedHint.transform.localScale;
                        scalerHint.x *= -1;
                        instantiatedHint.transform.localScale = scalerHint;
                    }
                }
                else
                {
                    Debug.LogError("Sibling Renderer Not Found");
                }

                hintButton.interactable = false;
                hintButtonImage.material = grayscaleMat;

                Transparent();

                StartCoroutine(PlaySFX());

                // Animation Sequence
                instantiatedHint.transform.DOMove(position, 0.4f).SetEase(Ease.OutExpo).SetDelay(0.3f).OnComplete(() =>
                {
                    // HintAnim(instantiatedHint.transform.localScale);
                    HintAnim();
                });
            }
        }
    }

    private void Transparent()
    {
        DOTween.Kill(instantiatedHintCircleRend);

        instantiatedHintCircleRend.color = new Color(instantiatedHintCircleRend.color.r, instantiatedHintCircleRend.color.g, instantiatedHintCircleRend.color.b, 0f);
    }

    private IEnumerator PlaySFX()
    {
        yield return new WaitForSeconds(0.3f);

        AudioManager.Instance.PlaySFX(hintMoveSfx);
    }

    private void HintAnim()
    {
        instantiatedHintCircleRend.DOFade(1f, breathAnimDuration).SetEase(Ease.InExpo).SetLoops(-1, LoopType.Yoyo).SetDelay(0.1f);
    }

    // private void HintAnim(Vector3 animScale)
    // {
    //     if (instantiatedHint != null)
    //     {
    //         if (animScale.x > 0)
    //         {
    //             animScale = new Vector3(animScale.x + animScaleIncrement, animScale.y + animScaleIncrement, animScale.z + animScaleIncrement);
    //         }
    //         else if(animScale.x < 0)
    //         {
    //             animScale = new Vector3(animScale.x - animScaleIncrement, animScale.y + animScaleIncrement, animScale.z + animScaleIncrement);
    //         }

    //         instantiatedHint.transform.DOScale(animScale, 0.5f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
    //     }
    // }

    private void DestroyHint()
    {
        if (instantiatedHint != null)
        {
            DOTween.Kill(instantiatedHintCircleRend);

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
        if (isEnable && GameManager.Instance.isGameActive && GameManager.Instance.isTouchActive && hintLimit > 0)
        {
            ShowHint();
            hintLimit--;
        }
    }
}
