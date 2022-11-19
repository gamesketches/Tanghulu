using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectType { PokeFruit, Success, Footstep};
public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioClip[] pokeClips;

    public AudioClip successClip;

    public AudioClip walkingClip;

    private List<AudioSource> audioSourcePool;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        audioSourcePool = new List<AudioSource>();
    }

    public void PlaySoundEffect(SoundEffectType soundType) { 
        switch(soundType) {
            case SoundEffectType.PokeFruit:
                PlayPokingSound();
                break;
            case SoundEffectType.Success:
                PlaySuccessSound();
                break;
        }
    }

    private void PlayPokingSound() {
        AudioClip randomPoke = pokeClips[Random.Range(0, pokeClips.Length)];
        PlaySound(randomPoke, Random.Range(0.9f, 1.2f));
    }

    private void PlaySuccessSound() {
        PlaySound(successClip);
    }

    private void PlaySound(AudioClip audioClip, float pitch = 1, float volume = 1) {
        AudioSource openSource = FindAudioSource();
        openSource.clip = audioClip;
        openSource.pitch = pitch;
        openSource.volume = volume;
        openSource.Play();
    }

    public IEnumerator PlayFootstep(float walkingTime) {
        AudioSource openSource = FindAudioSource();
        openSource.clip = walkingClip;
        openSource.Play();
        yield return new WaitForSeconds(walkingTime);
        openSource.Stop();
    }

    private AudioSource FindAudioSource() { 
        foreach(AudioSource src in audioSourcePool) { 
            if(!src.isPlaying) {
                return src;
            }
        }
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        audioSourcePool.Add(newSource);
        return newSource;
        }
}
