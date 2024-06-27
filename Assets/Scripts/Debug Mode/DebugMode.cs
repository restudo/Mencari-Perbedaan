using UnityEngine;

public class DebugMode : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSfx;

    private GameObject[] debugObj;

    public void ToggleDebugMode()
    {
        debugObj = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var item in debugObj)
        {
            SpriteRenderer rend = item.GetComponent<SpriteRenderer>();
            if(rend.enabled)
            {
                rend.enabled = false;
            }
            else
            {
                rend.enabled = true;
            }
        }
    }

    public void ExitApp()
    {
        AudioManager.Instance.PlaySFX(buttonSfx);

        Application.Quit();
    }
}
