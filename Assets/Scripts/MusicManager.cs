using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    AudioSource audioSource;
    public AudioClip backgroundAudio;
    [SerializeField] Slider slider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else { Destroy(gameObject); }
    }
    void Start()
    {
        if (backgroundAudio != null) { PlayBackground(false , backgroundAudio); }
        slider.onValueChanged.AddListener((value) => OnValueChanged());
    }

    //=null makes the parameter optional
    public static void PlayBackground(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            instance.audioSource.clip = audioClip;
        }
        if (instance.audioSource.clip != null) 
        {
            if (!resetSong) { instance.audioSource.Stop();  }
            instance.audioSource.Play();
        }
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;   
    }

    public void OnValueChanged()
    {
        SetVolume(slider.value);
    }


    public static void PauseBackground()
        {
            instance.audioSource.Pause();
        }
}
