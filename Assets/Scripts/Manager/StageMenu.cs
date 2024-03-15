using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    [SerializeField] private GameObject backButton;

    [SerializeField] private GameObject[] selectedVFX;
    [SerializeField] private Image[] graphics;
    [SerializeField] private GameObject[] startButtons;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject[] lockedIcon;

    int unlockedLevel;
    private int level;
    private int previousLevel;
    private bool isPlayAnim;

    private void Start()
    {
        GameManager.Instance.isThoucedActive = true;

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
                lockedIcon[i].SetActive(true); // set locked icon to true

                graphics[i].color = new Color32(94, 94, 94, 255);
            }
            else
            {
                lockedIcon[i].SetActive(false); // set locked icon to false

                graphics[i].color = Color.white;
            }
        }

        if (GameManager.Instance.canStageButtonAnim && graphics[unlockedLevel - 1] != null)
        {
            GameManager.Instance.isThoucedActive = false;
            lockedIcon[unlockedLevel - 1].SetActive(true);
            graphics[unlockedLevel - 1].color = new Color32(94, 94, 94, 255);
        }

        StartCoroutine(ButtonPopAnim());
    }

    private IEnumerator ButtonPopAnim()
    {
        foreach (Button button in levelButtons)
        {
            button.transform.localScale = Vector3.zero;
        }

        backButton.transform.localScale = Vector3.zero;
        backButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        foreach (Button button in levelButtons)
        {
            button.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.05f);
        }

        if (GameManager.Instance.canStageButtonAnim && graphics[unlockedLevel - 1] != null)
        {
            UnlockButtonAnim();

            GameManager.Instance.canStageButtonAnim = false;
        }
    }

    private void UnlockButtonAnim()
    {
        graphics[unlockedLevel - 1].DOColor(Color.white, 1.3f).SetEase(Ease.InExpo);
        lockedIcon[unlockedLevel - 1].transform.DOScale(0, 1f).SetEase(Ease.InElastic).OnComplete(() =>
        {
            lockedIcon[unlockedLevel - 1].SetActive(false);
            selectedVFX[unlockedLevel - 1].SetActive(true);
            startButtons[unlockedLevel - 1].SetActive(true);

            selectedVFX[unlockedLevel - 1].transform.localScale = Vector3.zero;
            startButtons[unlockedLevel - 1].transform.localScale = Vector3.zero;
            selectedVFX[unlockedLevel - 1].transform.DOScale(1f, 0.2f).SetEase(Ease.InQuad);
            startButtons[unlockedLevel - 1].transform.DOScale(1.1f, 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                startButtons[unlockedLevel - 1].transform.DOScale(1f, 0.7f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

                GameManager.Instance.isThoucedActive = true;
            });
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

        DOTween.KillAll();

        Vector3 targetScaleAnim = new Vector3(levelButtons[selectedLevel].transform.localScale.x + 0.1f, levelButtons[selectedLevel].transform.localScale.y + 0.1f, levelButtons[selectedLevel].transform.localScale.z + 0.1f);

        if (lockedIcon[selectedLevel].activeSelf)
        {
            GameManager.Instance.isThoucedActive = false;
            levelButtons[selectedLevel].transform.DOScale(targetScaleAnim, 0.1f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                GameManager.Instance.isThoucedActive = true;
                return;
            });

        }
        else
        {
            previousLevel = level; //for determine condition if level != previous level 
            level = selectedLevel + 1;

            if (startButtons[level - 1].activeSelf)
            {
                return;
            }

            startButtons[previousLevel - 1].SetActive(false);
            selectedVFX[previousLevel - 1].SetActive(false);
            levelButtons[previousLevel - 1].transform.DOScale(1, 0.1f).SetEase(Ease.OutQuad);

            startButtons[level - 1].SetActive(true);
            selectedVFX[level - 1].SetActive(true);
            levelButtons[level - 1].transform.DOScale(targetScaleAnim, 0.1f).SetEase(Ease.OutQuad);
        }
    }

    // assign to background gameobject
    public void HideStartButton()
    {
        if (!GameManager.Instance.isThoucedActive)
        {
            return;
        }

        levelButtons[level - 1].transform.DOScale(1, 0.1f).SetEase(Ease.OutQuad);

        startButtons[level - 1].SetActive(false);
        selectedVFX[level - 1].SetActive(false);
    }

    public void LoadToLevel()
    {
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

        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }
}
