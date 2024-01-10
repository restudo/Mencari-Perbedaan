using UnityEngine;
using System;
using UnityEditor.Tilemaps;

public class ImageControl : MonoBehaviour
{
    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject[] points;

    private void OnEnable()
    {
        EventHandler.ChangeImageTransform += ChangeImageTransform;
    }

    private void OnDisable()
    {
        EventHandler.ChangeImageTransform -= ChangeImageTransform;
    }

    private void ChangeImageTransform()
    {
        ImageTransform imgTransform = GetRandomTransform();

        switch (imgTransform)
        {
            case ImageTransform.flip:
                Flip();
                break;
            case ImageTransform.rotateLeft:
                RotateLeft();
                break;
            case ImageTransform.rotateRight:
                RotateRight();
                break;
            default:
                break;
        }

        Debug.Log(imgTransform);
    }

    private ImageTransform GetRandomTransform()
    {
        ImageTransform[] allImgTransform = (ImageTransform[])Enum.GetValues(typeof(ImageTransform));
        return allImgTransform[UnityEngine.Random.Range(0, allImgTransform.Length)];
    }

    private void Flip()
    {
        foreach (GameObject image in images)
        {
            Vector3 scaler = image.transform.localScale;
            scaler.x *= -1;
            image.transform.localScale = scaler;
        }

        foreach (GameObject point in points)
        {
            if (point.transform.localScale.x < 0)
            {
                point.transform.Rotate(0, 0, -point.transform.localRotation.z);
            }

            Vector3 scaler = point.transform.localScale;
            scaler.x *= -1;
            point.transform.localScale = scaler;
        }
    }

    private void RotateLeft()
    {
        foreach (GameObject image in images)
        {
            image.transform.Rotate(0, 0, 90);
        }

        foreach (GameObject point in points)
        {
            if (point.transform.localScale.x < 0)
            {
                point.transform.Rotate(0, 0, 90);
            }
            else
            {
                point.transform.Rotate(0, 0, -90);
            }
        }
    }

    private void RotateRight()
    {
        foreach (GameObject image in images)
        {
            image.transform.Rotate(0, 0, -90);
        }

        foreach (GameObject point in points)
        {
            if (point.transform.localScale.x < 0)
            {
                point.transform.Rotate(0, 0, -90);
            }
            else
            {
                point.transform.Rotate(0, 0, 90);
            }
        }
    }
}
