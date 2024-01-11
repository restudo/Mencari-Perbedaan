using UnityEngine;

public class Images : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.instance.isGameActive)
        {
            EventHandler.CallDecreaseHealthEvent();
        }
    }
}
