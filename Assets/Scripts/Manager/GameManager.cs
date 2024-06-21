using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    public bool isThoucedActive;

    public float countdownTimer;
    public float intervalTimer;

    public bool canStageButtonAnim;
    // public bool isFirstTimePlaying;
    public int maxLevel;

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

    private void Start()
    {
        PlayerPrefs.DeleteKey("UnlockedLevel");
        // PlayerPrefs.SetInt("UnlockedLevel", 7);
        // isFirstTimePlaying = true;
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.T))
    //     {
    //         PlayerPrefs.SetInt("UnlockedLevel", LoadUnlockedLevel() + 1);
    //     }
    // }

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
    }
}
