using UnityEngine;
using System;
using DG.Tweening;

public class ImageControl : MonoBehaviour
{
    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject[] points;
    [SerializeField] private Pooler pool;
    [Tooltip("automatically filled while playing")][SerializeField] private GameObject[] objPools;

    private void Start()
    {
        PoolToArray();
    }

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

    private void PoolToArray()
    {
        int totalPool = pool.transform.childCount;
        objPools = new GameObject[totalPool];
        for (int i = 0; i < objPools.Length; i++)
        {
            objPools[i] = pool.transform.GetChild(i).gameObject;
        }
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
            // image.transform.localScale = scaler;
            image.transform.DOScaleX(scaler.x, 0.3f);
        }

        foreach (GameObject point in points)
        {
            Vector3 scaler = point.transform.localScale;
            scaler.x *= -1;
            // point.transform.localScale = scaler;
            point.transform.DOScaleX(scaler.x, 0.3f);
        }

        foreach (GameObject objPool in objPools)
        {
            if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
            {
                Vector3 scaler = objPool.transform.localScale;
                scaler.x *= -1;
                // objPool.transform.localScale = scaler;
                objPool.transform.DOScaleX(scaler.x, 0.3f);
            }
        }
    }

    private void RotateLeft()
    {
        foreach (GameObject image in images)
        {
            // image.transform.Rotate(0, 0, 90);
            image.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
        }

        foreach (GameObject point in points)
        {
            if (point.transform.localScale.x < 0)
            {
                // point.transform.Rotate(0, 0, 90);
                point.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
            }
            else
            {
                // point.transform.Rotate(0, 0, -90);
                point.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
            }
        }

        foreach (GameObject objPool in objPools)
        {
            if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
            {
                if (objPool.transform.localScale.x < 0)
                {
                    // objPool.transform.Rotate(0, 0, 90);
                    objPool.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
                }
                else
                {
                    // objPool.transform.Rotate(0, 0, -90);
                    objPool.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
                }
            }

        }
    }

    private void RotateRight()
    {
        foreach (GameObject image in images)
        {
            // image.transform.Rotate(0, 0, -90);
            image.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
        }

        foreach (GameObject point in points)
        {
            if (point.transform.localScale.x < 0)
            {
                // point.transform.Rotate(0, 0, -90);
                point.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
            }
            else
            {
                // point.transform.Rotate(0, 0, 90);
                point.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
            }
        }

        foreach (GameObject objPool in objPools)
        {
            if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
            {
                if (objPool.transform.localScale.x < 0)
                {
                    // objPool.transform.Rotate(0, 0, -90);
                    objPool.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
                }
                else
                {
                    // objPool.transform.Rotate(0, 0, 90);
                    objPool.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
                }
            }
        }
    }

    private void ResetImageTransform()
    {
        foreach (GameObject image in images)
        {
            // image.transform.rotation = Quaternion.identity;
            image.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);

            if (image.transform.localScale.x < 0)
            {
                Vector3 scaler = image.transform.localScale;
                scaler.x *= -1;
                // image.transform.localScale = scaler;
                image.transform.DOScaleX(scaler.x, 0.3f);
            }
        }

        foreach (GameObject point in points)
        {
            // point.transform.rotation = Quaternion.identity;
            point.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);
            if (point.transform.localScale.x < 0)
            {
                Vector3 scaler = point.transform.localScale;
                scaler.x *= -1;
                // point.transform.localScale = scaler;
                point.transform.DOScaleX(scaler.x, 0.3f);
            }
        }

        foreach (GameObject objPool in objPools)
        {
            if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
            {
                // objPool.transform.rotation = Quaternion.identity;
                objPool.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);

                if (objPool.transform.localScale.x < 0)
                {
                    Vector3 scaler = objPool.transform.localScale;
                    scaler.x *= -1;
                    // objPool.transform.localScale = scaler;
                    objPool.transform.DOScaleX(scaler.x, 0.3f);
                }
            }
        }
    }
}
