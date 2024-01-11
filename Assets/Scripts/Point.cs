using System.Collections;
using System.Collections.Generic;
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
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.isGameActive)
        {
            if (!spRend.enabled && col.enabled)
            {
                spRend.enabled = true;
                col.enabled = false;

                pair.GetComponent<SpriteRenderer>().enabled = true;
                pair.GetComponent<Collider2D>().enabled = true;

                EventHandler.CallDifferenceClickedEvent();
            }
        }
    }
}
