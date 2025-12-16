using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;
    
    [SerializeField] private AudioSource soundEffectObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundEffect(AudioClip clip, Transform spawnTransform, float volume = 1.0f)
    {
        var audiosourse = Instantiate(soundEffectObject, spawnTransform.position, Quaternion.identity);
        
        audiosourse.clip = clip;
        audiosourse.volume = volume * PlayerPrefs.GetFloat("SoundFXVolume", 1f);
        audiosourse.Play();
        var clipLength = audiosourse.clip.length;
        Destroy(audiosourse.gameObject, clipLength);
    }
}
