using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // Singleton instance
    private AudioSource musicSource;
    private AudioSource audioSource;      // AudioSource to play sounds

    public AudioClip pickup;
    public AudioClip addIngredient;
    public AudioClip trash;
    public AudioClip makePotion;
    public AudioClip birdSummon;
    public AudioClip birdClicked;
    public AudioClip success;
    public AudioClip birdLeave;
    public AudioClip openBook;
    public AudioClip button;
    public AudioClip backgroundMusic;

    void Awake()
    {
        // Ensure that there's only one instance of the AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make sure this object persists across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate AudioManager objects
        }

        // Get the AudioSource component
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            audioSource = sources[0];  // First AudioSource for sound effects
            musicSource = sources[1];         // Second AudioSource for background music
            audioSource.volume = 0.2f;
        }
        else
        {
            Debug.LogError("AudioManager requires at least two AudioSource components.");
        }
    }

    void Start()
    {
        AudioManager.instance.PlayBackgroundMusic();
    }
    // Play a sound effect globally
    public void PlaySoundEffectPickUpItem()
    {
        if (audioSource != null && pickup != null)
        {
            audioSource.PlayOneShot(pickup);
        }
    }
    public void PlaySoundEffectAddIngredient()
    {
        if (audioSource != null && addIngredient != null)
        {
            audioSource.PlayOneShot(addIngredient);
        }
    }

    public void PlaySoundEffectTrash()
    {
        if (audioSource != null && trash != null)
        {
            audioSource.PlayOneShot(trash);
        }
    }
    public void PlaySoundEffectMakePotion()
    {
        if (audioSource != null && makePotion != null)
        {
            audioSource.PlayOneShot(makePotion);
        }
    }
    public void PlaySoundEffectBirdSummon()
    {
        if (audioSource != null && birdSummon != null)
        {
            audioSource.PlayOneShot(birdSummon);
        }
    }

    public void PlaySoundEffectBirdClicked()
    {
        if (audioSource != null && birdClicked != null)
        {
            audioSource.PlayOneShot(birdClicked);
        }
    }
    public void PlaySoundEffectSuccess()
    {
        if (audioSource != null && success != null)
        {
            audioSource.PlayOneShot(success);
        }
    }


    public void PlaySoundEffectBirdLeave()
    {
        if (audioSource != null && birdLeave != null)
        {
            audioSource.PlayOneShot(birdLeave);
        }
    }
    public void PlaySoundEffectOpenBook()
    {
        if (audioSource != null && openBook != null)
        {
            audioSource.PlayOneShot(openBook);
        }
    }

    public void PlaySoundEffectButton()
    {
        if (audioSource != null && button != null)
        {
            audioSource.PlayOneShot(button);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;  // Set the background music clip
            musicSource.loop = true;             // Loop the music
            musicSource.volume = 0.2f;
            musicSource.Play();                  // Start playing the music
        }
    }

    // Stop the background music
    public void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}
