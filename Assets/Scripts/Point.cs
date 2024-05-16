using UnityEngine;
using DG.Tweening;

public class Point : MonoBehaviour
{
    public Point pair;
    public bool isDuplicate { get; set; }

    [SerializeField] private SpriteRenderer spRend;
    [SerializeField] private GameObject checkRend;
    [SerializeField] private GameObject circCheckRend;

    [SerializeField] private int loopTime = 2;
    [Tooltip("Boink Feedback")][SerializeField] private float scaleIncrement = 0.07f;

    [Space(20)]
    [Header("Random Image")]
    [SerializeField] private Sprite[] imageVariant;
    private Collider2D col;
    private Collider2D pairCol;
    public bool isClicked { get; private set; }

    private void Start()
    {
        col = GetComponent<Collider2D>();
        pairCol = pair.GetComponent<Collider2D>();

        spRend.sprite = imageVariant[Random.Range(0, imageVariant.Length)];

        if (!isDuplicate)
        {
            do { pair.spRend.sprite = imageVariant[Random.Range(0, imageVariant.Length)]; }
            while (pair.spRend.sprite == spRend.sprite);
        }
        else
        {
            pair.spRend.sprite = spRend.sprite;
            col.enabled = false;
            pairCol.enabled = false;
        }

        isClicked = false;

        checkRend.SetActive(false);
        pair.checkRend.SetActive(false);
        circCheckRend.SetActive(false);
        pair.circCheckRend.SetActive(false);

        //set the position to Camera nearClipPlane, so its always on top 
        transform.position = new Vector3(transform.position.x, transform.position.y, -Camera.main.nearClipPlane);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.isGameActive && GameManager.Instance.isThoucedActive)
        {
            if (isClicked && pair.isClicked)
            {
                Debug.Log("returned " + gameObject.name);
                return;
            }
            else
            {
                isClicked = true;
                pair.isClicked = true;

                EventHandler.CallDifferenceClickedEvent();
                EventHandler.CallDifferenceClickedFeedbackEvent(transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName); // send the layer name to execute the feedback method

                Vector3 targetScale = new Vector3(spRend.transform.localScale.x + scaleIncrement, spRend.transform.localScale.y + scaleIncrement, spRend.transform.localScale.z + scaleIncrement);
                spRend.transform.DOScale(targetScale, 0.3f).SetLoops(loopTime * 2, LoopType.Yoyo);
                pair.spRend.transform.DOScale(targetScale, 0.3f).SetLoops(loopTime * 2, LoopType.Yoyo).OnComplete(() =>
                {
                    checkRend.SetActive(true);
                    pair.checkRend.SetActive(true);
                    circCheckRend.SetActive(true);
                    pair.circCheckRend.SetActive(true);
                });
            }

            EventHandler.CallDestroyHintEvent();
        }
    }
}
