using System.Collections;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectionMenu : MonoBehaviour
{
    [SerializeField] private GameObject collectionPanel;
    [SerializeField] private GameObject collectionContainer;
    [SerializeField] private SimpleScrollSnap simpleScrollSnap;
    [SerializeField] private TextMeshProUGUI title;

    [Space(50)]
    [SerializeField] private Transform scrollSnapObj;
    [SerializeField] private Transform nextButton;
    [SerializeField] private Transform previousButton;
    [SerializeField] private Transform backButton;
    [SerializeField] private Transform pagination;

    [Space(50)]
    [SerializeField] private Button[] collectionButtons;
    [SerializeField] private GameObject[] collectionObjects;
    [SerializeField] private TextMeshProUGUI[] collectionDescriptions;

    private const string collection1 = "Chichen Itza, Meksiko";
    private const string collection2 = "Colosseum Roma, Italia";
    private const string collection3 = "Petra, Yordania";
    private const string collection4 = "Taj Mahal, India";
    private const string collection5 = "Machu Picchu, Peru";
    private const string collection6 = "Patung Christ the Redeemer, Brazil";
    private const string collection7 = "Tembok Besar, China";

    private float nextButtonTargetX;
    private float previousButtonTargetX;
    private float paginationTargetY;

    private void Start()
    {
        collectionPanel.SetActive(false);
        collectionContainer.SetActive(true);
        title.gameObject.SetActive(true);

        nextButtonTargetX = nextButton.localPosition.x;
        previousButtonTargetX = previousButton.localPosition.x;
        paginationTargetY = pagination.localPosition.y;

        LoadCollection();
    }

    private void LoadCollection()
    {
        int unlockedCollection = GameManager.instance.LoadUnlockedLevel() - 1;
        for (int i = 0; i < collectionButtons.Length; i++)
        {
            if (i + 1 > unlockedCollection)
            {
                collectionButtons[i].interactable = false;
                collectionButtons[i].transform.GetChild(0).gameObject.SetActive(true); // set active the locked icon

                // set active the blocking panel and locked icon
                collectionObjects[i].transform.GetChild(collectionObjects[i].transform.childCount - 1).gameObject.SetActive(true);
                collectionDescriptions[i].text = "???"; // set the description to unknown
            }
            else
            {
                collectionButtons[i].interactable = true;
                collectionButtons[i].transform.GetChild(0).gameObject.SetActive(false);

                // nonactive the blocking panel and locked icon
                collectionObjects[i].transform.GetChild(collectionObjects[i].transform.childCount - 1).gameObject.SetActive(false);

                switch (i) // set the description to actual description
                {
                    case 0:
                        collectionDescriptions[i].text = collection1;
                        break;
                    case 1:
                        collectionDescriptions[i].text = collection2;
                        break;
                    case 2:
                        collectionDescriptions[i].text = collection3;
                        break;
                    case 3:
                        collectionDescriptions[i].text = collection4;
                        break;
                    case 4:
                        collectionDescriptions[i].text = collection5;
                        break;
                    case 5:
                        collectionDescriptions[i].text = collection6;
                        break;
                    case 6:
                        collectionDescriptions[i].text = collection7;
                        break;
                    default:
                        collectionDescriptions[i].text = "???";
                        break;
                }
            }
        }

        StartCoroutine(CollectionPopAnim());
    }

    private IEnumerator CollectionPopAnim()
    {
        foreach (Button colButton in collectionButtons)
        {
            colButton.transform.localScale = Vector3.zero;
        }

        backButton.localScale = Vector3.zero;
        backButton.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        foreach (Button colButton in collectionButtons)
        {
            colButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void ShowCollectionAnim()
    {
        scrollSnapObj.localScale = Vector3.zero;
        backButton.localScale = Vector3.zero;

        nextButton.localPosition = Vector3.zero;
        nextButton.localScale = Vector3.zero;
        previousButton.localPosition = Vector3.zero;
        previousButton.localScale = Vector3.zero;
        pagination.localPosition = Vector3.zero;
        pagination.localScale = Vector3.zero;

        scrollSnapObj.DOScale(1f, 0.3f).SetEase(Ease.OutCubic);
        backButton.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        nextButton.DOLocalMoveX(nextButtonTargetX, .5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        nextButton.DOScale(1, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOLocalMoveX(previousButtonTargetX, .5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOScale(1, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        pagination.DOLocalMoveY(paginationTargetY, .5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        pagination.DOScale(1, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
    }

    private IEnumerator CloseCollectionAnim()
    {
        scrollSnapObj.DOScale(0f, 0.3f).SetEase(Ease.OutExpo);
        backButton.DOScale(0f, 0.3f).SetEase(Ease.OutExpo);

        nextButton.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        nextButton.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        pagination.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        pagination.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);

        yield return new WaitForSeconds(0.2f);

        collectionPanel.SetActive(false);
        collectionContainer.SetActive(true);
        title.gameObject.SetActive(true);

        backButton.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        StartCoroutine(CollectionPopAnim());
    }

    public void ShowCollection(int index)
    {
        collectionPanel.SetActive(true);
        collectionContainer.SetActive(false);
        title.gameObject.SetActive(false);

        simpleScrollSnap.CustomGoToPanel(index);

        ShowCollectionAnim();
    }

    public void Back()
    {
        if (collectionPanel.activeSelf)
        {
            StartCoroutine(CloseCollectionAnim());
        }
        else
        {
            DOTween.KillAll();

            SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}
