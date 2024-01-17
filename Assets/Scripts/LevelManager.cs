using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float countdownTime = 60f; // Initial countdown time in seconds //TODO: change value from leveldata
    [SerializeField] private float intervalTime = 10f; //TODO: change value from level data
    private float currentTime; // Current countdown time
    private float currentInterval;
    private TimeSpan time;


    [Header("Health")]
    [SerializeField] private Image[] images;
    private int maxHealth = 0;


    [Header("Progress")]
    [SerializeField] private TextMeshProUGUI proggressText;
    [SerializeField] private int maxProgress = 5; //TODO: change value from level data
    private int progressCounter;


    [Header("GameOver Panel")]
    [SerializeField] private GameObject gameOverWinUI;
    [SerializeField] private GameObject gameOverLoseUI;

    [Header("Pause")]
    [SerializeField] private GameObject pauseUI;

    [Header("Animator")]
    [SerializeField] private Animator anim;
    private bool canPlayAnim;

    private void OnEnable()
    {
        EventHandler.DecreaseHealth += DecreaseHealth;
        EventHandler.DifferenceClicked += UpdateObjectiveText;
    }

    private void OnDisable()
    {
        EventHandler.DecreaseHealth -= DecreaseHealth;
        EventHandler.DifferenceClicked -= UpdateObjectiveText;
    }

    private void Start()
    {
        currentTime = countdownTime;
        currentInterval = intervalTime;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = true;
            maxHealth++;
        }

        progressCounter = 0;
        proggressText.text = progressCounter + "/" + maxProgress;

        gameOverWinUI.SetActive(false);
        gameOverLoseUI.SetActive(false);
        pauseUI.SetActive(false);

        canPlayAnim = true;
    }

    private void Update()
    {
        if (GameManager.instance.isGameActive)
        {
            // Update countdown time
            currentTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);

            // If countdown time is up, stop the timer and do something
            if (currentTime <= 0f)
            {
                Lose();
                Debug.Log("Countdown time is up!");
            }
            else
            {
                timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);

                currentInterval -= Time.deltaTime;

                if ((currentInterval < 2 && currentInterval > 0) && canPlayAnim)
                {
                    anim.Play("TransformIcon");
                    canPlayAnim = false;
                }

                if (currentInterval <= 0)
                {
                    ChangeImageTransform();
                    currentInterval = intervalTime;
                    canPlayAnim = true;
                }
            }
        }
    }

    private void ChangeImageTransform()
    {
        // Code to be executed every x second
        EventHandler.CallChangeImageTransformEvent();
    }

    private void DecreaseHealth()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].enabled)
            {
                images[i].enabled = false;
                maxHealth--;
                break;
            }
        }

        if (maxHealth <= 0)
        {
            Lose();
        }
    }

    private void UpdateObjectiveText()
    {
        progressCounter++;

        proggressText.text = progressCounter + "/" + maxProgress;

        if (progressCounter == maxProgress)
        {
            Win();
        }
    }

    private void Win()
    {
        GameManager.instance.isGameActive = false;
        EventHandler.CallResetImageTransformEvent();

        // Unlock level selection
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
        }

        //TODO: change with images animation then set active the gameover panel
        gameOverWinUI.SetActive(true);
        Debug.Log("Game Over - Win");
    }

    private void Lose()
    {
        GameManager.instance.isGameActive = false;
        EventHandler.CallResetImageTransformEvent();

        //TODO: change with images animation then set active the gameover panel
        gameOverLoseUI.SetActive(true);
        Debug.Log("Game Over - Lose");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        GameManager.instance.isGameActive = true;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameManager.instance.isGameActive = false;

        pauseUI.SetActive(true);
        Debug.Log("Paused!!");
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.instance.isGameActive = true;

        pauseUI.SetActive(false);
        Debug.Log("Resumed!!");
    }

    public void LoadNextLevel()
    {
        SceneController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.instance.isGameActive = true;
    }

    public void LoadMainMenu()
    {
        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }

    public void LoadStageMenu()
    {
        SceneController.instance.LoadScene(Scenes.StageMenu.ToString());
    }
}
