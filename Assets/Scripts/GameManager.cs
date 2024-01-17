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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // private void Start()
    // {
    //     // isGameActive = true;
    // }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
