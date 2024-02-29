using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    public bool isThoucedActive;

    public float countdownTimer;
    public float intervalTimer;

    public bool canStageButtonAnim;

    private int unlockedLevel;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int LoadUnlockedLevel()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        return unlockedLevel;
    }

    public void UnlockLevelSelection()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int level;

        if (int.TryParse(sceneName.Substring("Level".Length), out level) && level >= LoadUnlockedLevel())
        {
            PlayerPrefs.SetInt("UnlockedLevel", level + 1);
            PlayerPrefs.Save();
        }

        Debug.Log("Unlock level: " + level);
    }
}
