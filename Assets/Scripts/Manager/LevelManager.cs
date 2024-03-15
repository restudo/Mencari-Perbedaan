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
    [SerializeField] private Transform inAndOutEndTransform;
    [SerializeField] private Image objectAlertSwitch;
    [SerializeField] private Image objectAlertFlipLeft;
    [SerializeField] private Image objectAlertFlipRight;
    private ImageTransform imageTransform;
    private bool canPlayAnim;

    private void OnEnable()
    {
        EventHandler.DecreaseHealth += DecreaseHealthUI;
        EventHandler.DifferenceClicked += UpdateProgressUI;
    }

    private void OnDisable()
    {
        EventHandler.DecreaseHealth -= DecreaseHealthUI;
        EventHandler.DifferenceClicked -= UpdateProgressUI;
    }

    private void Start()
    {
        GameManager.Instance.isGameActive = false;
        GameManager.Instance.isThoucedActive = false;

        currentTime = GameManager.Instance.countdownTimer; //for now all the level has the same countdown timer
        currentInterval = GameManager.Instance.intervalTimer; //and also intervalTimer as well

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

        gameOverWinUI.transform.parent.gameObject.SetActive(false);
        gameOverLoseUI.transform.parent.gameObject.SetActive(false);
        pauseUI.SetActive(false);
        objectInAndOut.gameObject.SetActive(false);

        objectAlertFlipLeft.gameObject.SetActive(false);
        objectAlertFlipRight.gameObject.SetActive(false);
        objectAlertSwitch.gameObject.SetActive(false);

        canPlayAnim = true;

        time = TimeSpan.FromSeconds(currentTime);
        timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            // Update countdown time
            currentTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);

            // If countdown time is up, stop the timer and do something
            if (currentTime <= 0f)
            {
                GameManager.Instance.isGameActive = false;
                GameManager.Instance.isThoucedActive = false;

                StartCoroutine(Lose());
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
                    GameManager.Instance.isThoucedActive = false;
                }

                if (currentInterval <= 0)
                {
                    EventHandler.CallChangeImageTransformEvent(imageTransform);
                    currentInterval = GameManager.Instance.intervalTimer;
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
        Vector3 start = objectInAndOut.transform.position;
        Vector3 end = inAndOutEndTransform.transform.position; // in and out end

        yield return new WaitForSeconds(1);

        objectInAndOut.gameObject.SetActive(true);

        objectInAndOut.DOMove(end, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2.3f); // objectInAndOut wait for this seconds

        objectInAndOut.DOMove(start, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            objectInAndOut.gameObject.SetActive(false);
        });
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

    private void DecreaseHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < healthCount)
            {
                GameManager.Instance.isThoucedActive = false;
                healthCount--;
                // hearts[healthCount].sprite = fullHeart;
                hearts[healthCount].transform.GetChild(0).gameObject.SetActive(true);

                Vector3 punch = new Vector3(.7f, .7f, .7f);
                hearts[healthCount].transform.GetChild(0).DOPunchScale(punch, .3f, 0, 0.2f).OnComplete(() =>
                {
                    GameManager.Instance.isThoucedActive = true;
                });

                break;
            }
        }

        if (healthCount <= 0)
        {
            GameManager.Instance.isGameActive = false;
            GameManager.Instance.isThoucedActive = false;

            // Lose();
            StartCoroutine(Lose()); // change with Lose(); if there is an animation when winning
        }
    }

    private void UpdateProgressUI()
    {
        for (int i = 0; i < progresses.Length; i++)
        {
            if (i < progressCount)
            {
                GameManager.Instance.isThoucedActive = false;
                progressCount--;
                // progresses[progressCount].sprite = fullProgress;
                progresses[progressCount].transform.GetChild(0).gameObject.SetActive(true);

                Vector3 punch = new Vector3(.7f, .7f, .7f);
                progresses[progressCount].transform.GetChild(0).DOPunchScale(punch, .3f, 0, 0.2f).OnComplete(() =>
                {
                    GameManager.Instance.isThoucedActive = true;
                });

                break;
            }
        }

        if (progressCount <= 0)
        {
            GameManager.Instance.isGameActive = false;
            GameManager.Instance.isThoucedActive = false;

            StartCoroutine(Win()); // change with Win(); if there is an animation when winning
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1);

        EventHandler.CallResetImageTransformEvent();

        yield return new WaitForSeconds(1);

        // Anim
        gameOverWinUI.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        gameOverWinUI.transform.parent.localScale = Vector3.zero;
        gameOverWinUI.transform.parent.gameObject.SetActive(true);
        //TODO: change with images animation then set active the gameover panel
        gameOverWinUI.transform.parent.DOScale(1, 0.4f).SetEase(Ease.OutBounce).SetDelay(0.6f);
        gameOverWinUI.transform.parent.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 150), 1.5f).SetDelay(1f);

        string sceneName = SceneManager.GetActiveScene().name;
        int level;

        if (int.TryParse(sceneName.Substring("Level".Length), out level) && level == GameManager.Instance.LoadUnlockedLevel())
        {
            GameManager.Instance.canStageButtonAnim = true;
            Debug.Log("Unlocked Anim");
        }

        // Unlock level selection
        GameManager.Instance.UnlockLevelSelection();

        Debug.Log("Game Over - Win");
    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(1);

        EventHandler.CallResetImageTransformEvent();

        yield return new WaitForSeconds(1);

        // Anim
        gameOverLoseUI.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        gameOverLoseUI.transform.parent.localScale = Vector3.zero;
        gameOverLoseUI.transform.parent.gameObject.SetActive(true);
        //TODO: change with images animation then set active the gameover panel
        gameOverLoseUI.transform.parent.DOScale(1, 0.4f).SetEase(Ease.OutBounce).SetDelay(0.6f);
        gameOverLoseUI.transform.parent.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 150), 1.5f).SetDelay(1f);

        Debug.Log("Game Over - Lose");
    }

    public void RestartScene()
    {
        DOTween.KillAll();

        GameManager.Instance.isGameActive = true;
        GameManager.Instance.isThoucedActive = true;

        Time.timeScale = 1;

        GameManager.Instance.isGameActive = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        if (!GameManager.Instance.isGameActive)
        {
            return;
        }

        Time.timeScale = 0;
        GameManager.Instance.isGameActive = false;

        pauseUI.SetActive(true);
        Debug.Log("Paused!!");
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.Instance.isGameActive = true;

        pauseUI.SetActive(false);
        Debug.Log("Resumed!!");
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.isGameActive = true;
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
