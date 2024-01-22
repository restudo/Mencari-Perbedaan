using System.Collections;
using UnityEngine;

public class CrossSign : MonoBehaviour
{
    [SerializeField] private int waitFor;
    private Pooler pooler;
    private Transform parent;

    private void Awake()
    {
        parent = transform.parent;
        pooler = parent.GetComponent<Pooler>();
    }

    private void OnEnable()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitFor);

        gameObject.transform.SetParent(parent);
        pooler.pool.Release(gameObject);
    }
}
