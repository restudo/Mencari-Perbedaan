using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private GameObject pair;

    private SpriteRenderer spRend;
    private Collider2D col;

    private void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        transform.position = new Vector3(transform.position.x, transform.position.y, -Camera.main.nearClipPlane); //set the position to Camera nearClipPlane, so its always on top 
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.isGameActive && GameManager.instance.isThoucedActive)
        {
            if (spRend.enabled)
            {
                return;
            }
            else
            {
                spRend.enabled = true;
                // col.enabled = false;

                pair.GetComponent<SpriteRenderer>().enabled = true;
                // pair.GetComponent<Collider2D>().enabled = true;

                EventHandler.CallDifferenceClickedEvent();
            }
        }
    }
}
