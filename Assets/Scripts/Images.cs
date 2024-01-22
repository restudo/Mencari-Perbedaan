using UnityEngine;

public class Images : MonoBehaviour
{
    [SerializeField] private Pooler pooler;
    // [SerializeField] private Images pair;
    private Vector3 worldPosition;

    private void OnMouseDown()
    {
        if (GameManager.instance.isGameActive && GameManager.instance.isThoucedActive)
        {
            EventHandler.CallDecreaseHealthEvent();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            GameObject g = pooler.pool.Get();
            g.transform.parent = transform;
            g.transform.position = worldPosition;
        }
    }
}
