using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    public bool isThoucedActive;

    public float countdownTimer;
    public float intervalTimer;

    private int unlockedLevel;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

        Debug.Log(level);
    }
}
