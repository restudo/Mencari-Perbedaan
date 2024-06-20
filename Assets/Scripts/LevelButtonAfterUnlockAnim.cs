using UnityEngine;

public class LevelButtonAfterUnlockAnim : MonoBehaviour
{
    [SerializeField] private StageMenu stageMenu;

    public void Animate()
    {
        stageMenu.Animate();
    }
}
