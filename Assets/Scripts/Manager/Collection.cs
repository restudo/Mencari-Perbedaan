using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private GameObject collectionContainer;
    [SerializeField] private GameObject collectionPanel;

    [SerializeField] private SimpleScrollSnap simpleScrollSnap;

    public void ShowCollection(int index)
    {
        collectionPanel.SetActive(true);
        collectionContainer.SetActive(false);

        simpleScrollSnap.CustomGoToPanel(index);
    }

    public void Back()
    {
        if (collectionPanel.activeSelf)
        {
            collectionPanel.SetActive(false);
            collectionContainer.SetActive(true);
        }
        else
        {
            SceneController.instance.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}
