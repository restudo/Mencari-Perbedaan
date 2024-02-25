using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    [SerializeField] private GameObject backButton;

    [SerializeField] private GameObject[] startButtons;
    [SerializeField] private GameObject[] selectedVFX;
    [SerializeField] private Button[] levelButtons;

    private int level;
    private int previousLevel;
    private GameObject currentSelectedLevel;

    private void Start()
    {
        level = 1;

        foreach (var button in startButtons)
        {
            button.SetActive(false);
        }
        foreach (var vfx in selectedVFX)
        {
            vfx.SetActive(false);
        }

        int unlockedLevel = GameManager.instance.LoadUnlockedLevel();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > unlockedLevel)
            {
                levelButtons[i].interactable = false;
                levelButtons[i].transform.GetChild(levelButtons[i].transform.childCount - 1).gameObject.SetActive(true); // set locked icon to true, locked icon in the last child
            }
            else
            {
                levelButtons[i].interactable = true;
                levelButtons[i].transform.GetChild(levelButtons[i].transform.childCount - 1).gameObject.SetActive(false); // set locked icon to false, locked icon in the last child
            }
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
    }

    // assign to every level button
    public void SelectLevel()
    {
        Debug.Log(level);
        currentSelectedLevel = EventSystem.current.currentSelectedGameObject;
        previousLevel = level; //for determine condition if level != previous level 
        level = currentSelectedLevel.transform.GetSiblingIndex() + 1;
        Debug.Log(level);

        // RectTransform startButtonRect = (RectTransform)startButton.transform;
        // Vector3 originAnchorPos = startButtonRect.anchoredPosition; //define anchored pos from recttransform in inspector

        // startButton.transform.SetParent(currentSelectedLevel.transform);

        // startButtonRect.anchoredPosition = originAnchorPos;

        // if (!startButton.activeSelf || previousLevel != level)
        // {
        //     startButton.SetActive(true);
        //     startButton.transform.localScale = Vector3.zero;
        //     startButton.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        // }

        startButtons[previousLevel - 1].SetActive(false);
        selectedVFX[previousLevel - 1].SetActive(false);

        startButtons[level - 1].SetActive(true);
        selectedVFX[level - 1].SetActive(true);
    }

    // assign to background gameobject
    public void HideStartButton()
    {
        // startButtons[level - 1].transform.DOScale(0, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
        // {
        //     startButtons[level - 1].SetActive(false);
        // });

        startButtons[level - 1].SetActive(false);
        selectedVFX[level - 1].SetActive(false);
    }

    public void LoadToLevel()
    {
        DOTween.KillAll();

        SceneController.instance.LoadScene(((Scenes)level - 1).ToString());
        GameManager.instance.isGameActive = true;
    }

    public void LoadMainMenu()
    {
        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }
}
