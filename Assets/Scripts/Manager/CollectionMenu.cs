using System.Collections;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Xml.Serialization;

public class CollectionMenu : MonoBehaviour
{
    [SerializeField] private GameObject collectionPanel;
    [SerializeField] private GameObject collectionContainer;
    [SerializeField] private SimpleScrollSnap simpleScrollSnap;

    [Space(50)]
    [SerializeField] private Transform scrollSnapObj;
    [SerializeField] private Transform nextButton;
    [SerializeField] private Transform previousButton;
    [SerializeField] private Transform backButton;
    [SerializeField] private Transform pagination;

    [Space(50)]
    [SerializeField] private Button[] collectionButtons;
    [SerializeField] private GameObject[] collectionObjects;
    [SerializeField] private TextMeshProUGUI[] collectionTitles;
    [SerializeField] private TextMeshProUGUI[] collectionDescriptions;

    [Space(50)]
    [Header("Audio")]
    [SerializeField] private AudioClip[] collectionAudioClip;

    private float nextButtonTargetX;
    private float previousButtonTargetX;
    private float paginationTargetY;

    private void Start()
    {
        collectionPanel.SetActive(false);
        collectionContainer.SetActive(true);

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

                // set active the blocking panel and locked icon, be sure the blocking panel is in last children
                collectionObjects[i].transform.GetChild(collectionObjects[i].transform.childCount - 1).gameObject.SetActive(true);
                collectionTitles[i].text = "???"; // set the title to unknown
                // collectionTitles[i].alignment = TextAlignmentOptions.Left;
                collectionDescriptions[i].text = "???"; // set the description to unknown
            }
            else
            {
                collectionButtons[i].interactable = true;
                collectionButtons[i].transform.GetChild(0).gameObject.SetActive(false);

                // nonactive the blocking panel and locked icon
                collectionObjects[i].transform.GetChild(collectionObjects[i].transform.childCount - 1).gameObject.SetActive(false);

                // collectionTitles[i].alignment = TextAlignmentOptions.Center;

                switch (i) // set the title and description to its actual 
                {
                    case 0:
                        collectionTitles[i].text = Settings.collectionTitle1;
                        collectionDescriptions[i].text = Settings.collectionDesc1;
                        break;
                    case 1:
                        collectionTitles[i].text = Settings.collectionTitle2;
                        collectionDescriptions[i].text = Settings.collectionDesc2;
                        break;
                    case 2:
                        collectionTitles[i].text = Settings.collectionTitle3;
                        collectionDescriptions[i].text = Settings.collectionDesc3;
                        break;
                    case 3:
                        collectionTitles[i].text = Settings.collectionTitle4;
                        collectionDescriptions[i].text = Settings.collectionDesc4;
                        break;
                    case 4:
                        collectionTitles[i].text = Settings.collectionTitle5;
                        collectionDescriptions[i].text = Settings.collectionDesc5;
                        break;
                    case 5:
                        collectionTitles[i].text = Settings.collectionTitle6;
                        collectionDescriptions[i].text = Settings.collectionDesc6;
                        break;
                    case 6:
                        collectionTitles[i].text = Settings.collectionTitle7;
                        collectionDescriptions[i].text = Settings.collectionDesc7;
                        break;
                    default:
                        collectionTitles[i].text = "???";
                        collectionDescriptions[i].text = "?????";
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
        backButton.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        StartCoroutine(CollectionPopAnim());
    }

    public void ShowCollection(int index)
    {
        collectionPanel.SetActive(true);
        collectionContainer.SetActive(false);

        simpleScrollSnap.CustomGoToPanel(index);
        simpleScrollSnap.OnPanelCentered.Invoke(simpleScrollSnap.CenteredPanel, simpleScrollSnap.SelectedPanel);

        ShowCollectionAnim();
    }

    public void Back()
    {
        if (collectionPanel.activeSelf)
        {
            StartCoroutine(CloseCollectionAnim());

            AudioManager.Instance.StopSFX();
        }
        else
        {
            DOTween.KillAll();

            SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
        }
    }

    public void PlaySound()
    {
        int unlockedCollection = GameManager.instance.LoadUnlockedLevel() - 1;

        AudioManager.Instance.StopSFX();

        if (simpleScrollSnap.CenteredPanel < unlockedCollection)
        {
            switch (simpleScrollSnap.CenteredPanel)
            {
                case 0:
                    AudioManager.Instance.PlaySFX(collectionAudioClip[0]);
                    break;
                case 1:
                    AudioManager.Instance.PlaySFX(collectionAudioClip[1]);
                    break;
                case 2:
                    AudioManager.Instance.PlaySFX(collectionAudioClip[2]);
                    break;
                case 3:
                    AudioManager.Instance.PlaySFX(collectionAudioClip[3]);
                    break;
                case 4:
                    AudioManager.Instance.PlaySFX(collectionAudioClip[4]);
                    break;
                case 5:
                    AudioManager.Instance.PlaySFX(collectionAudioClip[5]);
                    break;
                case 6:
                    AudioManager.Instance.PlaySFX(collectionAudioClip[6]);
                    break;
                default:
                    break;
            }
        }
    }
}
