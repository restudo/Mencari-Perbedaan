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
    [SerializeField] private GameObject healthContainer;
    // [SerializeField] private Sprite fullHeart;
    // [SerializeField] private Sprite emptyHeart;
    private GameObject[] hearts;
    private int healthCount;


    [Header("Progress")]
    [SerializeField] private GameObject progressContainer;
    // [SerializeField] private Sprite fullProgress;
    // [SerializeField] private Sprite emptyProgress;
    private GameObject[] progresses;
    private int progressCount;
    // [SerializeField] private int maxProgress = 5; //TODO: change value from level data


    [Header("GameOver Panel")]
    [SerializeField] private GameObject gameOverWinUI;
    [SerializeField] private GameObject gameOverLoseUI;

    [Header("Pause")]
    [SerializeField] private GameObject pauseUI;

    [Header("Animator & Image Control")]
    [SerializeField] private ImageControl imgControl;
    [SerializeField] private Transform objectInAndOut;
    [SerializeField] private Image objectAlertSwitch;
    [SerializeField] private Image objectAlertFlipLeft;
    [SerializeField] private Image objectAlertFlipRight;
    private ImageTransform imageTransform;
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

        int heartContainerChildCount = healthContainer.transform.childCount;
        hearts = new GameObject[heartContainerChildCount];
        for (int i = 0; i < heartContainerChildCount; i++)
        {
            hearts[i] = healthContainer.transform.GetChild(i).gameObject;
            hearts[i].transform.GetChild(0).gameObject.SetActive(false);
            // hearts[i].sprite = emptyHeart;
            healthCount++;
        }

        int progressContainerChildCount = progressContainer.transform.childCount;
        progresses = new GameObject[progressContainerChildCount];
        for (int i = 0; i < progressContainerChildCount; i++)
        {
            progresses[i] = progressContainer.transform.GetChild(i).gameObject;
            progresses[i].transform.GetChild(0).gameObject.SetActive(false);
            // progresses[i].sprite = emptyProgress;
            progressCount++;
        }
        // proggressText.text = progressCounter + "/" + maxProgress;

        gameOverWinUI.SetActive(false);
        gameOverLoseUI.SetActive(false);
        pauseUI.SetActive(false);

        objectAlertFlipLeft.gameObject.SetActive(false);
        objectAlertFlipRight.gameObject.SetActive(false);
        objectAlertSwitch.gameObject.SetActive(false);

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

                if ((currentInterval < 2 && currentInterval > 0) && canPlayAnim && currentTime >= 5)
                {
                    canPlayAnim = false;

                    // animate object
                    imageTransform = RandomTransform();
                    StartCoroutine(ObjectAlert(imageTransform));
                    StartCoroutine(ObjectInAndOut());
                }

                if (currentInterval < 1 && currentInterval >= 0)
                {
                    GameManager.instance.isThoucedActive = false;
                }

                if (currentInterval <= 0)
                {
                    StartCoroutine(ChangeImageTransform(imageTransform));
                    // currentInterval = intervalTime;
                    currentInterval = GameManager.instance.intervalTimer;
                    canPlayAnim = true;
                }
            }
        }
    }

    private IEnumerator ObjectAlert(ImageTransform imageTransform)
    {
        switch (imageTransform)
        {
            case ImageTransform.Flip:
                objectAlertFlipLeft.gameObject.SetActive(true);
                objectAlertFlipRight.gameObject.SetActive(true);

                // Tween the alpha of a object alert color to 1
                DOTween.ToAlpha(() => objectAlertFlipLeft.color, x => objectAlertFlipLeft.color = x, 0, 0);
                DOTween.ToAlpha(() => objectAlertFlipRight.color, x => objectAlertFlipRight.color = x, 0, 0);

                objectAlertFlipLeft.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
                objectAlertFlipRight.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);

                yield return new WaitForSeconds(2f); // objectalertFlip wait for this seconds

                objectAlertFlipLeft.DOKill();
                objectAlertFlipRight.DOKill();
                objectAlertFlipLeft.gameObject.SetActive(false);
                objectAlertFlipRight.gameObject.SetActive(false);

                break;
            case ImageTransform.Switch:
                objectAlertSwitch.gameObject.SetActive(true);

                // Tween the alpha of a object alert color to 1
                DOTween.ToAlpha(() => objectAlertSwitch.color, x => objectAlertSwitch.color = x, 0, 0);

                objectAlertSwitch.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);

                yield return new WaitForSeconds(2f); // objectAlertSwitch wait for this seconds

                objectAlertSwitch.DOKill();
                objectAlertSwitch.gameObject.SetActive(false);

                break;
            // case ImageTransform.RotateLeft:
            //     RotateLeft();
            //     break;
            // case ImageTransform.RotateRight:
            //     RotateRight();
            //     break;
            default:
                break;
        }
    }

    private IEnumerator ObjectInAndOut()
    {
        float x = objectInAndOut.position.x;

        objectInAndOut.DOMoveX(-6, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2.3f); // objectInAndOut wait for this seconds

        objectInAndOut.DOMoveX(x, 0.5f).SetEase(Ease.InOutBack);
    }

    private ImageTransform RandomTransform()
    {
        ImageTransform imgTransform;

        do
        {
            imgTransform = GetRandomTransform();
        } while (imgTransform == ImageTransform.Flip &&
                (imgControl.images[0].transform.eulerAngles.z == 90f ||
                imgControl.images[0].transform.eulerAngles.z == 270));

        return imgTransform;
    }

    private ImageTransform GetRandomTransform()
    {
        ImageTransform[] allImgTransform = (ImageTransform[])Enum.GetValues(typeof(ImageTransform));
        return allImgTransform[UnityEngine.Random.Range(0, allImgTransform.Length)];
    }

    private IEnumerator ChangeImageTransform(ImageTransform imageTransform)
    {
        // Code to be executed every x second
        EventHandler.CallChangeImageTransformEvent(imageTransform);

        yield return new WaitForSeconds(.3f);

        GameManager.instance.isThoucedActive = true;
    }

    private void DecreaseHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < healthCount)
            {
                GameManager.instance.isThoucedActive = false;
                healthCount--;
                // hearts[healthCount].sprite = fullHeart;
                hearts[healthCount].transform.GetChild(0).gameObject.SetActive(true);

                Vector3 punch = new Vector3(.7f, .7f, .7f);
                hearts[healthCount].transform.GetChild(0).DOPunchScale(punch, .3f, 0, 0.2f).OnComplete(() =>
                {
                    GameManager.instance.isThoucedActive = true;
                });

                break;
            }
        }

        if (healthCount <= 0)
        {
            GameManager.instance.isGameActive = false;
            GameManager.instance.isThoucedActive = false;

            // Lose();
            Invoke("Lose", 2f); // change with Lose(); if there is an animation when winning
        }
    }

    private void UpdateObjectiveText()
    {
        // progressCount++;

        // proggressText.text = progressCounter + "/" + maxProgress;

        // if (progressCount == maxProgress)
        // {
        //     GameManager.instance.isGameActive = false;
        //     GameManager.instance.isThoucedActive = false;

        //     // Win();
        //     Invoke("Win", 2f); // change with Win(); if there is an animation when winning
        // }

        for (int i = 0; i < progresses.Length; i++)
        {
            if (i < progressCount)
            {
                GameManager.instance.isThoucedActive = false;
                progressCount--;
                // progresses[progressCount].sprite = fullProgress;
                progresses[progressCount].transform.GetChild(0).gameObject.SetActive(true);

                Vector3 punch = new Vector3(.7f, .7f, .7f);
                progresses[progressCount].transform.GetChild(0).DOPunchScale(punch, .3f, 0, 0.2f).OnComplete(() =>
                {
                    GameManager.instance.isThoucedActive = true;
                });

                break;
            }
        }

        if (progressCount <= 0)
        {
            GameManager.instance.isGameActive = false;
            GameManager.instance.isThoucedActive = false;

            Invoke("Win", 2f); // change with Win(); if there is an animation when winning
        }
    }

    private void Win()
    {
        EventHandler.CallResetImageTransformEvent();

        // Unlock level selection
        GameManager.instance.UnlockLevelSelection();

        //TODO: change with images animation then set active the gameover panel
        gameOverWinUI.transform.localScale = Vector3.zero;
        gameOverWinUI.transform.DOScale(1, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            gameOverWinUI.SetActive(true);
            DOTween.KillAll();
        });

        Debug.Log("Game Over - Win");
    }

    private void Lose()
    {
        GameManager.instance.isGameActive = false;
        EventHandler.CallResetImageTransformEvent();

        //TODO: change with images animation then set active the gameover panel
        gameOverLoseUI.transform.localScale = Vector3.zero;
        gameOverLoseUI.transform.DOScale(1, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            gameOverLoseUI.SetActive(true);
            DOTween.KillAll();
        });

        Debug.Log("Game Over - Lose");
    }

    public void RestartScene()
    {
        DOTween.KillAll();

        GameManager.instance.isGameActive = true;
        GameManager.instance.isThoucedActive = true;

        Time.timeScale = 1;

        GameManager.instance.isGameActive = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        if (!GameManager.instance.isGameActive)
        {
            return;
        }

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
