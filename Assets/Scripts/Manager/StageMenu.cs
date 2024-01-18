using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    [SerializeField] private GameObject startButton;

    [SerializeField] private Button[] levelButtons;

    private int level;

    private void Start()
    {
        startButton.SetActive(false);

        int unlockedLevel = GameManager.instance.LoadUnlockedLevel();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    public void SelectLevel()
    {
        level = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex() + 1;

        Debug.Log(level);
        startButton.SetActive(true);
    }

    public void LoadToLevel()
    {
        string sceneName = "Level" + level.ToString("D2");
        SceneController.instance.LoadScene(sceneName);
        GameManager.instance.isGameActive = true;
    }

    public void LoadMainMenu()
    {
        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }
}
