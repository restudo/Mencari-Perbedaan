using UnityEngine;
using System;

public class ImageControl : MonoBehaviour
{
    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject[] points;

    private void OnEnable()
    {
        EventHandler.ChangeImageTransform += ChangeImageTransform;
        EventHandler.ResetImageTransform += ResetImageTransform;
    }

    private void OnDisable()
    {
        EventHandler.ChangeImageTransform -= ChangeImageTransform;
        EventHandler.ResetImageTransform -= ResetImageTransform;
    }

    private void ChangeImageTransform()
    {
        ImageTransform imgTransform;

        do
        {
            imgTransform = GetRandomTransform();
        } while (imgTransform == ImageTransform.Flip &&
                 (images[0].transform.eulerAngles.z == 90f ||
                  images[0].transform.eulerAngles.z == 270));

        switch (imgTransform)
        {
            case ImageTransform.Flip:
                Flip();
                break;
            case ImageTransform.RotateLeft:
                RotateLeft();
                break;
            case ImageTransform.RotateRight:
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
            // if (point.transform.localScale.x < 0)
            // {
            //     point.transform.Rotate(0, 0, point.transform.localRotation.z);
            // }

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

    private void ResetImageTransform()
    {
        foreach (GameObject image in images)
        {
            image.transform.rotation = Quaternion.identity;

            if (image.transform.localScale.x < 0)
            {
                Vector3 scaler = image.transform.localScale;
                scaler.x *= -1;
                image.transform.localScale = scaler;
            }
        }

        foreach (GameObject point in points)
        {
            point.transform.rotation = Quaternion.identity;

            if (point.transform.localScale.x < 0)
            {
                Vector3 scaler = point.transform.localScale;
                scaler.x *= -1;
                point.transform.localScale = scaler;
            }
        }
    }
}
