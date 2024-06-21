using System.Collections;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;
    private SparkSpawner sparkSpawner;
    // [SerializeField] private ObjectPool<GameObject> _sparkVfxPool;

    private void Awake()
    {
        GameObject sparkSpawnerObject = GameObject.FindGameObjectWithTag("Spark Spawner");
        if (sparkSpawnerObject != null)
        {
            sparkSpawner = sparkSpawnerObject.GetComponent<SparkSpawner>();
        }
    }

    void OnEnable()
    {
        if (sparkSpawner != null)
        {
            StartCoroutine(DeactivateSparkVfxAfterTime());
        }
    }

    private IEnumerator DeactivateSparkVfxAfterTime()
    {
        float elapsedTime = 0f;
        while (elapsedTime < destroyTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //after the timer is over
        sparkSpawner.sparkVfxPool.Release(this.gameObject);
    }
}
