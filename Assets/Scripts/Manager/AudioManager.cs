using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    // [SerializeField] private AudioSource sfxSourceObject;

    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
    // {
    //     //spawn in gameObject
    //     AudioSource audioSource = Instantiate(sfxSourceObject, spawnTransform.position, Quaternion.identity);

    //     //assign the audioCtip
    //     audioSource.clip = audioClip;

    //     //assign volume
    //     audioSource.volume = volume;

    //     // play sound
    //     audioSource.Play();

    //     //get length of sound FX clip
    //     float clipLength = audioSource.clip.length;

    //     //destroy the clip after it is done playing
    //     Destroy(audioSource.gameObject, clipLength);
    // }

    public void PlaySFX(AudioClip clip, float volume)
    {
        // StopSFX();

        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip)
    {
        // StopSFX();
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(clip);
    }

    public AudioSource PlaySFXAndGetSource(AudioClip clip)
    {
        PlaySFX(clip);

        return sfxSource;
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.DOFade(volume, 0.1f);
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }
}
