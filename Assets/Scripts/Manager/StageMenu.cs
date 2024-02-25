using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject backButton;

    [SerializeField] private Button[] levelButtons;

    private int level;
    private int previousLevel;

    private void Start()
    {
        startButton.SetActive(false);

        int unlockedLevel = GameManager.instance.LoadUnlockedLevel();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > unlockedLevel)
            {
                levelButtons[i].interactable = false;
                levelButtons[i].transform.GetChild(0).gameObject.SetActive(true); // set locked icon to true
            }
            else
            {
                levelButtons[i].interactable = true;
                levelButtons[i].transform.GetChild(0).gameObject.SetActive(false); // set locked icon to false
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
        previousLevel = level; //for determine condition if level != previous level 
        level = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex() + 1;

        RectTransform startButtonRect = (RectTransform)startButton.transform;
        Vector3 originAnchorPos = startButtonRect.anchoredPosition; //define anchored pos from recttransform in inspector

        startButton.transform.SetParent(EventSystem.current.currentSelectedGameObject.transform);

        startButtonRect.anchoredPosition = originAnchorPos;

        if (!startButton.activeSelf || previousLevel != level)
        {
            startButton.SetActive(true);
            startButton.transform.localScale = Vector3.zero;
            startButton.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }
    }

    // assign to background gameobject
    public void HideStartButton()
    {
        if (startButton.activeSelf)
        {
            startButton.transform.DOScale(0, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                startButton.SetActive(false);
            });
        }
    }

    public void LoadToLevel()
    {
        DOTween.KillAll();

        SceneController.instance.LoadScene(((Scenes)level).ToString());
        GameManager.instance.isGameActive = true;
    }

    public void LoadMainMenu()
    {
        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }
}
