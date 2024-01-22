using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menuButtons;

    private void Start()
    {
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
            menuButton.transform.DOScale(1f, .3f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(.05f);
        }
    }

    public void LoadStageMenu()
    {
        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.StageMenu.ToString());
    }

    public void LoadCollectionMenu()
    {
        DOTween.KillAll();

        SceneController.instance.LoadScene(Scenes.CollectionMenu.ToString());
    }
}
