using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    // [SerializeField] private float countdownTime = 60f; // Initial countdown time in seconds //TODO: change value from leveldata
    // [SerializeField] private float intervalTime = 10f; //TODO: change value from level data
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
    [SerializeField] private Transform objectInAndOut;
    [SerializeField] private Image objectAlert;
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
        currentTime = GameManager.instance.countdownTimer; //for now all the level has the same countdown timer
        currentInterval = GameManager.instance.intervalTimer; //and also intervalTimer as well

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

        GameManager.instance.isGameActive = true;
        GameManager.instance.isThoucedActive = true;
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
                    canPlayAnim = false;

                    // animate object
                    StartCoroutine(ObjectAlert());
                    StartCoroutine(ObjectInAndOut());
                }

                if (currentInterval < 1 && currentInterval >= 0)
                {
                    GameManager.instance.isThoucedActive = false;
                }

                if (currentInterval <= 0)
                {
                    StartCoroutine(ChangeImageTransform());
                    // currentInterval = intervalTime;
                    currentInterval = GameManager.instance.intervalTimer;
                    canPlayAnim = true;
                }
            }
        }
    }

    private IEnumerator ChangeImageTransform()
    {
        // Code to be executed every x second
        EventHandler.CallChangeImageTransformEvent();

        yield return new WaitForSeconds(.3f);

        GameManager.instance.isThoucedActive = true;
    }

    private IEnumerator ObjectAlert()
    {
        // Tween the alpha of a object alert color to 1
        DOTween.ToAlpha(() => objectAlert.color, x => objectAlert.color = x, 0, 0);

        objectAlert.gameObject.SetActive(true);
        objectAlert.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);

        yield return new WaitForSeconds(2f); // objectalert wait for this seconds

        objectAlert.DOKill();
        objectAlert.gameObject.SetActive(false);
    }

    private IEnumerator ObjectInAndOut()
    {
        float x = objectInAndOut.position.x;

        objectInAndOut.DOMoveX(-6, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2.3f); // objectInAndOut wait for this seconds

        objectInAndOut.DOMoveX(x, 0.5f).SetEase(Ease.InOutBack);
    }

    private void DecreaseHealth()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].enabled)
            {
                maxHealth--;
                images[i].transform.DOPunchScale(new Vector3(.7f, .7f, .7f), .3f, 0, 0.2f).OnComplete(() =>
                {
                    images[i].enabled = false;
                });

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
        GameManager.instance.UnlockLevelSelection();

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

        GameManager.instance.isGameActive = true;
        GameManager.instance.isThoucedActive = true;

        Time.timeScale = 1;

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
        Time.timeScale = 1;
        SceneController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.instance.isGameActive = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }

    public void LoadStageMenu()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadScene(Scenes.StageMenu.ToString());
    }
}
