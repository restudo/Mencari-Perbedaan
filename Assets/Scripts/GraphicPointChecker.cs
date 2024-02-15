using UnityEngine;

public class GraphicPointChecker : MonoBehaviour
{
    [SerializeField] private Point[] duplicatePoints;

    private void Start()
    {
        int result = Random.Range(0, duplicatePoints.Length);
        for (int i = 0; i < duplicatePoints.Length; i++)
        {
            if(result != i)
            {
                duplicatePoints[i].isDuplicate = true;
                duplicatePoints[i].pair.isDuplicate = true;
            }
        }

    }
}
