using UnityEngine;
using UnityEngine.UIElements;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    
    [SerializeField] private AudioSource musicSoursePrefab;
    
    private AudioSource currentMusic;

    public AudioSource CurrentMusic
    {
        get => currentMusic;
        
        set => currentMusic = value;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        var audiosourse = Instantiate(musicSoursePrefab, Vector3.zero, Quaternion.identity);
        
        audiosourse.clip = clip;
        audiosourse.volume = volume * PlayerPrefs.GetFloat("SoundFXVolume", 1f);
        audiosourse.Play();
        currentMusic = audiosourse;
    }

    public void StopMusic()
    {
        currentMusic.Stop();
        Destroy(currentMusic.gameObject);
    }
}
