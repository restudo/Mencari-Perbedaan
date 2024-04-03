using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class ImageControl : MonoBehaviour
{
    [SerializeField] private Hint hint;

    [SerializeField] private float initialShuffleDuration = 0.2f;
    [SerializeField] private float finalShuffleDuration = 0.5f;
    [SerializeField] private int minShuffle = 4;
    [SerializeField] private int maxShuffle = 7;


    [SerializeField] public GameObject[] images;
    [SerializeField] private Transform[] imagesEntryTransform;
    [SerializeField] private GameObject[] checkpointsLeft;
    [SerializeField] private GameObject[] checkpointsRight;
    [Tooltip("automatically filled while playing")][SerializeField] private GameObject[] objPools;
    private Pooler pool;
    private GameObject instantiatedHint;
    private GameObject instantiatedHintRight;
    private GameObject instantiatedHintLeft;
    private const string LEFT_SIDE = "Left Side";
    private const string RIGHT_SIDE = "Right Side";

    // private bool isSwitched = false;
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
        if (instantiatedHint == null)
        {
            instantiatedHint = GameObject.FindGameObjectWithTag("HintObject");
            if (instantiatedHint != null && instantiatedHint.GetComponent<SpriteRenderer>().sortingLayerName == LEFT_SIDE)
            {
                instantiatedHintLeft = instantiatedHint;
            }
            else if (instantiatedHint != null && instantiatedHint.GetComponent<SpriteRenderer>().sortingLayerName == RIGHT_SIDE)
            {
                instantiatedHintRight = instantiatedHint;
            }
        }

        if (instantiatedHintLeft != null)
        {
            DOTween.Kill(instantiatedHintLeft.transform); // pause the hint anim

            if (instantiatedHintLeft.transform.localScale.x < 0)
            {
                instantiatedHintLeft.transform.localScale = hint.initScaleOdd;
            }
            else if (instantiatedHintLeft.transform.localScale.x > 0)
            {
                instantiatedHintLeft.transform.localScale = hint.initScaleEven;
            }
        }
        else if (instantiatedHintRight != null)
        {
            DOTween.Kill(instantiatedHintRight.transform); // pause the hint anim

            if (instantiatedHintRight.transform.localScale.x < 0)
            {
                instantiatedHintRight.transform.localScale = hint.initScaleOdd;
            }
            else if (instantiatedHintRight.transform.localScale.x > 0)
            {
                instantiatedHintRight.transform.localScale = hint.initScaleEven;
            }
        }

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
            ImageLeftFirst(i, durationStep);
            yield return new WaitForSeconds((initialShuffleDuration - (i * durationStep)) - 0.15f);
            ImageRightFirst(i, durationStep);

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

        if (instantiatedHintLeft != null)
        {
            EventHandler.CallHintAnimEvent(instantiatedHintLeft.transform.localScale); // resume the hint anim
        }
        else if (instantiatedHintRight != null)
        {
            EventHandler.CallHintAnimEvent(instantiatedHintRight.transform.localScale); // resume the hint anim
        }
    }

    private void ImageLeftFirst(int iteration, float durationStep)
    {
        foreach (GameObject point in checkpointsLeft)
        {
            Vector3 scalerPoint = point.transform.localScale;
            scalerPoint.x *= -1;
            point.transform.DOScaleX(scalerPoint.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
        }

        if (instantiatedHintLeft != null)
        {
            Vector3 scalerHint = instantiatedHintLeft.transform.localScale;
            scalerHint.x *= -1;
            instantiatedHintLeft.transform.DOScaleX(scalerHint.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
        }

        Vector3 scaler = images[0].transform.localScale;
        scaler.x *= -1;
        images[0].transform.DOScaleX(scaler.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
    }

    private void ImageRightFirst(int iteration, float durationStep)
    {
        foreach (GameObject point in checkpointsRight)
        {
            Vector3 scalerPoint = point.transform.localScale;
            scalerPoint.x *= -1;
            point.transform.DOScaleX(scalerPoint.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
        }

        if (instantiatedHintRight != null)
        {
            Vector3 scalerHint = instantiatedHintRight.transform.localScale;
            scalerHint.x *= -1;
            instantiatedHintRight.transform.DOScaleX(scalerHint.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
        }

        Vector3 scaler = images[1].transform.localScale;
        scaler.x *= -1;
        images[1].transform.DOScaleX(scaler.x, initialShuffleDuration - (iteration * durationStep)).SetEase(Ease.OutCirc);
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

            // if (!isSwitched)
            // {
            //     isSwitched = true;
            // }
            // else
            // {
            //     isSwitched = false;
            // }

            SwapImagesObject();

            yield return new WaitForSeconds(initialShuffleDuration - (i * durationStep));
        }

        GameManager.Instance.isThoucedActive = true;

        if (instantiatedHintLeft != null)
        {
            EventHandler.CallHintAnimEvent(instantiatedHintLeft.transform.localScale); // resume the hint anim
        }
        else if (instantiatedHintRight != null)
        {
            EventHandler.CallHintAnimEvent(instantiatedHintRight.transform.localScale); // resume the hint anim
        }
    }

    private void SwapImagesObject()
    {
        (images[0], images[1]) = (images[1], images[0]);

        (checkpointsLeft, checkpointsRight) = (checkpointsRight, checkpointsLeft);

        if (instantiatedHintLeft != null && instantiatedHintRight == null)
        {
            (instantiatedHintLeft, instantiatedHintRight) = (instantiatedHintRight, instantiatedHintLeft);
        }
        else if (instantiatedHintRight != null && instantiatedHintLeft == null)
        {
            (instantiatedHintRight, instantiatedHintLeft) = (instantiatedHintLeft, instantiatedHintRight);
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
