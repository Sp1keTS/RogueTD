using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager instance;
    
    [SerializeField] private AudioMixer mixer;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 100f));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 100f));
        SetSoundFXVolume(PlayerPrefs.GetFloat("SoundFXVolume", 100f));
    }
    
    private void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        mixer.SetFloat("MasterVolume", 80f * (volume - 1));
    }
    private void SetSoundFXVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        mixer.SetFloat("SoundFXVolume", 80f * (volume - 1));
    }
    private void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        mixer.SetFloat("MusicVolume", 80f * (volume - 1));
    }
    
}
