using UnityEngine;

public class LevelButtonAfterUnlockAnim : MonoBehaviour
{
    [SerializeField] private StageMenu stageMenu;
    [SerializeField] private GameObject sparkVfx;

    private void Start()
    {
        sparkVfx.SetActive(false);
    }

    public void Animate()
    {
        stageMenu.Animate();
    }

    public void PlayParticle()
    {
        sparkVfx.SetActive(true);
    }
}