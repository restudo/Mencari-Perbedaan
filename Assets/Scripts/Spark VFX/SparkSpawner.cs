using UnityEngine;
using UnityEngine.Pool;

public class SparkSpawner : MonoBehaviour
{
    public ObjectPool<GameObject> sparkVfxPool;
    [SerializeField] private GameObject sparkVfxObject;
    private Vector3 _pointPos;

    private void OnEnable()
    {
        EventHandler.SetToPointPosition += SetToPointPosition;
    }

    private void OnDisable()
    {
        EventHandler.SetToPointPosition -= SetToPointPosition;
    }

    private void Start()
    {
        sparkVfxPool = new ObjectPool<GameObject>(CreateSparkVfx, OnTakeSparkVfxFromPool, OnReturnSparkVfxFromPool, OnDestroySparkVfx, true, 10, 15);
    }

    private GameObject CreateSparkVfx()
    {
        GameObject sparkSfx = Instantiate(sparkVfxObject, transform.parent);

        return sparkSfx;
    }

    private void OnTakeSparkVfxFromPool(GameObject sparkVfxObj)
    {
        sparkVfxObj.transform.position = _pointPos;

        sparkVfxObj.SetActive(true);
    }

    private void OnReturnSparkVfxFromPool(GameObject sparkVfxObj)
    {
        sparkVfxObj.SetActive(false);
    }

    private void OnDestroySparkVfx(GameObject sparkVfxObj)
    {
        Destroy(sparkVfxObj);
    }

    public void SetToPointPosition(Vector3 pointPos)
    {
        _pointPos = pointPos;
    }
}
