using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    [SerializeField] private float waitBetweenStartAnim = 0.05f;
    [SerializeField] private GameObject backButton;

    [SerializeField] private GameObject[] selectedVFX;
    [SerializeField] private GameObject[] startButtons;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Animator[] animatorButtons;
    [SerializeField] private Image[] lockedBackground;
    [SerializeField] private GameObject[] lockedIcon;

    [Space(20)]
    [Header("Confirmation Panel")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Image levelImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Sprite[] levelImageRef;

    [Space(50)]
    [Header("BGM")]
    [SerializeField] private AudioClip bgmStageMenuAudioClip;
    [Header("SFX")]
    [SerializeField] private AudioClip[] locationSfx;
    [SerializeField] private AudioClip lockedSfx;
    [SerializeField] private AudioClip buttonClickSfx;

    int unlockedLevel;
    private int level;
    private int previousLevel;
    private bool isFirstTime;
    private Vector3 levelButtonInitScale;
    private Vector3 confirmationPanelScale;

    private void Start()
    {
        GameManager.Instance.isThoucedActive = true;

        isFirstTime = true;

        level = 1;

        foreach (var button in startButtons)
        {
            button.SetActive(false);
        }
        foreach (var vfx in selectedVFX)
        {
            vfx.SetActive(false);
        }

        unlockedLevel = GameManager.Instance.LoadUnlockedLevel();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > unlockedLevel)
            {
                lockedBackground[i].gameObject.SetActive(true); // set locked background to true
                lockedIcon[i].SetActive(true); // set locked icon to true
            }
            else
            {
                lockedBackground[i].gameObject.SetActive(false); // set locked background to false
                lockedIcon[i].SetActive(false); // set locked icon to false
            }

            animatorButtons[i].enabled = false;
        }

        if (GameManager.Instance.canStageButtonAnim && lockedBackground[unlockedLevel - 1] != null)
        {
            GameManager.Instance.isThoucedActive = false;
            lockedIcon[unlockedLevel - 1].SetActive(true);
            lockedBackground[unlockedLevel - 1].gameObject.SetActive(true);
        }

        levelButtonInitScale = levelButtons[0].transform.localScale;
        confirmationPanelScale = confirmationPanel.transform.localScale;

        StartCoroutine(ButtonPopAnim());

        AudioManager.Instance.PlayMusic(bgmStageMenuAudioClip);
    }

    private IEnumerator ButtonPopAnim()
    {
        // backButton.transform.localScale = Vector3.zero;
        // backButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        foreach (Button button in levelButtons)
        {
            button.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].transform.DOScale(levelButtonInitScale, 0.3f).SetEase(Ease.OutBounce);

            if (i == unlockedLevel - 1)
            {
                // REVIEW : select button selected when open stage menu
                if (unlockedLevel <= GameManager.Instance.maxLevel)
                {
                    SelectLevel(unlockedLevel - 1); // set the level button by unlocked level parameter
                }
                else if (unlockedLevel > GameManager.Instance.maxLevel)
                {
                    SelectLevel(GameManager.Instance.maxLevel - 1); // set the level button by unlocked level parameter
                }
            }

            yield return new WaitForSeconds(waitBetweenStartAnim);
        }

        if (GameManager.Instance.canStageButtonAnim && lockedBackground[unlockedLevel - 1] != null)
        {
            // UnlockButtonAnim();

            animatorButtons[unlockedLevel - 1].enabled = true;

            GameManager.Instance.canStageButtonAnim = false;
        }
    }

    private void UnlockButtonAnim()
    {
        // lockedBackground[unlockedLevel - 1].DOFade(0, 1.3f).SetEase(Ease.InExpo);
        // lockedIcon[unlockedLevel - 1].transform.DOScale(0, 1f).SetEase(Ease.InElastic).OnComplete(() =>
        // {
        //     lockedIcon[unlockedLevel - 1].SetActive(false);
        //     selectedVFX[unlockedLevel - 1].SetActive(true);
        //     startButtons[unlockedLevel - 1].SetActive(true);

        //     selectedVFX[unlockedLevel - 1].transform.localScale = Vector3.zero;
        //     startButtons[unlockedLevel - 1].transform.localScale = Vector3.zero;
        //     levelButtons[unlockedLevel - 1].transform.DOScale(1f, 0.15f).SetEase(Ease.InQuad);
        //     selectedVFX[unlockedLevel - 1].transform.DOScale(1f, 0.2f).SetEase(Ease.InQuad);
        //     startButtons[unlockedLevel - 1].transform.DOScale(1.1f, 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
        //     {
        //         startButtons[unlockedLevel - 1].transform.DOScale(1f, 0.7f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

        //         GameManager.Instance.isThoucedActive = true;
        //     });
        // });
    }

    public void Animate()
    {
        animatorButtons[unlockedLevel - 1].enabled = false;

        startButtons[unlockedLevel - 1].transform.DOScale(1.1f, 0.4f).SetEase(Ease.InQuad).SetDelay(0.2f).OnComplete(() =>
        {
            startButtons[unlockedLevel - 1].transform.DOScale(1f, 0.7f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

            GameManager.Instance.isThoucedActive = true;
        });

        level = unlockedLevel;
    }

    // assign to every level button
    public void SelectLevel(int selectedLevel)
    {
        if (!GameManager.Instance.isThoucedActive)
        {
            return;
        }

        // DOTween.KillAll();

        Vector3 targetScaleAnim = new Vector3(levelButtonInitScale.x + 0.1f, levelButtonInitScale.y + 0.1f, levelButtonInitScale.z + 0.1f);

        // if levelbutton still locked
        if (lockedIcon[selectedLevel].activeSelf)
        {
            GameManager.Instance.isThoucedActive = false;
            AudioManager.Instance.PlaySFX(lockedSfx);
            levelButtons[selectedLevel].transform.DOScale(targetScaleAnim, 0.1f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                {
                    GameManager.Instance.isThoucedActive = true;
                    return;
                });
        }
        else
        {
            if(!isFirstTime)
            {
                AudioManager.Instance.PlaySFX(buttonClickSfx);
            }

            isFirstTime = false;

            previousLevel = level; //for determine condition if level != previous level 
            level = selectedLevel + 1;

            if (startButtons[level - 1].activeSelf)
            {
                return;
            }

            GameManager.Instance.isThoucedActive = false;
            
            startButtons[previousLevel - 1].SetActive(false);
            selectedVFX[previousLevel - 1].SetActive(false);
            levelButtons[previousLevel - 1].transform.DOScale(levelButtonInitScale, 0.1f).SetEase(Ease.OutQuad);

            startButtons[level - 1].SetActive(true);
            selectedVFX[level - 1].SetActive(true);
            levelButtons[level - 1].transform.DOScale(targetScaleAnim, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                GameManager.Instance.isThoucedActive = true;
            });
        }
    }

    // assign to background gameobject
    public void HideStartButton()
    {
        if (!GameManager.Instance.isThoucedActive)
        {
            return;
        }

        levelButtons[level - 1].transform.DOScale(levelButtonInitScale, 0.1f).SetEase(Ease.OutQuad);

        startButtons[level - 1].SetActive(false);
        selectedVFX[level - 1].SetActive(false);
    }

    public void OpenConfirmationPanel()
    {
        GameManager.Instance.isThoucedActive = false;

        levelImage.sprite = levelImageRef[level - 1];
        switch (level - 1)
        {
            case 0:
                titleText.text = Settings.collectionTitle1;
                break;
            case 1:
                titleText.text = Settings.collectionTitle2;
                break;
            case 2:
                titleText.text = Settings.collectionTitle3Alt;
                break;
            case 3:
                titleText.text = Settings.collectionTitle4;
                break;
            case 4:
                titleText.text = Settings.collectionTitle5;
                break;
            case 5:
                titleText.text = Settings.collectionTitle6;
                break;
            case 6:
                titleText.text = Settings.collectionTitle7;
                break;
            default:
                titleText.text = "???";
                break;
        }

        AudioManager.Instance.PlaySFX(locationSfx[level - 1]);

        confirmationPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        confirmationPanel.transform.localScale = Vector3.zero;
        confirmationPanel.SetActive(true);

        confirmationPanel.transform.DOScale(1, 0.4f).SetEase(Ease.OutExpo);
        confirmationPanel.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 150), 0.3f).SetDelay(0.2f);
    }

    public void CloseConfirmationPanel()
    {
        AudioManager.Instance.StopSFX();

        AudioManager.Instance.PlaySFX(buttonClickSfx);

        confirmationPanel.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 0), 0.1f).OnComplete(() =>
        {
            confirmationPanel.transform.DOScale(0, 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                confirmationPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 150);
                confirmationPanel.transform.localScale = confirmationPanelScale;

                GameManager.Instance.isThoucedActive = true;
                confirmationPanel.SetActive(false);
            });
        });
    }

    public void LoadToLevel()
    {
        AudioManager.Instance.StopSFX();

        AudioManager.Instance.PlaySFX(buttonClickSfx);

        DOTween.KillAll();

        SceneController.instance.LoadScene(((Scenes)level - 1).ToString());
        GameManager.Instance.isGameActive = true;
    }

    public void LoadMainMenu()
    {
        if (!GameManager.Instance.isThoucedActive)
        {
            return;
        }

        AudioManager.Instance.PlaySFX(buttonClickSfx);

        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }
}
