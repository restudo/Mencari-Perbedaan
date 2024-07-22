using System.Collections;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

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

    [Space(50)]
    [Header("Collection")]
    [SerializeField] private Button[] collectionButtons;
    [SerializeField] private GameObject[] lockedBackground;
    [SerializeField] private GameObject[] infos;
    [SerializeField] private GameObject[] lockedInfos;
    [SerializeField] private TextMeshProUGUI[] collectionShortTitles;
    [SerializeField] private TextMeshProUGUI[] collectionTitles;
    [SerializeField] private TextMeshProUGUI[] collectionLocations;
    [SerializeField] private TextMeshProUGUI[] collectionDescriptions;

    [Space(50)]
    [Header("Collection Audio")]
    [SerializeField] private AudioClip[] collectionAudioClip;
    [Header("BGM")]
    [SerializeField] private AudioClip bgmCollectionAudioClip;
    [Header("SFX")]
    [SerializeField] private AudioClip lockedSfx;
    [SerializeField] private AudioClip buttonClickSfx;

    private float nextButtonTargetX;
    private float previousButtonTargetX;

    private void Start()
    {
        foreach (Button colButton in collectionButtons)
        {
            colButton.transform.localScale = Vector3.zero;
        }

        GameManager.Instance.isTouchActive = true;

        collectionPanel.SetActive(false);
        collectionContainer.SetActive(true);

        nextButtonTargetX = nextButton.localPosition.x;
        previousButtonTargetX = previousButton.localPosition.x;

        LoadCollection();

        AudioManager.Instance.PlayMusic(bgmCollectionAudioClip);
    }

    private void LoadCollection()
    {
        int unlockedCollection = GameManager.Instance.LoadUnlockedLevel() - 1;
        for (int i = 0; i < collectionButtons.Length; i++)
        {
            if (i + 1 > unlockedCollection)
            {
                collectionButtons[i].transform.GetChild(0).gameObject.SetActive(true); // blocking graphic
                lockedBackground[i].SetActive(true);
                lockedInfos[i].SetActive(true);
                infos[i].SetActive(false);

                collectionButtons[i].transform.GetChild(collectionButtons[i].transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = "???"; // set title of photo to ???  
                collectionShortTitles[i].text = "???";
                collectionTitles[i].text = "???"; // set the title to unknown
                collectionLocations[i].text = "???";
                collectionDescriptions[i].text = "???"; // set the description to unknown
            }
            else
            {
                collectionButtons[i].transform.GetChild(0).gameObject.SetActive(false); // blocking graphic
                lockedBackground[i].SetActive(false);
                lockedInfos[i].SetActive(false);
                infos[i].SetActive(true);

                TextMeshProUGUI buttonTitleText = collectionButtons[i].transform.GetChild(collectionButtons[i].transform.childCount - 1).GetComponent<TextMeshProUGUI>();

                switch (i) // set the title and description to its actual 
                {
                    case 0:
                        buttonTitleText.text = Settings.collectionTitle1;
                        collectionShortTitles[i].text = Settings.collectionTitle1;
                        collectionTitles[i].text = Settings.collectionTitle1;
                        collectionLocations[i].text = Settings.collectionLocation1;
                        collectionDescriptions[i].text = Settings.collectionDesc1;
                        break;
                    case 1:
                        buttonTitleText.text = Settings.collectionTitle2;
                        collectionShortTitles[i].text = Settings.collectionTitle2;
                        collectionTitles[i].text = Settings.collectionTitle2;
                        collectionLocations[i].text = Settings.collectionLocation2;
                        collectionDescriptions[i].text = Settings.collectionDesc2;
                        break;
                    case 2:
                        buttonTitleText.text = Settings.collectionTitle3Alt;
                        collectionShortTitles[i].text = Settings.collectionTitle3Alt;
                        collectionTitles[i].text = Settings.collectionTitle3;
                        collectionLocations[i].text = Settings.collectionLocation3;
                        collectionDescriptions[i].text = Settings.collectionDesc3;
                        break;
                    case 3:
                        buttonTitleText.text = Settings.collectionTitle4;
                        collectionShortTitles[i].text = Settings.collectionTitle4;
                        collectionTitles[i].text = Settings.collectionTitle4;
                        collectionLocations[i].text = Settings.collectionLocation4;
                        collectionDescriptions[i].text = Settings.collectionDesc4;
                        break;
                    case 4:
                        buttonTitleText.text = Settings.collectionTitle5;
                        collectionShortTitles[i].text = Settings.collectionTitle5;
                        collectionTitles[i].text = Settings.collectionTitle5;
                        collectionLocations[i].text = Settings.collectionLocation5;
                        collectionDescriptions[i].text = Settings.collectionDesc5;
                        break;
                    case 5:
                        buttonTitleText.text = Settings.collectionTitle6;
                        collectionShortTitles[i].text = Settings.collectionTitle6;
                        collectionTitles[i].text = Settings.collectionTitle6;
                        collectionLocations[i].text = Settings.collectionLocation6;
                        collectionDescriptions[i].text = Settings.collectionDesc6;
                        break;
                    case 6:
                        buttonTitleText.text = Settings.collectionTitle7;
                        collectionShortTitles[i].text = Settings.collectionTitle7;
                        collectionTitles[i].text = Settings.collectionTitle7;
                        collectionLocations[i].text = Settings.collectionLocation7;
                        collectionDescriptions[i].text = Settings.collectionDesc7;
                        break;
                    default:
                        buttonTitleText.text = "???";
                        collectionTitles[i].text = "???";
                        collectionLocations[i].text = "???";
                        collectionDescriptions[i].text = "?????";
                        break;
                }
            }
        }

        StartCoroutine(CollectionPopAnim());
    }

    private IEnumerator CollectionPopAnim()
    {
        yield return new WaitForSeconds(0.1f);

        // backButton.localScale = Vector3.zero;
        // backButton.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        foreach (Button colButton in collectionButtons)
        {
            colButton.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.06f);
        }
    }

    private void ShowCollectionAnim()
    {
        scrollSnapObj.localScale = Vector3.zero;
        // backButton.localScale = Vector3.zero;

        nextButton.localPosition = Vector3.zero;
        nextButton.localScale = Vector3.zero;
        previousButton.localPosition = Vector3.zero;
        previousButton.localScale = Vector3.zero;

        scrollSnapObj.DOScale(1f, 0.3f).SetEase(Ease.OutCubic);
        // backButton.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        nextButton.DOLocalMoveX(nextButtonTargetX, .5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        nextButton.DOScale(1, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOLocalMoveX(previousButtonTargetX, .5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOScale(1, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
    }

    private IEnumerator CloseCollectionAnim()
    {
        scrollSnapObj.DOScale(0f, 0.3f).SetEase(Ease.OutExpo);
        // backButton.DOScale(0f, 0.3f).SetEase(Ease.OutExpo);

        nextButton.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        nextButton.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        previousButton.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(0.15f);

        yield return new WaitForSeconds(0.2f);

        collectionPanel.SetActive(false);
        collectionContainer.SetActive(true);
        // backButton.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        StartCoroutine(CollectionPopAnim());
    }

    public void ShowCollection(int index)
    {
        if (!GameManager.Instance.isTouchActive)
        {
            return;
        }

        // if the collectionbutton is locked
        if (collectionButtons[index].transform.GetChild(0).gameObject.activeSelf)
        {
            GameManager.Instance.isTouchActive = false;
            AudioManager.Instance.PlaySFX(lockedSfx);
            Vector3 targetScale = new Vector3(collectionButtons[index].transform.localScale.x + 0.05f, collectionButtons[index].transform.localScale.y + 0.05f, collectionButtons[index].transform.localScale.z + 0.05f);
            collectionButtons[index].transform.DOScale(targetScale, 0.1f).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                GameManager.Instance.isTouchActive = true;
            });
        }
        else
        {
            AudioManager.Instance.PlaySFX(buttonClickSfx);

            collectionPanel.SetActive(true);
            collectionContainer.SetActive(false);

            simpleScrollSnap.CustomGoToPanel(index);
            simpleScrollSnap.OnPanelCentered.Invoke(simpleScrollSnap.CenteredPanel, simpleScrollSnap.SelectedPanel);

            ShowCollectionAnim();
        }
    }

    public void Back()
    {
        if (collectionPanel.activeSelf)
        {
            StartCoroutine(CloseCollectionAnim());

            AudioManager.Instance.StopSFX();

            AudioManager.Instance.PlaySFX(buttonClickSfx);
        }
        else
        {
            AudioManager.Instance.PlaySFX(buttonClickSfx);

            DOTween.KillAll();

            SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
        }
    }

    public void PlaySound()
    {
        if (!GameManager.Instance.isTouchActive)
        {
            return;
        }
        else
        {
            GameManager.Instance.isTouchActive = false;

            Transform currentObj = EventSystem.current.currentSelectedGameObject.transform;

            DOTween.Kill(currentObj.transform);

            if (simpleScrollSnap.CenteredPanel < (GameManager.Instance.LoadUnlockedLevel() - 1))
            {
                Sound();
            }
            else
            {
                AudioManager.Instance.PlaySFX(lockedSfx);
            }

            Vector3 targetScaleAnim = new Vector3(currentObj.transform.localScale.x - 0.05f, currentObj.transform.localScale.y - 0.05f, currentObj.transform.localScale.z - 0.05f);
            currentObj.DOScale(targetScaleAnim, 0.1f).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                GameManager.Instance.isTouchActive = true;
            });
        }
    }

    public void Sound()
    {
        AudioManager.Instance.StopSFX();

        int unlockedCollection = GameManager.Instance.LoadUnlockedLevel() - 1;

        if (simpleScrollSnap.CenteredPanel < unlockedCollection)
        {
            float musicVolumeOrigin = AudioManager.Instance.GetMusicVolume();
            AudioManager.Instance.SetMusicVolume(0.3f);

            int clipIndex = simpleScrollSnap.CenteredPanel;
            AudioClip clipToPlay = collectionAudioClip[clipIndex];

            // Play the SFX and get the AudioSource
            AudioSource audioSource = AudioManager.Instance.PlaySFXAndGetSource(clipToPlay);

            // Delay based on the length of the audio clip
            StartCoroutine(WaitForSFXAndRestoreMusicVolume(audioSource, musicVolumeOrigin));
        }
    }

    private IEnumerator WaitForSFXAndRestoreMusicVolume(AudioSource audioSource, float musicVolumeOrigin)
    {
        // Wait until the audio clip has finished playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Restore music volume to its origin
        AudioManager.Instance.SetMusicVolume(musicVolumeOrigin);
    }

    public void PlayFeedbackAnim()
    {
        if (!GameManager.Instance.isTouchActive)
        {
            return;
        }

        GameManager.Instance.isTouchActive = false;
        Vector3 targetScale = new Vector3(scrollSnapObj.transform.localScale.x + 0.05f, scrollSnapObj.transform.localScale.y + 0.05f, scrollSnapObj.transform.localScale.z + 0.05f);
        scrollSnapObj.transform.DOScale(targetScale, 0.1f).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            GameManager.Instance.isTouchActive = true;
        });
    }

    public void PlaySfxButton()
    {
        AudioManager.Instance.PlaySFX(buttonClickSfx);
    }
}
