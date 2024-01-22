using UnityEngine;

public class DebugMode : MonoBehaviour
{
    [SerializeField] private GameObject[] debugObj;

    public void ToggleDebugMode()
    {
        foreach (var item in debugObj)
        {
            if(item.activeSelf)
            {
                item.SetActive(false);
            }
            else
            {
                item.SetActive(true);
            }
        }
    }
}
