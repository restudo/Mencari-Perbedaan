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
    [SerializeField] private GameObject transformIcon;
    [SerializeField] private Image objectAlertSwitch;
    [SerializeField] private Image objectAlertFlipLeft;
    [SerializeField] private Image objectAlertFlipRight;

    [Space(50)]
    [Header("BGM")]
    [SerializeField] private AudioClip gameplayAudioClip;
    [Header("SFX")]
    [SerializeField] private AudioClip[] mikoAudioClip;
    [SerializeField] private AudioClip positiveSfx;
    [SerializeField] private AudioClip negativeSfx;
    [SerializeField] private AudioClip winSfx;
    [SerializeField] private AudioClip loseSfx;
    [SerializeField] private AudioClip buttonSfx;
    public AudioClip ButtonSfx { get => buttonSfx; private set => buttonSfx = value; }
    [SerializeField] private AudioClip countSfx;

    private GameObject switchIcon;
    private GameObject flipIcon;
    private ImageTransform imageTransform;
    private bool canPlayAnim;
    private bool isWinning;

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
        AudioManager.Instance.StopMusic();

        GameManager.Instance.isGameActive = false;
        GameManager.Instance.isTouchActive = false;

        currentTime = GameManager.Instance.countdownTimer; //for now all the level has the same countdown timer
        currentInterval = GameManager.Instance.intervalTimer; //and also intervalTimer as well

        int heartContainerChildCount = healthContainer.transform.childCount;
        hearts = new GameObject[heartContainerChildCount];
        for (int i = 0; i < heartContainerChildCount; i++)
        {
            hearts[i] = healthContainer.transform.GetChild(i).gameObject;
            hearts[i].transform.GetChild(0).gameObject.SetActive(false);
            healthCount++;
        }

        int progressContainerChildCount = progressContainer.transform.childCount;
        progresses = new GameObject[progressContainerChildCount];
        for (int i = 0; i < progressContainerChildCount; i++)
        {
            progresses[i] = progressContainer.transform.GetChild(i).gameObject;
            progresses[i].transform.GetChild(0).gameObject.SetActive(false);
            progressCount++;
        }

        gameOverWinUI.transform.GetChild(gameOverWinUI.transform.childCount - 1).gameObject.SetActive(false); // the confetti
        gameOverWinUI.transform.parent.gameObject.SetActive(false);
        gameOverLoseUI.transform.parent.gameObject.SetActive(false);
        pauseUI.SetActive(false);

        objectInAndOut.gameObject.SetActive(false);
        objectAlertFlipLeft.gameObject.SetActive(false);
        objectAlertFlipRight.gameObject.SetActive(false);
        objectAlertSwitch.gameObject.SetActive(false);

        switchIcon = transformIcon.transform.GetChild(0).gameObject; //swicth icon in the first child
        flipIcon = transformIcon.transform.GetChild(transformIcon.transform.childCount - 1).gameObject; //flip icon in the last child

        switchIcon.SetActive(false);
        flipIcon.SetActive(false);

        canPlayAnim = true;
        isWinning = false;

        time = TimeSpan.FromSeconds(currentTime);
        // timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
        timerText.text = time.Seconds.ToString();

        AudioManager.Instance.PlayMusic(gameplayAudioClip);
        AudioManager.Instance.SetMusicVolume(0.9f);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            // if (Input.GetKeyDown(KeyCode.T))
            // {
            //     // EventHandler.CallChangeImageTransformEvent(ImageTransform.Flip);
            //     int index = UnityEngine.Random.Range(0, mikoAudioClip.Length); // random miko sfx
            //     AudioManager.Instance.PlaySFX(mikoAudioClip[index]); // play miko sfx
            // }
            // if (Input.GetKeyDown(KeyCode.Y))
            // {
            //     EventHandler.CallChangeImageTransformEvent(ImageTransform.Switch);
            // }


            // Update countdown time
            currentTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentTime);

            // If countdown time is up, stop the timer and do something
            if (currentTime <= 0f)
            {
                GameManager.Instance.isGameActive = false;
                GameManager.Instance.isTouchActive = false;

                StartCoroutine(Lose());
            }
            else
            {
                // timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
                timerText.text = time.Seconds.ToString();

                currentInterval -= Time.deltaTime;

                // Play SFX every second left from 10 to 0
                if (currentTime <= 11 && Mathf.Floor(currentTime) != Mathf.Floor(currentTime + Time.deltaTime))
                {
                    AudioManager.Instance.PlaySFX(countSfx);
                }

                if ((currentInterval < 2 && currentInterval > 0) && canPlayAnim && currentTime >= 5)
                {
                    GameManager.Instance.isTouchActive = false;

                    canPlayAnim = false;

                    // animate object
                    imageTransform = RandomTransform();
                    StartCoroutine(ObjectAlert(imageTransform));
                    StartCoroutine(ObjectInAndOut());
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
        transformIcon.SetActive(true);
        transformIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        transformIcon.GetComponent<Image>().DOFade(1, 0.5f).SetEase(Ease.OutExpo);

        switch (imageTransform)
        {
            case ImageTransform.Flip:
                Vector3 flipIconOrigin = flipIcon.transform.localScale;
                flipIcon.SetActive(true);
                flipIcon.transform.DOScaleX(-1, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

                yield return new WaitForSeconds(2f); // objectalertFlip wait for this seconds

                flipIcon.transform.DOKill();
                flipIcon.SetActive(false);
                flipIcon.transform.localScale = flipIconOrigin;

                transformIcon.SetActive(false);

                break;
            case ImageTransform.Switch:
                Vector3 switchIconOrigin = switchIcon.transform.eulerAngles;
                switchIcon.SetActive(true);
                switchIcon.transform.DOLocalRotate(new Vector3(0, 0, -360), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

                yield return new WaitForSeconds(2f); // objectAlertSwitch wait for this seconds

                switchIcon.transform.DOKill();
                switchIcon.SetActive(false);
                switchIcon.transform.eulerAngles = switchIconOrigin;

                transformIcon.SetActive(false);

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

        int index = UnityEngine.Random.Range(0, mikoAudioClip.Length); // random miko sfx
        AudioManager.Instance.PlaySFX(mikoAudioClip[index]); // play miko sfx
        // AudioManager.Instance.PlaySFX(mikoAudioClip[index], transform, mikoAudioClipVolume);

        objectInAndOut.gameObject.SetActive(true);

        objectInAndOut.DOMove(end, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2f); // objectInAndOut wait for this seconds

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
                GameManager.Instance.isTouchActive = false;
                healthCount--;
                // hearts[healthCount].sprite = fullHeart;
                hearts[healthCount].transform.GetChild(0).gameObject.SetActive(true);

                Vector3 punch = new Vector3(.7f, .7f, .7f);
                hearts[healthCount].transform.GetChild(0).DOPunchScale(punch, .3f, 0, 0.3f).OnComplete(() =>
                {
                    GameManager.Instance.isTouchActive = true;
                });

                AudioManager.Instance.PlaySFX(negativeSfx);

                break;
            }
        }

        if (healthCount <= 0)
        {
            GameManager.Instance.isGameActive = false;
            GameManager.Instance.isTouchActive = false;

            // Lose();
            StartCoroutine(Lose()); // change with Lose(); if there is an animation when winning
        }
    }

    // private void UpdateProgressUI(float animDuration)
    // {
    //     StartCoroutine(UpdateProgressUICouroutine(animDuration));
    // }

    private void UpdateProgressUI(float animDuration)
    {
        for (int i = 0; i < progresses.Length; i++)
        {
            if (i <= progressCount)
            {
                if (progressCount == 1)
                {
                    GameManager.Instance.isGameActive = false;
                }

                StartCoroutine(DelayTouch(animDuration));

                // progresses[progressCount].sprite = fullProgress;

                break;
            }
        }
    }

    private IEnumerator DelayTouch(float animDuration)
    {
        GameManager.Instance.isTouchActive = false;

        yield return new WaitForSeconds(animDuration / 2);

        GameManager.Instance.isTouchActive = true;

        yield return new WaitForSeconds(animDuration / 2);

        AudioManager.Instance.PlaySFX(positiveSfx);

        progressCount--;

        // if this is the last progress, flag the win condition
        if (progressCount == 0)
        {
            GameManager.Instance.isGameActive = false;
            GameManager.Instance.isTouchActive = false;

            isWinning = true;
        }

        progresses[progressCount].transform.GetChild(0).gameObject.SetActive(true);

        Vector3 punch = new Vector3(.7f, .7f, .7f);
        progresses[progressCount].transform.GetChild(0).DOPunchScale(punch, 0.3f, 0, 0.3f).OnComplete(() =>
        {
            if (isWinning)
            {
                GameManager.Instance.isTouchActive = false;

                StartCoroutine(Win()); // change with Win(); if there is an animation when winning
            }
        });
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1);

        EventHandler.CallDestroyHintEvent();

        if (imgControl.images[0].transform.localScale.x < 0)
        {
            EventHandler.CallResetImageTransformEvent();
            yield return new WaitForSeconds(1);
        }

        // gameOverWinUI.transform.GetChild(gameOverWinUI.transform.childCount - 1).gameObject.SetActive(false); // the confetti

        // Anim
        gameOverWinUI.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        gameOverWinUI.transform.parent.localScale = Vector3.zero;
        gameOverWinUI.transform.parent.gameObject.SetActive(true);
        gameOverWinUI.transform.parent.DOScale(1, 0.4f).SetEase(Ease.OutBounce).SetDelay(0.6f);
        gameOverWinUI.transform.parent.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 150), 1.5f).SetDelay(1f);

        string sceneName = SceneManager.GetActiveScene().name;
        int level;
        if (int.TryParse(sceneName.Substring("Level".Length), out level) && level == GameManager.Instance.LoadUnlockedLevel() && level < GameManager.Instance.maxLevel)
        {
            GameManager.Instance.canStageButtonAnim = true;
        }

        // Unlock level selection
        GameManager.Instance.UnlockLevelSelection();

        yield return new WaitForSeconds(1.1f);
        gameOverWinUI.transform.GetChild(gameOverWinUI.transform.childCount - 1).gameObject.SetActive(true); // the confetti
        AudioManager.Instance.PlaySFX(winSfx);
    }

    private IEnumerator Lose()
    {
        // yield return new WaitForSeconds(1);

        EventHandler.CallDestroyHintEvent();

        if (imgControl.images[0].transform.localScale.x < 0)
        {
            EventHandler.CallResetImageTransformEvent();
            yield return new WaitForSeconds(1);
        }

        // Anim
        gameOverLoseUI.transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        gameOverLoseUI.transform.parent.localScale = Vector3.zero;
        gameOverLoseUI.transform.parent.gameObject.SetActive(true);
        gameOverLoseUI.transform.parent.DOScale(1, 0.4f).SetEase(Ease.OutBounce).SetDelay(0.6f);
        gameOverLoseUI.transform.parent.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 150), 1.5f).SetDelay(1f);

        yield return new WaitForSeconds(0.65f);
        AudioManager.Instance.PlaySFX(loseSfx, 0.7f);
    }

    public void RestartScene()
    {
        DOTween.KillAll();

        GameManager.Instance.isGameActive = true;
        GameManager.Instance.isTouchActive = true;

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

        AudioManager.Instance.PlaySFX(buttonSfx);

        Time.timeScale = 0;
        GameManager.Instance.isGameActive = false;

        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        AudioManager.Instance.PlaySFX(buttonSfx);

        Time.timeScale = 1;
        GameManager.Instance.isGameActive = true;

        pauseUI.SetActive(false);
    }

    // public void LoadNextLevel()
    // {
    //     Time.timeScale = 1;
    //     SceneController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //     GameManager.Instance.isGameActive = true;
    // }

    public void LoadMainMenu()
    {
        DOTween.KillAll();

        Time.timeScale = 1;
        SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
    }

    public void LoadStageMenu()
    {
        DOTween.KillAll();

        Time.timeScale = 1;
        SceneController.instance.LoadScene(Scenes.StageMenu.ToString());
    }

    public int GetProgress()
    {
        return progressContainer.transform.childCount;
    }
}
