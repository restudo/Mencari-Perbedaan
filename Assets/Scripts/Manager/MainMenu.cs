using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menuButtons;

    [Space(50)]
    [Header("BGM")]
    [SerializeField] private AudioClip bgmMenuAudioClip;
    [Header("SFX")]
    [SerializeField] private AudioClip buttonClickSfx;
    // [Header("SFX")]
    // [SerializeField] private AudioClip[] mikoAudioClip;

    private void Start()
    {
        GameManager.Instance.isTouchActive = true;

        // AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(bgmMenuAudioClip);

        StartCoroutine(PopAnim());
    }

    private IEnumerator PopAnim()
    {
        foreach (GameObject menuButton in menuButtons)
        {
            menuButton.transform.localScale = Vector3.zero;
        }
        foreach (GameObject menuButton in menuButtons)
        {
            yield return new WaitForSeconds(.05f);
            menuButton.transform.DOScale(1f, .3f).SetEase(Ease.OutBounce);
        }
    }

    public void LoadStageMenu()
    {
        AudioManager.Instance.PlaySFX(buttonClickSfx);

        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.StageMenu.ToString());
    }

    public void LoadCollectionMenu()
    {
        AudioManager.Instance.PlaySFX(buttonClickSfx);

        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.CollectionMenu.ToString());
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
