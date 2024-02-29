using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class ImageControl : MonoBehaviour
{
    [SerializeField] private float initialShuffleDuration = 0.2f;
    [SerializeField] private float finalShuffleDuration = 0.5f;
    [SerializeField] private int minShuffle = 4;
    [SerializeField] private int maxShuffle = 7;


    [SerializeField] public GameObject[] images;
    [SerializeField] private Transform[] imagesEntryTransform;
    [SerializeField] private GameObject[] checkpointsLeft;
    [SerializeField] private GameObject[] checkpointsRight;
    private Pooler pool;
    [Tooltip("automatically filled while playing")][SerializeField] private GameObject[] objPools;

    private bool isSwitched = false;
    private GameObject imageLeftOrigin;
    private GameObject imageRightOrigin;
    private Vector3[] imagesOriginPos;
    private Vector3[] imagesOriginRot;

    private void OnEnable()
    {
        EventHandler.ImagesEntry += ImageSetupAnim;
        EventHandler.ChangeImageTransform += ChangeImageTransform;
        EventHandler.ResetImageTransform += ResetImageTransform;
    }

    private void OnDisable()
    {
        EventHandler.ImagesEntry -= ImageSetupAnim;
        EventHandler.ChangeImageTransform -= ChangeImageTransform;
        EventHandler.ResetImageTransform -= ResetImageTransform;
    }

    private void Start()
    {
        pool = GetComponent<Pooler>();

        PoolToArray();

        imageLeftOrigin = images[0];
        imageRightOrigin = images[1];

        imagesOriginPos = new Vector3[images.Length];
        imagesOriginRot = new Vector3[images.Length];

        imagesOriginPos[0] = images[0].transform.position;
        imagesOriginPos[1] = images[1].transform.position;
        imagesOriginRot[0] = images[0].transform.eulerAngles;
        imagesOriginRot[1] = images[1].transform.eulerAngles;

        images[0].transform.position = imagesEntryTransform[0].transform.position;
        images[1].transform.position = imagesEntryTransform[1].transform.position;
        images[0].transform.eulerAngles = imagesEntryTransform[0].transform.eulerAngles;
        images[1].transform.eulerAngles = imagesEntryTransform[1].transform.eulerAngles;
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

    private void ImageSetupAnim()
    {
        images[0].transform.DOMove(imagesOriginPos[0], 0.3f).SetEase(Ease.OutBack);
        images[1].transform.DOMove(imagesOriginPos[1], 0.3f).SetEase(Ease.OutBack).SetDelay(0.15f);

        images[0].transform.DORotate(imagesOriginRot[0], 0.3f).SetEase(Ease.OutBack);
        images[1].transform.DORotate(imagesOriginRot[1], 0.3f).SetEase(Ease.OutBack).SetDelay(0.15f);
    }

    private void ChangeImageTransform(ImageTransform imgTransform)
    {

        switch (imgTransform)
        {
            case ImageTransform.Flip:
                StartCoroutine(Flips());
                break;
            case ImageTransform.Switch:
                StartCoroutine(Switch());
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
        int numberOfIterations = UnityEngine.Random.Range(minShuffle - 2, maxShuffle - 2);
        float durationStep = (initialShuffleDuration - finalShuffleDuration) / (numberOfIterations - 1);

        for (int i = 0; i < numberOfIterations; i++)
        {
            if (!isSwitched)
            {
                ImageLeftFirst(i, durationStep);
                yield return new WaitForSeconds((initialShuffleDuration - (i * durationStep)) - 0.15f);
                ImageRightFirst(i, durationStep);
            }
            else
            {
                ImageRightFirst(i, durationStep);
                yield return new WaitForSeconds((initialShuffleDuration - (i * durationStep)) - 0.15f);
                ImageLeftFirst(i, durationStep);
            }

            foreach (GameObject objPool in objPools)
            {
                if (objPool.transform.parent == images[0].transform || objPool.transform.parent == images[1].transform)
                {
                    Vector3 scalerPool = objPool.transform.localScale;
                    scalerPool.x *= -1;
                    // objPool.transform.localScale = scalerPool;
                    objPool.transform.DOScaleX(scalerPool.x, initialShuffleDuration - (i * durationStep)).SetEase(Ease.OutCirc);
                }
            }

            yield return new WaitForSeconds(initialShuffleDuration - (i * durationStep));
        }

        GameManager.Instance.isThoucedActive = true;
    }

    private void ImageLeftFirst(int iteration, float durationStep)
    {
        Vector3 scaler = images[0].transform.localScale;
        scaler.x *= -1;

        images[0].transform.DOScaleX(scaler.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);

        foreach (GameObject point in checkpointsLeft)
        {
            Vector3 scalerPoint = point.transform.localScale;
            scalerPoint.x *= -1;
            point.transform.DOScaleX(scalerPoint.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
        }
    }

    private void ImageRightFirst(int iteration, float durationStep)
    {
        Vector3 scaler = images[1].transform.localScale;
        scaler.x *= -1;

        images[1].transform.DOScaleX(scaler.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);

        foreach (GameObject point in checkpointsRight)
        {
            Vector3 scalerPoint = point.transform.localScale;
            scalerPoint.x *= -1;
            point.transform.DOScaleX(scalerPoint.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
        }
    }

    /// <summary>
    /// switch position between two images (left and right)
    /// </summary>
    private IEnumerator Switch()
    {
        int numberOfIterations = UnityEngine.Random.Range(minShuffle, maxShuffle);
        float durationStep = (initialShuffleDuration - finalShuffleDuration) / (numberOfIterations - 1);

        for (int i = 0; i < numberOfIterations; i++)
        {
            Vector3 tempPos = images[0].transform.position;

            images[0].transform.DOJump(images[1].transform.position, 3f, 0, initialShuffleDuration - (i * durationStep), false).SetEase(Ease.OutQuad);
            images[1].transform.DOJump(tempPos, -3f, 0, initialShuffleDuration - (i * durationStep), false).SetEase(Ease.OutQuad);

            if (!isSwitched)
            {
                isSwitched = true;
            }
            else
            {
                isSwitched = false;
            }

            SwapImagesObject();

            yield return new WaitForSeconds(initialShuffleDuration - (i * durationStep));
        }

        if (numberOfIterations % 2 != 0)
        {
            SwapImagesObject();
            SwapImagesPosition();
        }

        GameManager.Instance.isThoucedActive = true;
    }

    private void SwapImagesObject()
    {
        GameObject tempObj = images[0];
        images[0] = images[1];
        images[1] = tempObj;
    }

    private void SwapImagesPosition()
    {
        Vector3 tempPos = images[0].transform.position;
        images[0].transform.position = images[1].transform.position;
        images[1].transform.position = tempPos;
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
