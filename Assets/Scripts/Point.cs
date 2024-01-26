using UnityEngine;
using DG.Tweening;

public class Point : MonoBehaviour
{
    [SerializeField] private Point pair;

    [SerializeField] private SpriteRenderer spRend;
    [SerializeField] private GameObject checkRend;

    [SerializeField] private int loopTime = 2;

    [Space(20)]
    [Header("Random Image")]
    [SerializeField] private Sprite[] imageVariant;
    private Collider2D col;
    private bool isClicked;

    private void Start()
    {
        spRend.sprite = imageVariant[Random.Range(0, imageVariant.Length)];

        do { pair.spRend.sprite = imageVariant[Random.Range(0, imageVariant.Length)]; }
        while (pair.spRend.sprite == spRend.sprite);

        col = GetComponent<Collider2D>();
        isClicked = false;

        checkRend.SetActive(false);
        pair.checkRend.SetActive(false);

        //set the position to Camera nearClipPlane, so its always on top 
        transform.position = new Vector3(transform.position.x, transform.position.y, -Camera.main.nearClipPlane);
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.isGameActive && GameManager.instance.isThoucedActive)
        {
            if (isClicked)
            {
                return;
            }
            else
            {
                isClicked = true;

                EventHandler.CallDifferenceClickedEvent();

                spRend.transform.DOScale(1.1f, 0.3f).SetLoops(loopTime * 2, LoopType.Yoyo);
                pair.spRend.transform.DOScale(1.1f, 0.3f).SetLoops(loopTime * 2, LoopType.Yoyo).OnComplete(() =>
                {
                    checkRend.SetActive(true);
                    pair.checkRend.SetActive(true);
                });
            }
        }
    }
}
