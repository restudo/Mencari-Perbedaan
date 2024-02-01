using System.Collections;
using UnityEngine;

public class CrossSign : MonoBehaviour
{
    [SerializeField] private int waitFor;
    private Pooler pool;
    private Transform parent;

    private void Awake()
    {
        parent = transform.parent;
        pool = parent.GetComponent<Pooler>();
    }

    private void OnEnable()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitFor);

        gameObject.transform.SetParent(parent);
        pool.ReturnObject(gameObject);
    }
}
