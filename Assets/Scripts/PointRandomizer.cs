using System.Collections.Generic;
using UnityEngine;

public class PointRandomizer : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private Point[] duplicatePoints;
    private List<Point> finalPointSpots;
    private List<Point> leftPointSpots;
    private List<int> selectedIndices;

    private void Start()
    {
        duplicatePoints = new Point[transform.childCount];

        finalPointSpots = new List<Point>();
        leftPointSpots = new List<Point>();
        selectedIndices = new List<int>();

        for (int i = 0; i < transform.childCount; i++)
        {
            duplicatePoints[i] = transform.GetChild(i).GetComponent<Point>();
        }

        int progress = levelManager.GetProgress();
        int totalPoints = duplicatePoints.Length;

        // Randomly select points without duplicates
        while (selectedIndices.Count < progress && selectedIndices.Count < totalPoints)
        {
            int randomIndex = Random.Range(0, totalPoints);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
            }
        }

        // Populate final and left point spots
        for (int i = 0; i < totalPoints; i++)
        {
            if (selectedIndices.Contains(i))
            {
                finalPointSpots.Add(duplicatePoints[i]);
            }
            else
            {
                leftPointSpots.Add(duplicatePoints[i]);
            }
        }

        // Set points based on their classification
        foreach (Point finalPoint in finalPointSpots)
        {
            finalPoint.SetFinalPoints();
        }

        foreach (Point leftPoint in leftPointSpots)
        {
            leftPoint.SetLeftPoints();
        }
    }
}
