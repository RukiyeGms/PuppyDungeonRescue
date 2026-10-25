using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    static SoundEffectManager instance;
    static AudioSource audioSource;
    static SoundEffectLibrary soundEffectLibrary;
    Slider SFXSlider;
    float sValue = 0.5f;


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


    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null) 
        {
            audioSource.pitch = 1;
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
        sValue = SFXSlider.value;
    }

    public static void ChangeSFXSlider(Slider slider)
    {
        if (slider != null) { slider.value = instance.sValue; }
        instance.SFXSlider = slider;
        instance.SFXSlider.onValueChanged.AddListener((value) => instance.OnValueChanged());
    }

    public static void PlayVoice(AudioClip ac, float pitch = 1f)
    {
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(ac);
    }
}
