using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // isGameActive = true;
    }
}
