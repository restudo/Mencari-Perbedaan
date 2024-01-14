using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadStageMenu()
    {
        SceneController.instance.LoadScene(Scenes.StageMenu.ToString());
    }

    public void LoadCollectionMenu()
    {
        SceneController.instance.LoadScene(Scenes.CollectionMenu.ToString());
    }
}
