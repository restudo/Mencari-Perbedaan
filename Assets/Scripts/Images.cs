using UnityEngine;

public class Images : MonoBehaviour
{
    [SerializeField] private Pooler pool;
    // [SerializeField] private Images pair;
    private Vector3 worldPosition;

    private void OnMouseDown()
    {
        if (GameManager.instance.isGameActive)
        {
            EventHandler.CallDecreaseHealthEvent();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            GameObject g = pool.GetObject();
            g.transform.parent = transform;
            g.transform.position = worldPosition;

            Debug.Log(worldPosition);
        }
    }
}
