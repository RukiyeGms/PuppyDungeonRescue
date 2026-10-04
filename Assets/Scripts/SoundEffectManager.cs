using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    static SoundEffectManager instance;
    static AudioSource audioSource;
    static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] Slider SFXSlider;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

    }

    void Start()
    {
        SFXSlider.onValueChanged.AddListener((value) => OnValueChanged());
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null) 
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;    
    }

    public void OnValueChanged()
    {
        SetVolume(SFXSlider.value);
    }
}
