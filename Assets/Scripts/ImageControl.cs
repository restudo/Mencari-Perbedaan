using UnityEngine;
using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class ImageControl : MonoBehaviour
{
    [SerializeField] public GameObject[] images;
    [SerializeField] private GameObject[] checkpointsLeft;
    [SerializeField] private GameObject[] checkpointsRight;
    private Pooler pool;
    [Tooltip("automatically filled while playing")][SerializeField] private GameObject[] objPools;

    private bool isSwitched = false;

    private void Start()
    {
        pool = GetComponent<Pooler>();

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
        List<Transform> objCounter = new List<Transform>();
        for (int i = 0; i < pool.transform.childCount; i++)
        {
            if (pool.gameObject.transform.GetChild(i).CompareTag("Cross"))
            {
                objCounter.Add(pool.gameObject.transform.GetChild(i));
            }
        }

        int totalPool = objCounter.Count;
        objPools = new GameObject[totalPool];
        for (int i = 0; i < objPools.Length; i++)
        {
            objPools[i] = objCounter[i].gameObject;
        }
    }

    private void ChangeImageTransform(ImageTransform imgTransform)
    {

        switch (imgTransform)
        {
            case ImageTransform.Flip:
                StartCoroutine(Flips());
                break;
            case ImageTransform.Switch:
                Switch();
                break;
            // case ImageTransform.RotateLeft:
            //     RotateLeft();
            //     break;
            // case ImageTransform.RotateRight:
            //     RotateRight();
            //     break;
            default:
                break;
        }

        Debug.Log(imgTransform);
    }

    /// <summary>
    /// Normal Flip
    /// </summary>
    // private void Flip()
    // {
    //     foreach (GameObject image in images)
    //     {
    //         Vector3 scaler = image.transform.localScale;
    //         scaler.x *= -1;
    //         // image.transform.localScale = scaler;
    //         image.transform.DOScaleX(scaler.x, 0.3f);
    //     }

    //     foreach (GameObject point in pointsLeft)
    //     {
    //         Vector3 scaler = point.transform.localScale;
    //         scaler.x *= -1;
    //         // point.transform.localScale = scaler;
    //         point.transform.DOScaleX(scaler.x, 0.3f);
    //     }

    //     foreach (GameObject point in pointsRight)
    //     {
    //         Vector3 scaler = point.transform.localScale;
    //         scaler.x *= -1;
    //         // point.transform.localScale = scaler;
    //         point.transform.DOScaleX(scaler.x, 0.3f);
    //     }

    //     foreach (GameObject objPool in objPools)
    //     {
    //         if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
    //         {
    //             Vector3 scaler = objPool.transform.localScale;
    //             scaler.x *= -1;
    //             // objPool.transform.localScale = scaler;
    //             objPool.transform.DOScaleX(scaler.x, 0.3f);
    //         }
    //     }
    // }

    /// <summary>
    /// Take turn Flip
    /// </summary>
    private IEnumerator Flips()
    {
        if (!isSwitched)
        {
            ImageLeftFirst();
            yield return new WaitForSeconds(0.2f);
            ImageRightFirst();
        }
        else
        {
            ImageRightFirst();
            yield return new WaitForSeconds(0.2f);
            ImageLeftFirst();
        }

        foreach (GameObject objPool in objPools)
        {
            if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
            {
                Vector3 scalerPool = objPool.transform.localScale;
                scalerPool.x *= -1;
                // objPool.transform.localScale = scalerPool;
                objPool.transform.DOScaleX(scalerPool.x, 0.3f);
            }
        }
    }

    private void ImageLeftFirst()
    {
        Vector3 scaler = images[0].transform.localScale;
        scaler.x *= -1;

        images[0].transform.DOScaleX(scaler.x, 0.3f);

        foreach (GameObject point in checkpointsLeft)
        {
            Vector3 scalerPoint = point.transform.localScale;
            scalerPoint.x *= -1;
            point.transform.DOScaleX(scalerPoint.x, 0.3f);
        }
    }

    private void ImageRightFirst()
    {
        Vector3 scaler = images[1].transform.localScale;
        scaler.x *= -1;

        images[1].transform.DOScaleX(scaler.x, 0.3f);

        foreach (GameObject point in checkpointsRight)
        {
            Vector3 scalerPoint = point.transform.localScale;
            scalerPoint.x *= -1;
            point.transform.DOScaleX(scalerPoint.x, 0.3f);
        }
    }

    /// <summary>
    /// switch position between two images (left and right)
    /// </summary>
    private void Switch()
    {
        Vector3 tempPos = images[0].transform.position;
        // images[0].transform.position = images[1].transform.position;
        // images[1].transform.position = tempPos;

        images[0].transform.DOJump(images[1].transform.position, 3f, 0, 0.5f, false);
        images[1].transform.DOJump(tempPos, -3f, 0, 0.5f, false);

        if (!isSwitched)
        {
            isSwitched = true;
        }
        else
        {
            isSwitched = false;
        }
    }

    // private void RotateLeft()
    // {
    //     foreach (GameObject image in images)
    //     {
    //         // image.transform.Rotate(0, 0, 90);
    //         image.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
    //     }

    //     foreach (GameObject point in points)
    //     {
    //         if (point.transform.localScale.x < 0)
    //         {
    //             // point.transform.Rotate(0, 0, 90);
    //             point.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
    //         }
    //         else
    //         {
    //             // point.transform.Rotate(0, 0, -90);
    //             point.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
    //         }
    //     }

    //     foreach (GameObject objPool in objPools)
    //     {
    //         if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
    //         {
    //             if (objPool.transform.localScale.x < 0)
    //             {
    //                 // objPool.transform.Rotate(0, 0, 90);
    //                 objPool.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
    //             }
    //             else
    //             {
    //                 // objPool.transform.Rotate(0, 0, -90);
    //                 objPool.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
    //             }
    //         }

    //     }
    // }

    // private void RotateRight()
    // {
    //     foreach (GameObject image in images)
    //     {
    //         // image.transform.Rotate(0, 0, -90);
    //         image.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
    //     }

    //     foreach (GameObject point in points)
    //     {
    //         if (point.transform.localScale.x < 0)
    //         {
    //             // point.transform.Rotate(0, 0, -90);
    //             point.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
    //         }
    //         else
    //         {
    //             // point.transform.Rotate(0, 0, 90);
    //             point.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
    //         }
    //     }

    //     foreach (GameObject objPool in objPools)
    //     {
    //         if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
    //         {
    //             if (objPool.transform.localScale.x < 0)
    //             {
    //                 // objPool.transform.Rotate(0, 0, -90);
    //                 objPool.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.3f, RotateMode.LocalAxisAdd);
    //             }
    //             else
    //             {
    //                 // objPool.transform.Rotate(0, 0, 90);
    //                 objPool.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f, RotateMode.LocalAxisAdd);
    //             }
    //         }
    //     }
    // }

    private void ResetImageTransform()
    {
        foreach (GameObject image in images)
        {
            // image.transform.rotation = Quaternion.identity;
            // image.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);

            if (image.transform.localScale.x < 0)
            {
                Vector3 scaler = image.transform.localScale;
                scaler.x *= -1;
                // image.transform.localScale = scaler;
                image.transform.DOScaleX(scaler.x, 0.3f);
            }
        }

        foreach (GameObject point in checkpointsLeft)
        {
            // point.transform.rotation = Quaternion.identity;
            // point.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);
            if (point.transform.localScale.x < 0)
            {
                Vector3 scaler = point.transform.localScale;
                scaler.x *= -1;
                // point.transform.localScale = scaler;
                point.transform.DOScaleX(scaler.x, 0.3f);
            }
        }

        foreach (GameObject point in checkpointsRight)
        {
            // point.transform.rotation = Quaternion.identity;
            // point.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);
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
                // objPool.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f);

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
